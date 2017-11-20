using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightingSystem;



public class TakeGuns : MonoBehaviour {
	[HideInInspector]
	public Transform Fpoint;
	[HideInInspector]
	public bool istrue=false;
	[HideInInspector]
	public GunManager gun;
	[HideInInspector]
	public Collider _other;
	[HideInInspector]
	public Animator ani;
	[HideInInspector]
	public AudioSource source;
	GameObject line;

	void OnTriggerEnter (Collider other)
	{
		if (other.tag.Equals ("Weapon")&&!istrue) {
			Highlighter h = other.gameObject.GetComponent<Highlighter>();
			h.ConstantOn(Color.yellow);
		}
	}
	void OnTriggerStay (Collider other)
	{
		if (other.tag.Equals ("Weapon")) {
			if (Input.GetKeyDown (KeyCode.F) && !istrue) {
				other.gameObject.GetComponent<Highlighter> ().ConstantOffImmediate();
				_other = other;
				SetGunPossition ();
				line.GetComponent<LineRenderer>().widthMultiplier=1f; 
			}
		}
		if (istrue) {
			GiveUpGun ();
		}
	}
	void OnTriggerExit(Collider other)
	{
		if (other.tag.Equals ("Weapon")) {
			other.gameObject.GetComponent<Highlighter> ().ConstantOffImmediate ();
		}
	}

	public void SetGunPossition()
	{
		ani.SetBool ("isOpen", true);

		_other.gameObject.transform.parent = gameObject.transform.parent.parent.transform;
		_other.gameObject.transform.position = this.gameObject.transform.GetChild(0).position;
		_other.gameObject.transform.localEulerAngles = new Vector3 (0,90,0);
		istrue = true;
		GunAttribute ();
		if (_other.gameObject.GetComponent<Rigidbody> () != null) {
			Destroy (_other.gameObject.GetComponent<Rigidbody> ());
		}
	}

	public void GunAttribute()
	{

			string type = _other.gameObject.name;
			switch (type) {
			case "AssaultRifle09":
			_other.gameObject.AddComponent<GunA> ();
			gun = _other.GetComponent<GunA> ();
				gun.attribute ();
				break;
			case "AssaultRifle02":
			_other.gameObject.AddComponent<GunB> ();
			gun = _other.GetComponent<GunB> ();
				gun.attribute ();
				break;
		    case "AssaultRifle08":
			_other.gameObject.AddComponent<GunC> ();
			gun = _other.GetComponent<GunC> ();
				gun.attribute ();
				break;


		}
	}
	public void GiveUpGun()
	{
		if (Input.GetKeyDown (KeyCode.G)) {
			_other.gameObject.transform.parent = null;
			ani.SetBool ("isOpen", false);
			line.GetComponent<LineRenderer>().widthMultiplier=0f;
			if (_other.gameObject.GetComponent<Rigidbody> () == null) {
				_other.gameObject.AddComponent<Rigidbody> ();
			}
			istrue = false;
		}
	}

//	protected void 
	// Use this for initialization
	void Start () {
		Fpoint = GameObject.Find("Fpoint").transform;
		ani = GetComponent<Animator> ();
		source = GetComponent<AudioSource> ();
		line=GameObject.Find ("Line");
	}
	
	// Update is called once per frame
	void Update () {
		if(istrue)
		{
	    gun.mShot (); 
	if (Input.GetKeyDown (KeyCode.R)) {
				gun.mHZD ();
		}
	    }

	}

}
