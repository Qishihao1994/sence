using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManage {
	private static AnimatorManage _ani;
	public static AnimatorManage GetInstate()
	{
		if (_ani == null) {
			_ani = new AnimatorManage ();
		}
		return _ani;
	}

	public void SetAnimator(Animator ani,string name)
	{
		if (ani != null) {
			ani.SetTrigger(name);
		}
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
