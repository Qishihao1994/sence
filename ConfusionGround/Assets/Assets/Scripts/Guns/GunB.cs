using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunB : GunManager {
	public override void attribute ()
	{
		_BulletNum = 30;
		_MaxBullet = 30;
		_damage = 30f;
		_ShotSpeed = 0.5f;
		bullet=Resources.Load ("Prefabs/Ef_Enemy_Bullet_01")as GameObject;
	}
//	public override void mShot ()
//	{
//		base.mShot ();
//
//	}
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
		print("B装弹完成");
	}

}
