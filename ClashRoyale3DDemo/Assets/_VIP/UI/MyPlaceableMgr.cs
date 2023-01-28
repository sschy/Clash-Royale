using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using System;

public partial class MyPlaceable
{
    public UnityRoyale.Placeable.Faction faciton = UnityRoyale.Placeable.Faction.None;//阵营
    public MyPlaceable Clone()
    {
        //　简单来说，new仅申请开辟内存，而后调用类的构造函数来初始化对象。
        //　clone也是先开辟内存，但不会调用构造函数来初始化，而是将源对象值复制给目标对象。
        //　若是深拷贝，则还需要一同拷贝源对象引用类型指向的内存，浅拷贝则只是拷贝引用本身。
        return (MyPlaceable)this.MemberwiseClone();//克隆这个类
    }
}

public class MyPlaceableMgr : MonoBehaviour
{
    public static MyPlaceableMgr instance;
    public List<MyPlaceableView> mine = new List<MyPlaceableView>();
    public List<MyPlaceableView> his = new List<MyPlaceableView>();
    public Transform trHisTower;//国王塔地点
    //投掷物
    public List<MyProjectile> myProjList = new List<MyProjectile>();
    public List<MyProjectile> hisProjList = new List<MyProjectile>();
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        his.Add(trHisTower.GetComponent<MyPlaceableView>());
    }
    private void Update()
    {
        for (int i = 0; i < mine.Count; i++)
        {
            MyPlaceableView view = mine[i];//获取兵种数据
            MyPlaceable data = view.data;
            MyAIBase ai = view.GetComponent<MyAIBase>();//获取兵种AI
            NavMeshAgent nav = ai.GetComponent<NavMeshAgent>();
            Animator animator = view.GetComponent<Animator>();
            //根据当前兵种AI当前状态执行状态机，这里没使用复杂的状态机
            switch (ai.state)
            {
                case AIState.Idle:
                    //找场景内最近的敌人
                    ai.target = FindNearestEnemy(ai.transform.position, data.faciton);
                    if (ai.target != null)
                    {
                        print($"找到最近的敌人{ai.gameObject.name}");
                        ai.state = AIState.Seek;
                        nav.enabled = true;
                        animator.SetBool("IsMoving", true);
                    }
                    //检测是否有敌人在范围内
                    //GetComponent<CanvasGroup>().DOFade();
                    //有敌人切换到Seek状态
                    break;
                case AIState.Seek:
                    //往敌人方向前进
                    nav.destination = trHisTower.position;//设置目标点                               
                                                          //是否在攻击范围内
                    if (ISInAttackRange(view.transform.position, ai.target.transform.position, view.data.attackRange))
                    {
                        nav.enabled = false;//停止移动
                        //进去攻击状态
                        ai.state = AIState.Attack;
                    }
                    //有进入攻击状态
                    break;
                case AIState.Attack:
                    if (ai.target.GetComponent<MyPlaceableView>().data.hitPoints<=0)
                    {
                        ai.state = AIState.Idle;
                        break;
                    }
                    if (ISInAttackRange(view.transform.position, ai.target.transform.position, view.data.attackRange) == false)
                    {
                        ai.state = AIState.Idle;
                        break;
                    }
                    //如果当前时间小于上次攻击时间加攻击间隔， 其实就是攻击冷却还没好不攻击
                    if (Time.time < ai.lastBlowTime + data.attackRatio)
                    {
                        break;
                    }
                    ai.lastBlowTime = Time.time;
                    //攻击
                    animator.SetTrigger("Attack");
                    //伤害结算

                    //血量低于0
                    if (ai.target.GetComponent<MyPlaceableView>().data.hitPoints < 0)
                    {
                        ai.state = AIState.Die;
                        ai.target.GetComponent<MyPlaceableView>().data.hitPoints = 0;
                        if (ai.target.GetComponent<Animator>() != null)
                        {
                            ai.target.GetComponent<Animator>().SetTrigger("IsDead");
                            print($"{ai.target.gameObject.name}已经死亡");
                        }

                    }
                    //不在切换为IDle
                    break;
                case AIState.Die:

                    break;
            }
        }
        //子弹飞行与命中运算
        for (int i = 0; i < myProjList.Count; i++)
        {
            //遍历每一个投掷物
            var proj = myProjList[i];
            //进度等于时间*速度
            proj.progress += Time.deltaTime * proj.speed;
            //transform默认在角度或者中心，提高一点飞行位置
            proj.transform.position = Vector3.Lerp(proj.caster.transform.position, proj.caster.target.transform.position + Vector3.up, proj.progress);
            if (proj.progress>=1)
            {
                MyUnitAI castertAI = proj.caster as MyUnitAI;
                MyAIBase targetAI=  proj.caster.target;
                //伤害计算
                castertAI.OnDealDameage();
                //敌人被击中目标血量小于0
                if (targetAI.GetComponent<MyPlaceableView>().data. hitPoints<=0)
                {
                    if (targetAI.state!=AIState.Die)
                    {
                        targetAI.state = AIState.Die;
                        Animator animator = targetAI.GetComponent<Animator>();
                        if (animator != null)
                        {
                            animator.SetTrigger("IsDead");
                        }
                      
                    }
                }
            }
        }
    }

    private bool ISInAttackRange(Vector3 mypos, Vector3 targetPos, float attackRange)
    {
        return Vector3.Distance(mypos, targetPos) < attackRange;
    }

    /// <summary>
    /// 玩家敌人通用查找方法
    /// </summary>
    /// <param name="faciton"></param>
    private MyAIBase FindNearestEnemy(Vector3 myPos, UnityRoyale.Placeable.Faction faciton)
    {
        List<MyPlaceableView> units = faciton == UnityRoyale.Placeable.Faction.Player ? his : mine;
        //找到最近的敌人
        float x = float.MaxValue;
        MyAIBase nearest = null;
        foreach (MyPlaceableView unit in units)
        {
            float d = Vector3.Distance(unit.transform.position, myPos);
            if (d < x)
            {
                x = d;
                nearest = unit.GetComponent<MyAIBase>();
            }
        }
        return nearest;
    }
}
