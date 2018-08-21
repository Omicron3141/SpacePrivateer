using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class AIController : MonoBehaviour {

	public GameObject explosion1;
	public GameObject explosion2;
	public GameObject explosion3;

	private GameObject overlay;

	public bool enemy;

	public GameObject indicatortype;

	public Sprite enemyIndicator;

	public Sprite enemyIndicator2;

	private GameObject indicator;

	public GameObject target;
	
	public float rotationspeed;

	public float velocity;

	public float reloadtime;
	
	private float reloadtimer=0.0f;

	public GameObject bullet;

	public float health;

	public float damage;

	private int manuver;

	private Quaternion orRot;
	
	private float sTime;

	private float lastHit;

	private GameObject nearest;

	private float nearestTimer;

	private float initialvel;

	private float explosiontimer;

	private float indicatorTimer;

	private Vector3 indscale;
	





	

	// Use this for initialization
	void Start () {
		overlay = GameObject.Find ("Overlay");
		if (overlay != null) {
			indicator = Instantiate (indicatortype);
			indicator.gameObject.transform.parent = overlay.transform;
			indscale = indicator.transform.localScale;
		}
		chooseTarget ();
		lastHit = -3f;
		nearestTimer = 0f;
		initialvel = velocity;
		if (Time.timeSinceLevelLoad < 3.5f && GameObject.Find("Main Camera").GetComponent<MenuCamController>()==null){
			velocity *= 12f;

		}


	
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.timeSinceLevelLoad > 3.5f) {
			velocity=initialvel;
		}
		if (nearestTimer < 0f || nearest==null) {
			nearest = nearestShip ();
			nearestTimer = 1f;
		} else {
			nearestTimer -= Time.deltaTime;
		}
		avoid();

		if (Time.time - lastHit <= 2f) {
			evade ();
		} else if (health < 10) {
			flee ();
		} else { 
			attack ();
		}

		if (explosiontimer < 0f) {
			GameObject[] explosions = GameObject.FindGameObjectsWithTag("Explosion");
			foreach (GameObject ex in explosions){
				if((ex.transform.position-transform.position).magnitude<1000f){
					health-=12f-((ex.transform.position-transform.position).magnitude)/83f;
				}
			}
			
			if(health<=0){
				GameObject exp;
				float r = Random.Range(0.0f, 3.0f);
				if(r<=1.0f){
					exp = (GameObject)Instantiate(explosion1, transform.position, transform.rotation);
				}else if(r>=2.0f){
					exp = (GameObject)Instantiate(explosion2, transform.position, transform.rotation);
				}else{
					exp = (GameObject)Instantiate(explosion3, transform.position, transform.rotation);
				}
				
				Destroy(exp, 2.0f);
				Destroy(indicator);
				Destroy(gameObject);
				if(enemy){
					Debug.Log("Enemy Destroyed by Explosion");
				}else{
					Debug.Log("Ally Destroyed by Explosion");
				}
			}
			explosiontimer=1f;
		} else {
			explosiontimer -= Time.deltaTime;
		}

		if (Mathf.Abs (transform.position.x) > 5000f || Mathf.Abs (transform.position.y) > 5000f || Mathf.Abs (transform.position.z) > 5000f) {
			transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.LookRotation ((new Vector3(0f,0f,0f) - transform.position)), Time.deltaTime * rotationspeed * 50f);
		}

		if (indicator != null) {
			indicator.transform.position = transform.position;
			indicator.transform.LookAt (Camera.main.transform);
			if(indicator.transform.localScale.x>indscale.x){
				indicator.transform.localScale -= new Vector3(Time.deltaTime, Time.deltaTime, 0f);
			}
			if(target.Equals(GameObject.FindGameObjectWithTag("Player"))){
				indicator.transform.Rotate(new Vector3(3f, 3f, 3f)*Time.deltaTime, Space.Self);
			}
		}

	}

	void attack(){
		if (target == null) {
			Debug.Log("Target Destroyed. Retargeting");
			chooseTarget ();
		}
		if (targetScore(target)<2f) {
			Debug.Log("Target Deemed Suboptimal. Retargeting");
			chooseTarget();
		}
		Vector3 trans = new Vector3 (0.0f, 0.0f, velocity);
		transform.Translate (trans * Time.deltaTime, Space.Self);


		transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.LookRotation(target.transform.position-transform.position), Time.deltaTime * rotationspeed);
		if (Vector3.Angle(target.transform.position-transform.position, transform.rotation.eulerAngles)<100.0f && Vector3.Angle(target.transform.position-transform.position, transform.rotation.eulerAngles)>80.0f && reloadtimer < 0 && (target.transform.position-transform.position).magnitude<3000.0f) {
			//Debug.Log("Angle "+Vector3.Angle(target.transform.position-transform.position, transform.rotation.eulerAngles));
			Instantiate (bullet, transform.position+transform.TransformDirection(Vector3.forward)*15.0f, Quaternion.LookRotation(((target.transform.position+target.transform.rotation*Vector3.forward*velocity)-transform.position)));
			reloadtimer=reloadtime;
			
			
		} else {
			reloadtimer-=Time.deltaTime;
		}
	}

	void flee(){
		if (target == null) {
			Debug.Log("Target Destroyed. Retargeting");
			target = nearest;
		}
		Vector3 trans = new Vector3 (0.0f, 0.0f, velocity);
		transform.Translate (trans * Time.deltaTime, Space.Self);
		
		
		transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.LookRotation(-(target.transform.position-transform.position)), Time.deltaTime * rotationspeed);
	}

	void evade(){
		if (manuver == 1) {
			manuver = HalfLoop (manuver, orRot, sTime);
		} else if (manuver == 2) {
			manuver = RightTwirl (manuver, orRot, sTime);
		} else if (manuver == 3) {
			manuver = LeftTwirl (manuver, orRot, sTime);
		} else {
			manuver=Random.Range(0, 4);
			orRot = transform.rotation;
			sTime = Time.time;
		}
	}

	void avoid(){
		transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.LookRotation(-(nearest.transform.position-transform.position)), Time.deltaTime * rotationspeed*600f/(nearest.transform.position-transform.position).magnitude);
	}






	int HalfLoop(float m, Quaternion orRot, float sTime){
		if (Time.time - sTime < 1f) {
			transform.Rotate(new Vector3(-180f, 0f, 0f)*Time.deltaTime);
		} else {
			chooseTarget ();
			return 0;
		}
		Vector3 movement = new Vector3 (0.0f, 0.0f, velocity*2f);
		
		transform.Translate (movement * Time.deltaTime, Space.Self);
		return 1;
	}

	
	int RightTwirl(float m, Quaternion orRot, float sTime){
		if (Time.time - sTime < 2f) {
			transform.Rotate(new Vector3(-164f, 0f, -82f)*Time.deltaTime);
		} else {
			chooseTarget ();
			return 0;
		}
		Vector3 movement = new Vector3 (0.0f, 0.0f, velocity*0.7f);
		transform.Translate (movement * Time.deltaTime, Space.Self);
		transform.Translate (orRot*Vector3.right* 2f, Space.World);
		return 2;
	}
	
	int LeftTwirl(float m, Quaternion orRot, float sTime){
		if (Time.time - sTime < 2f) {
			transform.Rotate(new Vector3(-164f, 0f, 82f)*Time.deltaTime);
		} else {
			chooseTarget ();
			return 0;
		}
		Vector3 movement = new Vector3 (0.0f, 0.0f, velocity*0.7f);
		transform.Translate (movement * Time.deltaTime, Space.Self);
		transform.Translate (orRot*Vector3.left* 2f, Space.World);
		return 3;
	}











	private GameObject nearestShip(){
		GameObject target = GameObject.FindWithTag ("Player");
		GameObject[] allies = GameObject.FindGameObjectsWithTag ("Ally");
		if (target == null) {
			target = allies [0];
		}
		for (int i=0; i<allies.Length; i++) {
			if (((allies [i].transform.position - transform.position).magnitude) < ((target.transform.position - transform.position).magnitude) && !allies[i].Equals(gameObject) && !allies[i].Equals(target)) {
				target = allies [i];
			}
		}
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		for (int i=0; i<enemies.Length; i++) {
			if (((enemies [i].transform.position - transform.position).magnitude) < ((target.transform.position - transform.position).magnitude) && !enemies[i].Equals(gameObject) && !enemies[i].Equals(target)) {
				target = enemies [i];
			}
		}
		

		return target;
	}

	void OnTriggerEnter(Collider otherObj) {
		//Debug.Log("Hit Something");

		if ((enemy && ((otherObj.gameObject.tag == "PlayerBullet")||(otherObj.gameObject.tag == "AllyBullet")))||(!enemy && otherObj.gameObject.tag == "EnemyBullet")) {

			lastHit = Time.time;
			health-=damage;
			if(GameObject.Find("Overlay")!=null){
				indicator.transform.localScale += new Vector3(0.5f, 0.5f, 0f);
			}
		

			Destroy (otherObj.gameObject);


		if(health<=0){

				GameObject ex;
				float r = Random.Range(0.0f, 3.0f);
				if(r<=1.0f){
					ex = (GameObject)Instantiate(explosion1, transform.position, transform.rotation);
				}else if(r>=2.0f){
					ex = (GameObject)Instantiate(explosion2, transform.position, transform.rotation);
				}else{
					ex = (GameObject)Instantiate(explosion3, transform.position, transform.rotation);
				}

				Destroy(ex, 2.0f);
				Destroy(indicator);
				Destroy(gameObject);
				if(enemy){
					if(otherObj.gameObject.tag == "PlayerBullet"){
						GameObject.Find("KillLabel").GetComponent<Text>().text=float.Parse(GameObject.Find("KillLabel").GetComponent<Text>().text)+1f+"";
						GameObject.Find("KillLabel2").GetComponent<Text>().text=float.Parse(GameObject.Find("KillLabel").GetComponent<Text>().text)+1f+"";
						Debug.Log("Enemy Destroyed by Player");

					}else{
						Debug.Log("Enemy Destroyed by Ally");
					}
				}else{
					Debug.Log("Ally Destroyed");
				}
			}
		
		
		}
	}

	float targetScore(GameObject t){
		float score = 0f;
		score += 1500f / ((t.transform.position - transform.position).magnitude);
		score += (5-Mathf.Atan2(t.transform.position.y - transform.position.y, t.transform.position.x - transform.position.x) * 5 / Mathf.PI);
		if(t.Equals(GameObject.Find("Player"))){
			score*=3f;
		}

		return score;
	}


	void chooseTarget(){
		GameObject oldtarget = target;
		target = null;
		if (enemy) {
			target = GameObject.FindWithTag ("Player");
			GameObject[] allies = GameObject.FindGameObjectsWithTag ("Ally");
			if(target==null){
				target=allies[0];
			}
			float tarscore = targetScore(target);
			for (int i=0; i<allies.Length; i++) {
				if (targetScore(allies[i])>tarscore && !allies[i].Equals(oldtarget) && !allies[i].Equals(gameObject) && allies[i].GetComponent<AIController>().target!=gameObject) {
					target = allies [i];
					tarscore = targetScore(target);
				}
			}
			if(target.Equals(GameObject.FindGameObjectWithTag("Player")) && GameObject.Find("Overlay")!=null){
				Image i =indicator.GetComponent<Image>();
				indicator.transform.localScale = new Vector3(1.7f, 1.7f, 1f);
				indscale=indicator.transform.localScale;
				i.sprite = enemyIndicator2;
			}else{
				Image i =indicator.GetComponent<Image>();
				i.sprite = enemyIndicator;
				indicator.transform.localScale = new Vector3(1f, 1f, 1f);
				indscale=indicator.transform.localScale;
			}
			
			
		} else {
			GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
			target=enemies[0];
			float tarscore = targetScore(target);
			for (int i=0; i<enemies.Length; i++) {
				if (targetScore(enemies[i])>tarscore && !enemies[i].Equals(oldtarget) && !enemies[i].Equals(gameObject) && enemies[i].GetComponent<AIController>().target!=gameObject) {
					target = enemies [i];
					tarscore = targetScore(target);
				}
			}
			
		}
	}



}
