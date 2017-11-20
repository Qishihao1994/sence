using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunA : GunManager {

	public override void attribute ()
	{
		_BulletNum = 50;
		_MaxBullet = 50;
		_damage = 20f;
		_ShotSpeed = 0.2f;
		bullet=Resources.Load ("Prefabs/Bolt")as GameObject;
	}
//	public override void mShot ()
//	{
//		base.mShot ();
//
//	}
//	public override void mHZD()
//	{
//		base.mHZD ();
//	}
	public override IEnumerator mChangeBullet()
	{
		for(float i=0;i<3f;i+=Time.deltaTime)
			yield return 0;
		_BulletNum = _MaxBullet;
		print("A装弹完成");
	}
	public override float GetDamage()
	{
		return _damage;
	}
 }
