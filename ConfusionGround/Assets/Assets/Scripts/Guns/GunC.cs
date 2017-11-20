using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunC : GunManager {

	public override void attribute ()
	{
		_BulletNum = 10;
		_MaxBullet = 10;
		_damage = 100f;
		_ShotSpeed = 1f;
		bullet=Resources.Load ("Prefabs/Ef_GunFire_01")as GameObject;
	}
//	public override void mShot ()
//	{
//		base.mShot ();
//	}
//		
//	public override void mHZD()
//	{
//		base.mHZD ();
//
//	}
	public override IEnumerator mChangeBullet()
	{
		for(float i=0;i<3f;i+=Time.deltaTime)
			yield return 0;
		_BulletNum = _MaxBullet;
		print("c装弹完成");
	}
	public override float GetDamage()
	{
		return _damage;
	}
//		_BulletNum = _MaxBullet;
//		print (_BulletNum);

}
