using UnityEngine;
using System.Collections;

public class LookAtMouse : MonoBehaviour
{
	
	// speed is the rate at which the object will rotate
	public float speed;
	public float mouseAdjustSpeed;

	public float acceleration;

	public float maxspeed;


	private float velocity=0.0f;



	private Vector3 rot;

	private float ztransspeed=1;

	void Start(){
		Debug.Log ("Starting");
		RenderSettings.ambientIntensity = 0.0f;
	}

	void FixedUpdate () 
	{
	
		//Debug.Log("Mousex " + Input.mousePosition.x);
		//Debug.Log("Mousey " + Input.mousePosition.y);
		//Debug.Log ("Screen Width " + Screen.width / 2);

		/*Vector3 rot = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
		if (Input.mousePosition.x > Screen.width / 2) {
			rot.Set(transform.rotation.x, transform.rotation.y+speed, transform.rotation.z);
			if(transform.rotation.y>(360-speed)){
				rot.Set(transform.rotation.x, 360-(speed-transform.rotation.y), transform.rotation.z);
				Debug.Log("correcting >");

			}


		}
		if (Input.mousePosition.x < Screen.width / 2) {
			rot.Set(transform.rotation.x, transform.rotation.y-speed, transform.rotation.z);
			if(transform.rotation.y<(speed)){
				rot.Set(transform.rotation.x, speed-(360-transform.rotation.y), transform.rotation.z);
				Debug.Log("correcting <");
			}
			
		}

		transform.Rotate(rot, Space.World);
*/
		float ztrans = 0.0f;
		if (transform.rotation.eulerAngles.z > 1.0f && transform.eulerAngles.z < 180.0f) {
			ztrans = -1.0f;
		}else if (transform.rotation.eulerAngles.z < 359.0f && transform.rotation.eulerAngles.z > 180.0f) {
			ztrans = 1.0f;
		}


		//rot = new Vector3(-Input.mousePosition.y+(Screen.height/2), Input.mousePosition.x-(Screen.height/2), ztrans);
		rot = new Vector3(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"), ztrans);


		transform.Rotate(rot * speed * Time.deltaTime, Space.Self);

		if (Input.GetKey ("left shift")&&velocity<maxspeed) {
			velocity+=acceleration;
		}
		if (Input.GetKey ("left ctrl")&&velocity>0.0f) {
			velocity-=acceleration;
		}
		Debug.Log (velocity);

		Vector3 movement = new Vector3 (0.0f, 0.0f, velocity);

		transform.Translate (movement * Time.deltaTime, Space.Self);

		GameObject[] engines = GameObject.FindGameObjectsWithTag("Engine Effects");

		for (int i=0; i<4; i++) {
			engines[i].GetComponent<ParticleSystem>().startSpeed=(velocity*1.5f);
		}




	
	}
}
