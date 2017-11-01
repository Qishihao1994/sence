using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///luohui is sb hahahahahahahahahahahah
<<<<<<< Updated upstream
/// //????
=======
/// 
/// 
/// 
/// 到此一游
/// ???????????
>>>>>>> Stashed changes
public class GodPersonCameraViewer : MonoBehaviour {
	private Transform ViewPoint;
	private Transform subViewPoint;
	private Transform CameraPoint;
	public Transform Player;
	private Vector3 ViewPointOffset;
	private Vector3 CameraStartPosition;
	public float CameraOffsetHeight=20,CameraOffsetLength=10;
	void Start(){
		subViewPoint = new GameObject ("SubViewPoint").transform;
		subViewPoint.SetParent (transform);
		ViewPoint = new GameObject ("ViewPoint").transform;
		ViewPoint.SetParent (transform);
		CameraPoint = new GameObject ("CameraPoint").transform;
		CameraPoint.SetParent (ViewPoint);
		Camera.main.transform.position = transform.position + new Vector3 (0, CameraOffsetHeight, CameraOffsetLength);
		Camera.main.transform.LookAt (ViewPoint);
		Camera.main.transform.SetParent (ViewPoint);

		CameraStartPosition = Camera.main.transform.position - ViewPoint.position;
		CameraPoint.position = Camera.main.transform.position;
		ViewPointOffset = ViewPoint.position - Player.position;
	}
	void Update(){
		ViewPoint.position = Player.position+ViewPointOffset;
		CameraPoint.rotation = Camera.main.transform.rotation;
		if (Input.GetKey(KeyCode.LeftAlt)) {
			subViewPoint.Rotate (Input.GetAxis("Mouse Y"),0,0,Space.Self);
			subViewPoint.Rotate (0,Input.GetAxis("Mouse X"),0,Space.World);
			subViewPoint.rotation=Quaternion.Euler (Mathf.Clamp(CheckAngle(subViewPoint.eulerAngles.x),-30,0),subViewPoint.eulerAngles.y,0);
		}
		CameraPoint.Translate (0,0,Input.GetAxis("Mouse ScrollWheel")*5,Space.Self);
		Camera.main.transform.position = Vector3.Lerp (Camera.main.transform.position,CameraPoint.position,Time.deltaTime*5);
		ViewPoint.rotation = Quaternion.Slerp (ViewPoint.rotation,subViewPoint.rotation,Time.deltaTime*5);

		if (Input.GetKeyDown(KeyCode.E)) {
			subViewPoint.Rotate (0,90,0,Space.World);
		}else if(Input.GetKeyDown(KeyCode.Q)){
			subViewPoint.Rotate(0,-90,0,Space.World);
		}
	}

	public float CheckAngle(float value)
	{
		float angle = value - 180;
		if (angle > 0)
			return angle - 180;
		return angle + 180;
	}
}
