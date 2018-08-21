using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void goToGame(){
		Debug.Log ("Game");

		Application.LoadLevel (1);
	}

	public void goToMenu(){
		if (GameObject.FindWithTag ("Player") == null) {
			Time.timeScale=1f;
			Debug.Log ("Menu");
			Application.LoadLevel (0);
		}
	}
}
