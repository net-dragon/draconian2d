using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour 
{
	//private bool chargingDash;
	//private bool dashing;
	public float chargeStart = 100f;
	public float chargeCounter;
	public Vector2 mousePos;
	private Rigidbody2D m_Rigidbody2D;
	private Vector2 dashVector;
	public float dashForce = 100f;	



	// Use this for initialization
	void Start () 
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		//chargingDash = false;
		//dashing = false;
		chargeCounter = 0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
	 
	}
	private void FixedUpdate()
	{ 
		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Debug.DrawLine (transform.position, mousePos);
		Debug.Log (chargeCounter);

		if (Input.GetButton ("Fire1")) 
		{
		
			chargeCounter++;

		
			//if (chargeCounter >= chargeStart)
				//dashing = true;
				//StartCoroutine(DashRoutine());
			 
		}


		if (Input.GetButtonUp ("Fire1")) 
		{				
			if (chargeCounter >= chargeStart)
			{
				dashVector = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y );
				m_Rigidbody2D.AddForce(dashVector *dashForce);
				//dashing = true;
			}
			chargeCounter = 0f;			
				
			   
		}
		

		
		
	}

	//IEnumerator DashRoutine()
	//{
		
	//}
}
