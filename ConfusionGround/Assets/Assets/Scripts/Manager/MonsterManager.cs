using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager:MonoBehaviour  {
	protected  float _HP;
	protected float _AttckDamage;
	protected string _name;

	public  void attribute()
	{
		_HP = 100;
	}
	public float GetHp()
	{
		return _HP;
	}


}
