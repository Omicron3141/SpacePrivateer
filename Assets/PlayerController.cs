using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	
	// speed is the rate at which the object will rotate
	public float speed;


	public float energyrecover;
	
	public float maxspeed;

	public GameObject bullet;

	public float reloadtime;
	

	private float reloadtimer=0.0f;
	
	
	public float velocity=0.0f;
	
	
	
	private Vector3 rot;

	private float turnspeed;

	private MeshRenderer shields;

	public float shieldDecay;
	

	private float shieldtimer;

	public float maxhealth;

	private float health;
	
	public float damage;

	public GameObject explosion1;


	public AudioClip shootSound;
	
	private AudioSource source;

	public float maxrotspeed;


	public int manuver;

	private Quaternion orRot;

	private float sTime;

	private float energy;

	private bool wallman=false;

	
	void Start(){
		energy = 100f;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		Debug.Log ("Starting");
		GameObject.Find("GameOver").GetComponent<Canvas>().enabled=false;

		RenderSettings.ambientIntensity = 0.0f;
		shields = GameObject.Find ("Shield").GetComponent<MeshRenderer> ();
		shieldtimer = 0.0f;
		Color co = new Color(0.0f, 0.0f, 1.0f);
		co.a = 0.0f;
		shields.material.SetColor ("_Color", co);
		health = maxhealth;
		source = GetComponent<AudioSource>();
		manuver = 0;
		velocity = 2500f;
		shields.enabled = false;
		GameObject.Find("Overlay").GetComponent<Canvas>().enabled=false;

		
		
		
	}
	
	void FixedUpdate () 
	{
		if (Time.timeSinceLevelLoad < 5f) {
			Vector3 movement = new Vector3 (Random.Range(-10f, 10f), Random.Range(-10f, 10f), velocity);
			transform.Translate (movement * Time.deltaTime, Space.Self);
			manuver=0;
		} else {
			GameObject.Find("Overlay").GetComponent<Canvas>().enabled=true;
			if(velocity>maxspeed){
				velocity-=3000f*Time.deltaTime;
			}

			if(GameObject.Find("Warp")!=null){
				Destroy(GameObject.Find("Warp"));
			}


			GameObject[] explosions = GameObject.FindGameObjectsWithTag("Explosion");
			foreach (GameObject ex in explosions){
				if((ex.transform.position-transform.position).magnitude<1000f){
					health-=(1f-((ex.transform.position-transform.position).magnitude)/1000f) * Time.deltaTime;
					if(shieldtimer<-10f){

						shieldtimer=shieldDecay;
						Color c = shields.material.color;
						c.a=0.8f;
						c.r=1.0f-health/maxhealth;
						c.b=health/maxhealth;
						shields.enabled=true;
						shields.material.SetColor("_Color", c);	

					}
					GameObject.Find("ShieldLabel").GetComponent<Text>().text=Mathf.Round(health/maxhealth * 100f)+"%";
				}
			}

			if(health<=0.0f){
				EndGame();
			}


			if (manuver == 1) {
				manuver = HalfLoop (manuver, orRot, sTime);
			} else if (manuver == 2) {
				manuver = QuickReverse (manuver, orRot, sTime);
			} else if (manuver == 3) {
				manuver = RightTwirl (manuver, orRot, sTime);
			}else if (manuver == 4) {
				manuver = LeftTwirl (manuver, orRot, sTime);
			}else {
				
				if (Input.GetKey ("w") && energy>=20f) {
					manuver = 1;
					orRot = transform.rotation;
					sTime = Time.time;
				} else if (Input.GetKey ("s") && energy>=50f) {
					manuver = 2;
					orRot = transform.rotation;
					sTime = Time.time;
				} else if (Input.GetKey ("d") && energy>=20f) {
					manuver = 3;
					orRot = transform.rotation;
					sTime = Time.time;
				} else if (Input.GetKey ("a") && energy>=20f) {
					manuver = 4;
					orRot = transform.rotation;
					sTime = Time.time;
				}
				
				if (energy < 100f) {
					energy += energyrecover * Time.deltaTime;
				}
				
				
				turnspeed = speed * maxspeed / 50;
				
				//turnspeed = 48f;
				//rot = new Vector3(-Input.mousePosition.y+(Screen.height/2), Input.mousePosition.x-(Screen.height/2), ztrans);
				
				float mousex = Input.GetAxis ("Vertical");
				float mousey = Input.GetAxis ("Horizontal");
				
				if (mousex > maxrotspeed) {
					mousex = maxrotspeed;
				} else if (mousex < -maxrotspeed) {
					mousex = -maxrotspeed;
				}
				
				
				if (mousey > maxrotspeed) {
					mousey = maxrotspeed;
				} else if (mousey < -maxrotspeed) {
					mousey = -maxrotspeed;
				}
				
				
				rot = new Vector3 (mousex, mousey, 0f);
				
				rot *= turnspeed;
				
				
				transform.Rotate (rot * turnspeed * Time.deltaTime, Space.Self);
				

				
				
				
				Vector3 movement = new Vector3 (0.0f, 0.0f, velocity);
				
				transform.Translate (movement * Time.deltaTime, Space.Self);
				
				GameObject[] engines = GameObject.FindGameObjectsWithTag ("Engine Effects");
				
				for (int i=0; i<4; i++) {
					engines [i].GetComponent<ParticleSystem> ().startSpeed = (velocity);
				}
				
				if (Input.GetAxis ("Fire1") > 0.5f && reloadtimer < 0) {
					source.PlayOneShot (shootSound, 0.5f);
					Instantiate (bullet, transform.position, transform.rotation);
					reloadtimer = reloadtime;
					
				} else {
					reloadtimer -= Time.deltaTime;
				}
			}
			shieldtimer -= Time.deltaTime;
			if (shieldtimer > 0.03f) {
				Color c = shields.material.color;
				c.a = 0.8f * shieldtimer / shieldDecay;
				shields.material.SetColor ("_Color", c);
			} else if (shieldtimer <= 0.03f) {
				shields.enabled = false;
			}
			
			GameObject.Find ("EnergyLabel").GetComponent<Text> ().text = Mathf.Round (energy) + "%";

		}

		
		
		
		
	}

	int HalfLoop(float m, Quaternion orRot, float sTime){
		if (Time.time - sTime < 1f) {
			transform.Rotate(new Vector3(-180f, 0f, 0f)*Time.deltaTime);
		} else {
			wallman=false;
			return 0;
		}
		Vector3 movement = new Vector3 (0.0f, 0.0f, velocity*0.6f);
		
		transform.Translate (movement * Time.deltaTime, Space.Self);
		if (!wallman) {
			energy -= 20f * Time.deltaTime;
		}
		return 1;
	}

	int QuickReverse(float m, Quaternion orRot, float sTime){
		if (Time.time - sTime < 2f) {
			transform.Rotate (new Vector3 (0, 0f, 180f) * Time.deltaTime);
			Vector3 movement = new Vector3 (0.0f, 0.0f, velocity * 3f);
			transform.Translate (movement * Time.deltaTime, Space.Self);
		} else if (Time.time - sTime < 2.5f) {
			transform.Rotate (new Vector3 (-360, 0f, 0f) * Time.deltaTime);
			Vector3 movement = new Vector3 (0.0f, 0.0f, velocity * 0.6f);
			transform.Translate (movement * Time.deltaTime, Space.Self);
		}else {
			wallman=false;
			return 0;
		}
		energy -= 20f * Time.deltaTime;

		return 2;
	}

	int RightTwirl(float m, Quaternion orRot, float sTime){
		if (Time.time - sTime < 2f) {
			transform.Rotate(new Vector3(-164f, 0f, -82f)*Time.deltaTime);
		} else {
			return 0;
		}
		Vector3 movement = new Vector3 (0.0f, 0.0f, velocity*2.0f);
		transform.Translate (movement * Time.deltaTime, Space.Self);
		transform.Translate (orRot*Vector3.right* 10f, Space.World);
		energy -= 10f * Time.deltaTime;
		return 3;
	}

	int LeftTwirl(float m, Quaternion orRot, float sTime){
		if (Time.time - sTime < 2f) {
			transform.Rotate(new Vector3(-164f, 0f, 82f)*Time.deltaTime);
		} else {
			return 0;
		}
		Vector3 movement = new Vector3 (0.0f, 0.0f, velocity*2.0f);
		transform.Translate (movement * Time.deltaTime, Space.Self);
		transform.Translate (orRot*Vector3.left* 10f, Space.World);
		energy -= 10f * Time.deltaTime;
		return 4;
	}
	
	void OnTriggerEnter(Collider otherObj){
		if ((otherObj.gameObject.tag == "Boundaries")) {
			wallman=true;
			manuver=1;
			orRot=transform.rotation;
			sTime=Time.time;
		} else if ((otherObj.gameObject.tag == "EnemyBullet")) {
			Destroy(otherObj);
			shieldtimer=shieldDecay;
			Color c = shields.material.color;
			c.a=0.8f;
			health-=damage;
			c.r=1.0f-health/maxhealth;
			c.b=health/maxhealth;
			shields.enabled=true;
			shields.material.SetColor("_Color", c);	
			GameObject.Find("ShieldLabel").GetComponent<Text>().text=Mathf.Round(health/maxhealth * 100f)+"%";
			if(health<=0.0f){
				EndGame();
			}
		}
	}

	void EndGame(){
		GameObject ex = (GameObject)Instantiate(explosion1, transform.position, transform.rotation);
		Destroy(ex, 2.0f);
		GameObject.Find("GameOver").GetComponent<Canvas>().enabled=true;
		Destroy(GameObject.Find("Canvas"));
		Destroy(GameObject.Find("Holo"));
		Destroy(GameObject.Find("Overlay"));
		Destroy(GameObject.Find("Pause"));
		Destroy(GameObject.Find("UI Camera"));
		
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		
		if(!PlayerPrefs.HasKey("HighScore")){
			PlayerPrefs.SetInt("HighScore", 0);
		}
		
		int highscore = PlayerPrefs.GetInt("HighScore");
		int playerscore = int.Parse(GameObject.Find("KillLabel2").GetComponent<Text>().text);
		
		GameObject.Find("Highscore").GetComponent<Text>().text = highscore+"";
		
		if(playerscore>highscore){
			PlayerPrefs.SetInt("HighScore", playerscore);
			GameObject.Find("Newhighscore").GetComponent<Text>().enabled=true;
			GameObject.Find("Highscoredesc").GetComponent<Text>().text="Old High Score:";
		}
		
		
		
		
		
		
		Destroy(gameObject);
	}
}
