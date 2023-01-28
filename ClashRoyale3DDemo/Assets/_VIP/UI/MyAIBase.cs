using System.Collections;
using UnityEngine;


public enum AIState
{

    Idle,
    Seek,
    Attack,
    Die
}


public class MyAIBase : MonoBehaviour
{
    public MyAIBase target = null;//攻击目标
    public AIState state = AIState.Idle;//默认状态Idle
    /// <summary>
    /// 上次攻击时间
    /// </summary>
    public float lastBlowTime;
    public virtual void OnIdle() { }
    public virtual void OnSeek() { }
    public virtual void OnAttack() { }
    public virtual void OnDie() { }
}
