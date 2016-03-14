using UnityEngine;
using System.Collections;

public class EnemyBullet : MonoBehaviour {

	public uint lifetime; //in frames
	public float speed;

	private uint lifeCounter;

	// Use this for initialization
	void Start () {
		lifeCounter = lifetime;

		Rigidbody rb = GetComponent<Rigidbody> ();
		rb.velocity = transform.right * speed;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate () {
		
		--lifeCounter;
		Debug.Log ("Life Counter: " + lifeCounter.ToString());
		if (lifeCounter == 0)
			Destroy (gameObject);
	}
}
