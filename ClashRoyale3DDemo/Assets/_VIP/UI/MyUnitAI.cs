using UnityEngine;

/// <summary>
/// 可移动单位AI
/// </summary>
class MyUnitAI : MyAIBase
{
    public GameObject Projectile;//投掷物
    public Transform firePos;//发射位置

    /// <summary>
    /// 敌人扣血
    /// </summary>
    public void OnDealDameage()
    {
        this.target.GetComponent<MyPlaceableView>().data.hitPoints -= this.GetComponent<MyPlaceableView>().data.damagePerAttack;
        if (this.target.GetComponent<Animator>() != null)
        {
            this.target.GetComponent<MyPlaceableView>().data.hitPoints = 0;
        }
    }
    public void OnFireProjectile()
    {
       GameObject go= Instantiate(Projectile, firePos.position,Quaternion.identity);//放在手部位置（世界坐标）但是不跟着手部走，不以手部为父节点
        //赋值释放者用于其他搅拌
        go.GetComponent<MyProjectile>().caster = this;
       // go.GetComponent<MyProjectile>().targer = this.target;
        //添加投掷物到到管理器统一管理
        MyPlaceableMgr.instance.myProjList.Add(go.GetComponent<MyProjectile>());
    }
}

