
using System;
using System.Collections.Generic;
using UnityEngine;

using UnityRoyale;



[Serializable]
public partial class MyPlaceable
{
		public uint id;

		public string name;

		public Placeable.PlaceableType pType;

		public string associatedPrefab;

		public string alternatePrefab;

		public ThinkingPlaceable.AttackType attackType;

		public Placeable.PlaceableTarget targetType;

	    /// <summary>
	    /// 攻击速度也就是间隔
	    /// </summary>
		public float attackRatio;

		public float damagePerAttack;

		public float attackRange;

	/// <summary>
	/// 血量
	/// </summary>
		public float hitPoints;

		public string attackClip;

		public string dieClip;

		public float speed;

		public float lifeTime;

		public float damagePerSecond;




}

[Serializable]
public partial class MyPlaceableModel
{
	public List<MyPlaceable> list = new List<MyPlaceable>();

	public MyPlaceableModel()
	{
		list.Add(new MyPlaceable(){
			id = 10000,
			name = "Archer",
			pType = Placeable.PlaceableType.Unit,
			associatedPrefab = "Archer Red",
			alternatePrefab = "Archer Blue",
			attackType = ThinkingPlaceable.AttackType.Ranged,
			targetType = Placeable.PlaceableTarget.Both,
			attackRatio = 1.5f,
			damagePerAttack = 1f,
			attackRange = 6f,
			hitPoints = 8f,
			attackClip = "",
			dieClip = "",
			speed = 4f,
			lifeTime = 5f,
			damagePerSecond = 1f,

		});

		list.Add(new MyPlaceable(){
			id = 10001,
			name = "Mage",
			pType = Placeable.PlaceableType.Unit,
			associatedPrefab = "Mage Red",
			alternatePrefab = "Mage Blue",
			attackType = ThinkingPlaceable.AttackType.Ranged,
			targetType = Placeable.PlaceableTarget.Both,
			attackRatio = 1.5f,
			damagePerAttack = 3f,
			attackRange = 4f,
			hitPoints = 10f,
			attackClip = "",
			dieClip = "",
			speed = 3f,
			lifeTime = 5f,
			damagePerSecond = 1f,
		});

		list.Add(new MyPlaceable(){
			id = 10002,
			name = "Warrior",
			pType = Placeable.PlaceableType.Unit,
			associatedPrefab = "Warrior Red",
			alternatePrefab = "Warrior Blue",
			attackType = ThinkingPlaceable.AttackType.Melee,
			targetType = Placeable.PlaceableTarget.Both,
			attackRatio = 2.5f,
			damagePerAttack = 4f,
			attackRange = 2.5f,
			hitPoints = 20f,
			attackClip = "",
			dieClip = "",
			speed = 2f,
			lifeTime = 5f,
			damagePerSecond = 1f,

		});


	}

	public static MyPlaceableModel instance = new MyPlaceableModel();
}
