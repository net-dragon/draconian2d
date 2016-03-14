using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour 
{
	private bool chargingDash;
	public float chargeStart;
	public float chargeCounter;


	// Use this for initialization
	void Start () 
	{
		chargingDash = false;
		chargeCounter = 0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
	 
	}
	private void FixedUpdate()
	{
		if (Input.GetButton ("Fire1")) 
		{
		
			chargeCounter++;
		
			if (chargeCounter >= chargeStart)
			
				chargingDash = true;
		}


		if (Input.GetButtonUp ("Fire1")) 
		{				
			if (chargeCounter >= 100)
						
				StartCoroutine(DashRoutine());
			   
		}

		

		chargeCounter = 0f;
	}

	IEnumerator DashRoutine()
	{
		
	}
}
