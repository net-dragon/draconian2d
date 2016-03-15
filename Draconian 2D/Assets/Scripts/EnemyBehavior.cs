using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {

	public GameObject player;
	public GameObject bullet;

	public float coneAngle; //the angle from forward to the edge of the "cone" in degrees
	public float coneLength;

	public uint shotDelay; //in frames
	private uint shotCounter = 0;

	public int direction = -1; //-1 is left, 1 is right

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

	void FixedUpdate () {
		float rad = coneAngle * 2*Mathf.PI / 360.0f;

		Debug.DrawLine (transform.position, transform.position + Vector3.right * direction * coneLength);
		Debug.DrawLine (transform.position, transform.position + (Vector3.right * Mathf.Cos (rad) + Vector3.up * Mathf.Sin (rad)) * direction * coneLength);
		Debug.DrawLine (transform.position, transform.position + (Vector3.right * Mathf.Cos (rad) - Vector3.up * Mathf.Sin (rad)) * direction * coneLength);

		Vector2 lookVector = player.transform.position - transform.position;

		//Get the proper direction
		direction = lookVector.x == 0 ? direction : lookVector.x > 0 ? 1 : -1;
		//if (direction == 1)
			//GetComponent<SpriteRenderer> ().flipX = true;
		//else
			//GetComponent<SpriteRenderer> ().flipX = false;

		//Shoot a bullet if it's looking at the player
		if (lookVector.magnitude <= coneLength && //close enough
		    Vector2.Dot ((Vector2.right * direction), lookVector.normalized) >= Mathf.Cos (rad)) {//is looking at the player

			if (shotCounter == 0) {
				Instantiate(bullet, transform.position, Quaternion.FromToRotation(Vector2.right, lookVector));
				shotCounter = shotDelay;
			}
		}

		if(shotCounter > 0)
			--shotCounter;
	}
}
