using UnityEngine;
using System.Collections;

public class PlasmaController : MonoBehaviour {

	// Use this for initialization

	public float speed;

	public float range;

	private float total = 0.0f;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 movement = new Vector3 (0.0f, 0.0f, speed);
		
		transform.Translate (movement * Time.deltaTime, Space.Self);

		total += speed*Time.deltaTime;


		if (total > range) {
			Destroy(gameObject);
		}

		GetComponent<LensFlare> ().brightness = (10.0f-(total / (1.0f * range)))/10.0f;

	}
}
