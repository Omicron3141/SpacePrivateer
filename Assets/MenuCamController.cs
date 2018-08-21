using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuCamController : MonoBehaviour {

	public Vector3 pos1;

	public Vector3 pos2;

	public float speed;

	private Vector3 target;

	// Use this for initialization
	void Start () {
		target = pos1;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp (transform.position, target, speed * Time.deltaTime);
		if ((transform.position-target).magnitude<50f) {
			if(target.Equals(pos1)){
				GameObject.Find ("StartButton").GetComponent<Button>().interactable = true;
				GameObject.Find ("CreditButton").GetComponent<Button>().interactable = true;
			}else{
				GameObject.Find ("BackButton").GetComponent<Button>().interactable = true;
			}
		}
	}

	public void MainMenu(){
		target = pos1;
		GameObject.Find ("StartButton").GetComponent<Button>().interactable = false;
		GameObject.Find ("CreditButton").GetComponent<Button>().interactable = false;
		GameObject.Find ("BackButton").GetComponent<Button>().interactable = false;
	}

	public void Credits(){
		target = pos2;
		GameObject.Find ("StartButton").GetComponent<Button>().interactable = false;
		GameObject.Find ("CreditButton").GetComponent<Button>().interactable = false;
		GameObject.Find ("BackButton").GetComponent<Button>().interactable = false;	}
}
