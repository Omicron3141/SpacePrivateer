using UnityEngine;
using System.Collections;

public class TargetingSystem : MonoBehaviour {


	public Transform Target;
	public float range;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hitInfo = new RaycastHit();
		Vector3 showPos;
		if (Target != null) {
			Vector3 fwd = Target.transform.TransformDirection (Vector3.forward);

			if (Physics.Raycast (Target.transform.position, fwd, out hitInfo, range)) {
				showPos = hitInfo.point;
			} else {
				showPos = Target.transform.position + fwd * range;
			}
			//Camera.main.transform.position
			//Vector3 disttocam = showPos - Camera.main.transform.position;
			//Vector3 dirtocam = disttocam/disttocam.magnitude;


			//gameObject.transform.position.Set(showPos.x, showPos.y, 100.0f);
			gameObject.transform.position = showPos;

			gameObject.transform.LookAt (Camera.main.transform);
		}




	}
}
