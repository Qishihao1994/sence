using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeHandMod : MonoBehaviour {
	SteamVR_ControllerManager manger;
	SteamVR_TrackedObject lefthand,righthand;
	public Transform gunModel;
	// Use this for initialization
	void Start () {
		manger = GetComponent<SteamVR_ControllerManager> ();
		if (manger != null) {
			lefthand = manger.left.GetComponent<SteamVR_TrackedObject> ();
			righthand = manger.right.GetComponent<SteamVR_TrackedObject> ();
		}
		if (lefthand != null && gunModel) {
			ImportGun (gunModel.transform, lefthand.transform);
		}
		if (righthand != null && gunModel) {
			ImportGun (gunModel.transform, righthand.transform);
		}
	}
	void ImportGun(Transform gun,Transform par)
	{
		Transform gunMod = Instantiate (gun, par)as Transform;
		gunMod.localPosition = Vector3.zero;
		gunMod.localRotation = Quaternion.identity;
		gunMod.localScale = Vector3.one;
	}


	// Update is called once per frame
	void Update () {
		
	}
}
