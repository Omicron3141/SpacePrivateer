using UnityEngine;
using System.Collections;

public class GUIPositionController : MonoBehaviour {

	public bool top;
	public bool left;

	public Vector3 offset;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		float x = 0f;
		float y = 0f;
		if (left) {
			x = -(Screen.width / 2.0f);
		} else {
			x = (Screen.width / 2.0f);
		}
		if (top) {
			y = (Screen.height / 2.0f);
		} else {
			y = -(Screen.height / 2.0f);
		}
		transform.position = new Vector3(x + offset.x, y + offset.y, 0.0f);
	}
}
