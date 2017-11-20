using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GunManager : MonoBehaviour {

	protected int _BulletNum;
	protected int _MaxBullet;
	protected float _damage;
	protected float _ShotSpeed;
	protected float _CurrenCD=0;
	protected GameObject bullet;
	protected TakeGuns _takeguns;
	MonsterManager monster;
	float hp;

	public virtual void attribute()
	{

	}

	public virtual void mShot()
	{
		_takeguns = GameObject.Find ("hand (1)").GetComponent<TakeGuns> ();
		_CurrenCD += Time.deltaTime*2;
		if (_CurrenCD >= _ShotSpeed&&Input.GetMouseButtonDown(0)) {
			if (_BulletNum > 0) {
				_BulletNum--;
				GameObject clone = Instantiate<GameObject> (bullet, _takeguns.Fpoint.position, _takeguns.Fpoint.rotation);
				clone.GetComponent<Rigidbody> ().AddForce (Camera.main.transform.TransformDirection (0, 0, 10) *500);
				Destroy (clone, 2);
				_takeguns._other.GetComponent<AudioSource> ().Play ();
				_CurrenCD = 0;
				AnimatorManage.GetInstate ().SetAnimator (_takeguns.ani, "shoot");
				Animator anim = _takeguns.gun.GetComponent<Animator> ();
				AnimatorManage.GetInstate ().SetAnimator (anim, "shoot");
				RaycastHit hit;
				if (Physics.Raycast (_takeguns.Fpoint.position, _takeguns.Fpoint.TransformDirection(Vector3.forward), out hit)) {
					if (hit.transform.tag.Equals("Enemy")) {
						hp-= _damage;
						print (hp);
						if (hp <= 0) {
							Destroy (hit.transform.gameObject,2);
						}
					}
				}
			}
			else {
				AudioManager.GetInstance ().PlayClip (_takeguns.source, "Audio/Zhuangdan/cocked", false);
			     }
		}
	}
	public virtual void mHZD()
	{
		if (_BulletNum >= _MaxBullet) {
			AudioManager.GetInstance ().PlayClip (_takeguns.source, "Audio/Zhuangdan/they shoot", false);
			return;
		   }
		AudioManager.GetInstance ().PlayClip (_takeguns.source, "Audio/Zhuangdan/reload", false);
		StartCoroutine ("mChangeBullet");
	}
	public virtual float GetDamage()
	{
		return _damage;
	}
	public virtual IEnumerator mChangeBullet()
	{
		yield return 0;
	}

	void Start()
	{
	    monster = new MonsterManager ();
		monster.attribute ();
		hp=monster.GetHp();
	}
}
