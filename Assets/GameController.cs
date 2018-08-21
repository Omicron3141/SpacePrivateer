using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject enemy;
	public GameObject ally;

	public float boundRad;

	public float sDelay;

	public float initialSpawn;

	public float enemyPercent;

	public bool muteExplosions;


	private float sTimer;


	// Use this for initialization
	void Start () {
		sTimer = sDelay;
		for (int i=0; i<initialSpawn; i++) {
			createRandomShip();
		}
	}
	
	// Update is called once per frame
	void Update () {
		sTimer-=Time.deltaTime;
		if (sTimer < 0) {
			createRandomShip();
			sTimer=sDelay;
		}
	}

	void createRandomShip(){

		Vector3 rand = new Vector3 (Random.Range (-boundRad, boundRad), Random.Range (-boundRad, boundRad), Random.Range (-boundRad, boundRad));

		int randface = Random.Range (1, 6);
		if (randface == 1) {
			rand.x=boundRad;
		}else if (randface == 2) {
			rand.x=-boundRad;
		}else if (randface == 3) {
			rand.y=boundRad;
		}else if (randface == 4) {
			rand.y=-boundRad;
		}else if (randface == 5) {
			rand.z=boundRad;
		}else if (randface == 6) {
			rand.z=-boundRad;
		}
		if (Random.value <= enemyPercent) {
			Debug.Log("Enemy Spawned");
			Instantiate (enemy, rand, Random.rotation);
		} else {
			Debug.Log("Ally Spawned");
			Instantiate (ally, rand, Random.rotation);
		}
	}


}
