using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyProjectile : MonoBehaviour
{
    /// <summary>
    /// 投掷物释放者
    /// </summary>
    public MyAIBase caster;
    /// <summary>
    /// 目标
    /// </summary>
    //public MyAIBase targer;
    /// <summary>
    /// 飞行进度
    /// </summary>
    public float progress;
    /// <summary>
    /// 飞行速度
    /// </summary>
    public float speed;
}
