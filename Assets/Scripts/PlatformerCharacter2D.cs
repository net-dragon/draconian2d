using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UnityStandardAssets._2D
{
    public class PlatformerCharacter2D : MonoBehaviour
    {

        /*[Range(0, 1)]
        [SerializeField]
        private float m_CrouchSpeed = .36f;// Amount of maxSpeed applied to crouching movement. 1 = 100%*/

        [SerializeField]
        private LayerMask m_WhatIsGround;// A mask determining what is ground to the character

        private Rigidbody2D m_Rigidbody2D;

        //public PlayerStats stats;

        public SpriteRenderer spriteRenderer;

        public Slider staminaBar;

        public Image fill;

        public Canvas dashCanvas;

        //public bool beginStaminaRecharge;

        private Animator m_Anim;// Reference to the player's animator component.

        private Transform m_CeilingCheck;// A position marking where to check for ceilings
        private Transform m_GroundCheck;// A position marking where to check if the player is grounded.

        public int maxStamina = 100;// Maximum the stamina allowed

        public float baseStaminaRechargeDelay;// StaminaRechargeDelay reverts to this float when done
        public float currentStamina;// Current amount of stamina
        public float flyCost = 20f;// How much stamina a flap costs
        public float flapDelay = .3f;//Delay between flaps before another flap is allowed
        public float flyTime = .7f;//How long the player can flap there wings for
        public float glideDelay = .3f;// Delay between glide and other flying actions
        public float glideDrag = -2;// vertical velocity of character when glide is used
        const float k_CeilingRadius = .01f;// Radius of the overlap circle to determine if the player can stand up
        const float k_GroundedRadius = .2f;// Radius of the overlap circle to determine if grounded
        [SerializeField]
        private float m_FlyForce = 2f;//Amount of force added when the player jumps.
        [SerializeField]
        private float m_JumpForce = 400f;// Amount of force added when the player jumps.
        public float maxFallSpeed = -100f;// The maximum downward velocity the player can maintain
        public float maxFlySpeed = 7.0f;// The maximum upward velocity the player can maintain
        [SerializeField]
        private float m_MoveSpeed = 10f;// The force applied to the player while in the air
        public float m_MaxGroundSpeed = 100f;// The fastest the player can travel in the x axis on foot.
        public float m_MaxSpeed = 100f;// The fastest the player can travel in the x axis in air or ground.
        private float nextFlap;// time that has elapsed since the latest flap
        private float nextGlide;// time that has elapsed between glides
        public float slideDecreaseRate = .5f; //The rate at which the players velocity will decrease while sliding
        [SerializeField]
        private float slideSpeed; // speed player slides at
        private float slideTimer; // Increases as player slides
        public float slideTimerMax; //how long player will slide before speed starts to increase
        public float staminaRechargeDelay = 3.0f; //Delay after stamina depletes until stamina can recharge
        public float staminaRechargeRate = 50.0f; //Rate that stamina at recharges

        [SerializeField]
        private bool m_AirControl = false;// Whether or not a player can steer while jumping;
        private bool m_FacingRight = true;  // For determining which way the player is currently facing.
        private bool flying; // baisically if the player is in the air
        private bool flapping; // If the player wings are pushing are downwards.
        private bool isSliding; //Checks to see if player is sliding
        [SerializeField]
        private bool m_CrouchCancel; //To see if player can slide
        private bool m_Grounded;// Whether or not the player is grounded.
        //private bool gliding;
        //public bool staminaRecharge;

        
        public Vector2 flyVector; //Vector where player will fly
        public Transform charPos; 
        

        public bool AirControl /*Received from the Playyer Attack script*/
        {
            get { return m_AirControl; }
            set { m_AirControl = value; }
        }

        /* public float Stamina
         {
             get { return currentStamina; }
             set { currentStamina = value; }
         }*/

        public bool Grounded
        {
            get { return m_Grounded; }
            set { m_Grounded = value; }
        }

        private PlayerAttack attacker;

        private void Start()
        {
            isSliding = false;
            m_CrouchCancel = false;
            flapping = false;
            //stats = new PlayerStats();
            // Setting up references
            m_GroundCheck = transform.Find("GroundCheck");
            m_CeilingCheck = transform.Find("CeilingCheck");
            m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            // stats.setMaxStamina(maxStamina);
            // stats.setCurrentStamina(stats.getMaxStamina());
            //stats.setCurrentStamina(maxStamina);
            //stats.setStaminaRechargeDelay(staminaRechargeDelay);
            // baseStaminaRechargeDelay = staminaRechargeDelay;
            //staminaRecharge = false;
            //beginStaminaRecharge = false;
            //stats.setStaminaRechargeRate(staminaRechargeRate);
            attacker = GetComponent<PlayerAttack>();
            currentStamina = maxStamina;
        }


        void Update()
        {

        }


        private void FixedUpdate()
        {

            Debug.Log(m_Rigidbody2D.velocity.x);

            // Capping the players movement speed while falling
            if (m_Rigidbody2D.velocity.y < maxFallSpeed) //clean
                m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, maxFallSpeed);

            //capping player's horizontal movement speed
            if (Math.Abs(m_Rigidbody2D.velocity.x) > m_MaxSpeed && attacker.dashing == false)
            {
                if (m_Rigidbody2D.velocity.x > 0)
                    m_Rigidbody2D.velocity = new Vector2(m_MaxSpeed, m_Rigidbody2D.velocity.y);

                else if (m_Rigidbody2D.velocity.x < 0)
                    m_Rigidbody2D.velocity = new Vector2(-m_MaxSpeed, m_Rigidbody2D.velocity.y);
            }

            /*
           if(m_Rigidbody2D.velocity.y > maxFlySpeed)
           {
                   m_Rigidbody2D.velocity = Vector2.ClampMagnitude(m_Rigidbody2D.velocity, maxFlySpeed);
           }*/

            m_Grounded = false;//Constantly sets the state of the player as not grounded unless otherwise stated

            //Determines if the player is grounded or not
            if (attacker.dashing == false) //clean
            {
                // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
                // This can be done using layers instead but Sample Assets will not overwrite your project settings.
                Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                        m_Grounded = true;
                }
                m_Anim.SetBool("Ground", m_Grounded);
            }

            //Determines the color of the stamina bar
            if (staminaBar.value >= 50) //clean
                fill.color = Color.green;

            else if (staminaBar.value < 50)
                fill.color = Color.yellow;

            //animator
            m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y); //clean

            //Transfers the current position to new field
            charPos.position = m_Rigidbody2D.position;

            //Determines the fly vector used later by the fly coroutine
            flyVector = new Vector2(charPos.position.x, (charPos.position.y + m_FlyForce));

            //Keeps the currentstamina from falling below 0
            if (currentStamina < 0)
                currentStamina = 0;

            //Sets the stamina bar value to currentstamina value
            staminaBar.value = currentStamina;

            //resets stamina delay
            if ((currentStamina >= maxStamina && staminaRechargeDelay <= 0.0f) || (flapping && staminaRechargeDelay <= 0.0f)) //clean
                staminaRechargeDelay = baseStaminaRechargeDelay;

            // Stamina recharge
            if (currentStamina < maxStamina) //clean
                staminaRechargeDelay -= Time.deltaTime;

            // Begines the StaminaRecharge method
            if (staminaRechargeDelay <= 0.0f && m_Grounded && currentStamina != maxStamina)
                StaminaRecharge();
        }


        //recharges stamina when activated
        public void StaminaRecharge()
        {
            if (currentStamina < maxStamina)
                currentStamina += (staminaRechargeRate * Time.deltaTime);

            else if (currentStamina >= maxStamina)
                 currentStamina = maxStamina;

            //changes slider color
            fill.color = Color.cyan;
        }


        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
            /*Vector3 theScale =transform.localScale;
            theScale.x *= -1;*/

            // Flips the sprite renderer of the character
            if (spriteRenderer.flipX == true)
                spriteRenderer.flipX = false;

            else
                spriteRenderer.flipX = true;

            //transform.localScale = theScale;
        }


        public void Move(float move, bool crouch, bool jump, bool glide) //clean
        {

            Vector2 movement = new Vector2(move, 0.0f);
            //determines if crouch canceling
            if (crouch && Mathf.Abs(m_Rigidbody2D.velocity.x) > m_MaxGroundSpeed)
                m_CrouchCancel = true;
            else
                m_CrouchCancel = false;

                //Determines when you are flying and subtracts the stamina required
                if (Input.GetButtonDown("Jump") && currentStamina > 0 && Time.time > nextFlap && Time.time > nextGlide && !m_Grounded)
            {
                flapping = true;
                StartCoroutine(FlyRoutine());
                currentStamina = currentStamina - flyCost;
            }

            else
                flapping = false;

            // If crouching, check to see if the character can stand up
            if (!crouch && m_Anim.GetBool("Crouch"))
            {
                // If the character has a ceiling preventing them from standing up, keep them crouching
                if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
                    crouch = true;
            }

            // Set whether or not the character is crouching in the animator
            m_Anim.SetBool("Crouch", crouch);

            //only control the player if grounded or airControl is turned on
            if (m_AirControl && !attacker.dashing)
            {

                // Reduce the speed if crouching by the crouchSpeed multiplier
               // move = (crouch ? move * m_CrouchSpeed : move);

                // The Speed animator parameter is set to the absolute value of the horizontal input.
                m_Anim.SetFloat("Speed", Mathf.Abs(move));

                // Move the character
                if (m_Rigidbody2D.velocity.x * movement.x < m_MaxSpeed) //Checks to see if below max speed
                {
                    if (!m_Grounded/*|| (m_CrouchCancel && m_Grounded)*/)// Checks to see if crouch canceling or grounded
                        m_Rigidbody2D.AddForce(movement.normalized * m_MoveSpeed * Time.deltaTime, ForceMode2D.Impulse);//applies force to move

                    else if (m_Grounded /*&& !m_CrouchCancel*/)//check to see if grounded
                        m_Rigidbody2D.velocity = new Vector2(move * m_MaxGroundSpeed, m_Rigidbody2D.velocity.y);
                }

                if ((m_CrouchCancel && m_Grounded))// Makes sure grounded and crouch canceling the entire time
                {
                    if (crouch && Grounded && !isSliding) // begins slide
                    {
                        slideTimer = 0.0f;
                        isSliding = true;
                        slideSpeed = Mathf.Abs(m_Rigidbody2D.velocity.x);
                    }

                }
                    if (isSliding) //Decreases speed after a while while sliding, after a certain time sliding stops
                    {
                        m_Rigidbody2D.velocity = new Vector2(move *slideSpeed, m_Rigidbody2D.velocity.y);
                        slideTimer += Time.deltaTime;

                        if (slideTimer > slideTimerMax)
                            slideSpeed -= (slideDecreaseRate * Time.deltaTime);

                        if ((slideSpeed <= (m_MaxGroundSpeed -5)) || !crouch)// || (slideSpeed >= m_MaxGroundSpeed && move < 0))
                            isSliding = false;
                    }

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !m_FacingRight)
                    Flip();// ... flip the player.

                // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_FacingRight)
                    Flip();// ... flip the player.

            }
            // If the player should jump...
            if (m_Grounded && jump && m_Anim.GetBool("Ground")) //clean
            {
                // Add a vertical force to the player.
                //m_Grounded = false;
                m_Anim.SetBool("Ground", false);
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            }

            // if player should fly
            /*if (!m_Grounded && stats.getCurrentStamina () > 0 && jump && Time.time > nextFlap && Time.time > nextGlide) {
				//nextGlide = Time.time + glideDelay;
				nextFlap = Time.time + flapDelay;
				nextGlide = Time.time + glideDelay;
				m_Rigidbody2D.AddForce (new Vector2 (0f, m_FlyForce));
				stats.setCurrentStamina (stats.getCurrentStamina () - 20);
			}*/

            // if player should glide
            if (currentStamina > 0 && Input.GetKey(KeyCode.LeftShift) && Time.time > nextFlap && m_Rigidbody2D.velocity.y < -2) //clean
            {
                m_Rigidbody2D.velocity = new Vector2(move * m_MaxGroundSpeed, glideDrag);
                nextGlide = Time.time + glideDelay;
                /*} else {
                    m_Rigidbody2D.gravityScale = normalDrag;*/
            }

            if (Time.time < nextFlap && !m_Grounded) //clean
                flying = true;

            else
                flying = false;
        }


        // Handles how the player flys when called
        IEnumerator FlyRoutine()
        {
            float timer = 0;

            while (Input.GetButton("Jump") && timer < flyTime && !m_Grounded)
            {
                //Calculate how far through the jump we are as a percentage
                //apply the full jump force on the first frame, then apply less force
                //each consecutive frame

                //float proportionCompleted = timer / flyTime;
                //Vector2 thisFrameFlyVector = Vector2.Lerp( flyVector,m_Rigidbody2D.position,  proportionCompleted);
                //m_Rigidbody2D.AddForce(thisFrameFlyVector);
                m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_FlyForce);
                timer += Time.deltaTime;
                nextFlap = Time.time + flapDelay;
                nextGlide = Time.time + glideDelay;

                yield return null;
            }

        }
    }
}
