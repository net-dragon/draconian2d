using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyBullet : MonoBehaviour {

	public uint lifetime; //in frames
	public float speed;

	private uint lifeCounter;

	// Use this for initialization
	void Start () {
		lifeCounter = lifetime;

		Rigidbody2D rb = GetComponent<Rigidbody2D> ();
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

	void OnCollisionEnter2D(Collision2D clsn) {
		if (clsn.collider.CompareTag ("Player")) {
			clsn.gameObject.SetActive (false);

			GameObject.Find ("Canvas/Message").GetComponent<Text> ().text = "Game Over!";
		} 
	}
}
