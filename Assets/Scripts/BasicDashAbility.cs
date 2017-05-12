using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;
using System;
using UnityEngine.UI;

public class BasicDashAbility : MonoBehaviour
{
    //private bool chargingDash;
    //private bool dashing;
    [HideInInspector]
    public float dashCost = 50f;
    [HideInInspector]
    public float chargeStart = 100f;
    [SerializeField]
    private float chargeCounter;
    [SerializeField]
    private Vector2 mousePos;
    private Rigidbody2D m_Rigidbody2D;
    private Vector2 dashVector;
    [HideInInspector]
    public float initialDashForce = 50f;
    private bool dashCancel;
    [SerializeField]
    public float dashIncrement = 1f;
    [SerializeField]
    private float dashForce;
    [HideInInspector]
    public float maxDashForce = 100f;
    //public float vectorSaveTime = .4f;
    public float dashDuration;
    [HideInInspector]
    public float dashStop = 3f;
    // public float upTimer;
    private PlatformerCharacter2D character;
    //[HideInInspector]
    //public float stam;
    [SerializeField]
    private Slider dashBar;
    public bool abilityStart = false;
    [SerializeField]
    private DashAbility dash;

    public float currentStamina
    {
        get { return currentStamina; }
        set { currentStamina = value; }
    }


    public bool dashing;


    // Use this for initialization
    void Start()
    {
        dashBar.gameObject.SetActive(false);
        dashCancel = false;
        abilityStart = false;
        dashing = false;
        character = GetComponent<PlatformerCharacter2D>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        dashDuration = dashStop;
        dashBar.maxValue = maxDashForce;
        dashBar.minValue = initialDashForce;

    }

    public void Initialize ()
    {
        /*all taken from Start*/
        //chargingDash = false;
        chargeCounter = 0f;
        //upTimer = 0f;
        dashForce = initialDashForce;
        dashBar.maxValue = maxDashForce;
        dashBar.minValue = initialDashForce;
    }
    // Update is called once per frame
    void Update()
    {

    }//empty

    /*public void StartAbility()
    {
        abilityStart = true;
    }*/
    public void StartAbility() //clean
    {
        dashBar.minValue = initialDashForce;
        dashBar.maxValue = maxDashForce;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Space))
            dashCancel = true;
        else
            dashCancel = false;

        //stam = character.currentStamina;

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.DrawLine(transform.position, mousePos);

        // Debug.Log(upTimer);
        if (character.currentStamina > 0) //clean
        {
            if (Input.GetButton(dash.abilityButton))
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
    }

    public void StopAbility() //clean
    {

        if (Input.GetButtonUp(dash.abilityButton)) //clean
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
            dashBar.gameObject.SetActive(false);
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