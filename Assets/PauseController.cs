using UnityEngine;
using System.Collections;

public class PauseController : MonoBehaviour {
	private float paused;
	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Canvas>().enabled=false;
		paused = 1f;
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (Input.GetAxis ("Cancel"));
		if (Input.GetAxis ("Cancel") == 1f && paused<=0 && GameObject.FindWithTag("Player")!=null) {
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			gameObject.GetComponent<Canvas>().enabled=true;
			Time.timeScale=0f;
		}
		paused-=Time.deltaTime;
	}

	public void resume(){
		Time.timeScale=1f;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		gameObject.GetComponent<Canvas> ().enabled = false;
		paused = 1f;


	}
}
