using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {

	private GameObject player;
	public GameObject bullet;

	public float normalConeAngle;
	public float normalConeLength;

	public float suspiciousConeAngle;
	public float suspiciousConeLength;

	public float expandedConeAngle;
	public float expandedConeLength;

	private float coneAngle; //the angle from forward to the edge of the "cone" in degrees
	private float coneLength;

    private uint timeOutOfRange = 0;
    private uint extendedRangeTime = 300;

    public uint shotStartDelay = 7;
    private uint shotStartTimer = 0;
	public uint shotDelay; //in frames
	private uint shotCounter = 0;

	public uint dashHitTime = 120;
	private uint dashHitTimer = 0;

	public int direction = -1; //-1 is left, 1 is right

	private PlayerAttack playerAttack;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");

		coneAngle = normalConeAngle;
		coneLength = normalConeLength;

		playerAttack = player.GetComponent<PlayerAttack> ();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

	void FixedUpdate () {
		float rad = coneAngle * 2*Mathf.PI / 360.0f;
		float sRad = suspiciousConeAngle * 2*Mathf.PI / 360.0f;

		Debug.DrawLine (transform.position, transform.position + Vector3.right * direction * coneLength);
		Debug.DrawLine (transform.position, transform.position + (Vector3.right * Mathf.Cos (rad) + Vector3.up * Mathf.Sin (rad)) * direction * coneLength);
		Debug.DrawLine (transform.position, transform.position + (Vector3.right * Mathf.Cos (rad) - Vector3.up * Mathf.Sin (rad)) * direction * coneLength);

		Debug.DrawLine (transform.position, transform.position + Vector3.right * direction * suspiciousConeLength, Color.green);
		Debug.DrawLine (transform.position, transform.position + (Vector3.right * Mathf.Cos (sRad) + Vector3.up * Mathf.Sin (sRad)) * direction * suspiciousConeLength, Color.green);
		Debug.DrawLine (transform.position, transform.position + (Vector3.right * Mathf.Cos (sRad) - Vector3.up * Mathf.Sin (sRad)) * direction * suspiciousConeLength, Color.green);

		if (dashHitTimer == 0) {
			Vector2 lookVector = player.transform.position - transform.position;

			//Get the proper direction
			if (coneAngle == expandedConeAngle)
				direction = lookVector.x == 0 ? direction : lookVector.x > 0 ? 1 : -1;
			if (direction == 1)
				GetComponent<SpriteRenderer> ().flipX = true;
			else
				GetComponent<SpriteRenderer> ().flipX = false;

			if (shotStartTimer != 0) {
				++shotStartTimer;
				if (shotStartTimer == shotStartDelay) {
					//Shoot
					shotStartTimer = 0;
					Instantiate (bullet, transform.position, Quaternion.FromToRotation (Vector2.right, lookVector));
					shotCounter = shotDelay;
				}
			}

			//Shoot a bullet if it's looking at the player
			if (IsPlayerInsideCone (lookVector, rad, coneLength)) {//is looking at the player

				//Start the shot timer
				if (shotCounter == 0) {

					if (shotStartTimer == 0)
						++shotStartTimer;
					transform.FindChild ("Enemy Suspicion").gameObject.SetActive (false);
				}

				coneAngle = expandedConeAngle;
				coneLength = expandedConeLength;

			} else {

				++timeOutOfRange;
				if (timeOutOfRange >= extendedRangeTime) {
					coneAngle = normalConeAngle;
					coneLength = normalConeLength;
					timeOutOfRange = 0;
				}


				transform.FindChild ("Enemy Suspicion").gameObject.SetActive (IsPlayerInsideCone (lookVector, sRad, suspiciousConeLength));
			}

			if (shotCounter > 0)
				--shotCounter;
		} else {
			--dashHitTimer;
		}

	}

	bool IsPlayerInsideCone(Vector2 lookVector, float coneAngleRad, float coneLen)
	{
		return lookVector.magnitude <= coneLen && //close enough
			Vector2.Dot ((Vector2.right * direction), lookVector.normalized) >= Mathf.Cos (coneAngleRad);
	}

	void OnCollisionEnter2D(Collision2D clsn)
	{
		if (clsn.gameObject == player && playerAttack.dashing) {
			dashHitTimer = dashHitTime;
		}

	}
}
