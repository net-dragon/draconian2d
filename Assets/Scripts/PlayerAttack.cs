using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;
using System;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    //private bool chargingDash;
    //private bool dashing;
    public float dashCost = 50f;
    public float chargeStart = 100f;
    public float chargeCounter;
    public Vector2 mousePos;
    private Rigidbody2D m_Rigidbody2D;
    private Vector2 dashVector;
    public float initialDashForce = 50f;
    private bool dashCancel;
    public float dashIncrement = 1f;
    public float dashForce;
    public float maxDashForce = 100f;
    //public float vectorSaveTime = .4f;
    public float dashDuration;
    public float dashStop = 1f;
    // public float upTimer;
    private PlatformerCharacter2D character;
    public float stam;
    public Slider dashBar;

    public float currentStamina
    {
        get { return currentStamina; }
        set { currentStamina = value; }
    }


    public bool dashing
    {
        get { return dashDuration < dashStop; }
    }


    // Use this for initialization
    void Start()
    {
        dashBar.gameObject.SetActive(false);
        dashCancel = false;
        character = GetComponent<PlatformerCharacter2D>();

        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        //chargingDash = false;
        //dashing = false;
        chargeCounter = 0f;
        dashDuration = dashStop;
        //upTimer = 0f;
        dashForce = initialDashForce;
    }

    // Update is called once per frame
    void Update()
    {

    }//empty
    private void FixedUpdate() //clean
    {
        dashBar.maxValue = maxDashForce;
        dashBar.minValue = initialDashForce;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Space))
            dashCancel = true;
        else
            dashCancel = false;

        dashDuration += Time.deltaTime;
        stam = character.currentStamina;

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.DrawLine(transform.position, mousePos);

        // Debug.Log(upTimer);
        if (character.currentStamina > 0) //clean
        {
            if (Input.GetButton("Fire1"))
            {

                chargeCounter++;
                if (chargeCounter >= chargeStart)
                {
                    dashForce += (dashIncrement + Time.deltaTime);
                    dashBar.gameObject.SetActive(true);
                    dashBar.value = dashForce;
                }

                if (dashForce > maxDashForce)

                    dashForce = maxDashForce;
            }
            else

                dashBar.gameObject.SetActive(false);

            //if (chargeCounter >= chargeStart)
            //dashing = true;
            //StartCoroutine(DashChargeRoutine());

        }
        else if (character.currentStamina <= 0)
        {
            dashForce = initialDashForce;
            dashBar.gameObject.SetActive(false);
            chargeCounter = 0;
        }
            


        if (Input.GetButtonUp("Fire1")) //clean
        {
            if (chargeCounter >= chargeStart)
            {
                dashDuration = 0f;
                // dashDuration += (dashDuration * Time.deltaTime);
                dashVector = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);
                m_Rigidbody2D.velocity = dashVector.normalized * dashForce;        //m_Rigidbody2D.AddForce(dashVector.normalized * dashForce, ForceMode2D.Impulse);
                dashForce = initialDashForce;
                character.currentStamina = (character.currentStamina - dashCost);
            }
            //dashing = true;
            //StartCoroutine( DashRoutine());
            //dashDuration = 0f;
            //upTimer += (upTimer * Time.deltaTime);


            chargeCounter = 0f;
        }



        if (dashDuration < dashStop) //clean
        {
            character.AirControl = false;
        }
        else
        {
            character.AirControl = true;
        }

    }






    /*IEnumerator DashRoutine()
	{
        if (dashDuration < dashStop)
            character.AirControl = false;
        else
            character.AirControl = true;

        yield return null;


    }*/
}