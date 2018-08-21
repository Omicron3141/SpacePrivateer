using UnityEngine;
using System.Collections;

public class ExplosionController : MonoBehaviour {
	public AudioClip Sound;
	
	private AudioSource source;

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource>();
		if (!GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ().muteExplosions) {
			source.PlayOneShot (Sound, 1f);

		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
