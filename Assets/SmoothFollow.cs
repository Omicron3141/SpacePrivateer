using UnityEngine;

	public class SmoothFollow : MonoBehaviour
	{

		// The target we are following
		[SerializeField]
		private Transform target;
		// The distance in the x-z plane to the target
		[SerializeField]
		private float distance = 10.0f;
		// the height we want the camera to be above the target
		[SerializeField]
		private float height = 5.0f;

		[SerializeField]
		private float rotationDamping;
		[SerializeField]
		private float heightDamping;

		private float trackcount;

		// Use this for initialization
		void Start() { }

		// Update is called once per frame
		void LateUpdate()
		{
			// Early out if we don't have a target
			if (!target)
				return;

			// Calculate the current rotation angles
			/*var wantedRotationAngley = target.eulerAngles.y;
			var wantedHeight = target.position.y + height;

			var currentRotationAngley = transform.eulerAngles.y;
			var currentHeight = transform.position.y;

			var wantedRotationAnglex = target.eulerAngles.x;

			var currentRotationAnglex = transform.eulerAngles.x;

			// Damp the rotation around the y-axis
			currentRotationAnglex = Mathf.LerpAngle(currentRotationAnglex, wantedRotationAnglex, rotationDamping * Time.deltaTime);
			currentRotationAngley = Mathf.LerpAngle(currentRotationAngley, wantedRotationAngley, rotationDamping * Time.deltaTime);

			// Damp the height
			currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

			// Convert the angle into a rotation
			var currentRotation = Quaternion.Euler(currentRotationAnglex, currentRotationAngley, 0);

			// Set the position of the camera on the x-z plane to:
			// distance meters behind the target
			transform.position = target.position;
			transform.position -= currentRotation * Vector3.forward * distance;

			// Set the height of the camera
			//transform.position = new Vector3(transform.position.x ,transform.position.y+currentHeight , transform.position.z);
			transform.Translate (new Vector3 (0.0f, currentHeight, 0.0f)-transform.position, Space.Self);

			// Always look at the target
			transform.LookAt(target);*/

			//Get relevant rotations



			Quaternion currentRotation=transform.rotation;


			Quaternion wantedRotation=target.rotation;
			float tempdamp = rotationDamping;
			if (GameObject.FindWithTag ("Player").GetComponent<PlayerController> ().manuver > 0) {
				tempdamp = 0f;
				trackcount=1f;
			}else if(trackcount>0f){
				tempdamp = 6f;
				trackcount-=Time.deltaTime;
			}
			//Lerp to new rotation
			Quaternion newRotation=Quaternion.Lerp(currentRotation, wantedRotation, tempdamp*Time.deltaTime);
			
			//set rotation
			transform.rotation=newRotation;
			
			//set position: back distance, up height
			transform.position=target.position-(transform.forward*distance)+(target.up*height);

		}
	}