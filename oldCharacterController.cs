using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UnityStandardAssets._2D
{
    public class PlatformerCharacter2D : MonoBehaviour
    {
        [SerializeField]
        private float m_MoveSpeed = 10f;                    // The fastest the player can travel in the x axis.
        public float m_MaxSpeed = 100f;
        [SerializeField]
        private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
        [SerializeField]
        private float m_FlyForce = 2f;                   //Amount of force added when the player jumps. Modifies and changes
        [Range(0, 1)]
        [SerializeField]
        private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%

        [SerializeField]
        private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
        [SerializeField]
        private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

        private Animator m_Anim;            // Reference to the player's animator component.

        private Transform m_CeilingCheck;   // A position marking where to check for ceilings
        private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.

        public int maxStamina = 100;        // Maximum the stamina allowed, derived from getMaxStamina

        public float baseStaminaRechargeDelay;// StaminaRechargeDelay reverts to this float when done
        const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        public float currentStamina;        // Current stamina derived from getCurrentStamina();
        public float flapDelay = .3f;      //Delay between flaps
        public float flyCost = 20f;        // How much stamina a flap costs
        public float glideDelay = .3f;     // Delay between glide and other flying actions
        public float glideDrag = -2;       // vertical velocity of character when glide is used
        const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        private float nextFlap;             // time that has elapsed between flaps
        private float nextGlide;            // time that has elapsed between glides
                                            //public float normalDrag =0.3f;      // Drag applied to player under normal circumstances
        private bool m_Grounded;            // Whether or not the player is grounded.
        public float staminaRechargeDelay = 3.0f; //Delay after stamina depletes until stamina can recharge
        public float staminaRechargeRate = 50.0f; //Rate stamina recharges

        private Rigidbody2D m_Rigidbody2D;
        public PlayerStats stats;
        public SpriteRenderer spriteRenderer;

        public Slider staminaBar;
        public Image fill;
        public Canvas dashCanvas;

        //public bool beginStaminaRecharge;
        private bool m_FacingRight = true;  // For determining which way the player is currently facing.
        private bool flying;
        //private bool gliding;
        //public bool staminaRecharge;

        public float flyTime = .7f;
        public Vector2 flyVector;
        public Transform charPos;
        public float maxFallSpeed = -100f;
        public float maxFlySpeed = 7.0f;

        public bool AirControl
        {
            get { return m_AirControl; }
            set { m_AirControl = value; }
        }

        public float Stamina
        {
            get { return currentStamina; }
            set { currentStamina = value; }
        }

        public bool Grounded
        {
            get { return m_Grounded; }
            set { m_Grounded = value; }
        }


        private PlayerAttack attacker;



        private void Start()
        {
            stats = new PlayerStats();

            // Setting up references
            m_GroundCheck = transform.Find("GroundCheck");
            m_CeilingCheck = transform.Find("CeilingCheck");
            m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            stats.setMaxStamina(maxStamina);
            stats.setCurrentStamina(stats.getMaxStamina());
            stats.setCurrentStamina(maxStamina);
            stats.setStaminaRechargeDelay(staminaRechargeDelay);
            baseStaminaRechargeDelay = staminaRechargeDelay;
            //staminaRecharge = false;
            //beginStaminaRecharge = false;
            stats.setStaminaRechargeRate(staminaRechargeRate);
            attacker = GetComponent<PlayerAttack>();
        }

        /* private void Start()
         {
             attacker = GetComponent<PlayerAttack>();
         }*/
        void Update()
        {

        }



        private void FixedUpdate()
        {

            //Debug.Log (m_Rigidbody2D.velocity.y);
            if (m_Rigidbody2D.velocity.y < maxFallSpeed) //clean
            {

                m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, maxFallSpeed);
            }

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

            m_Grounded = false;

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

                // float move = Input.GetAxis("Horizontal");
                // float moveVertical = Input.GetAxis("Vertical");

                //Vector2 movement = new Vector2(move, 0.0f);


            }
            //stamina bar manager
            if (staminaBar.value >= 50) //clean
            {
                fill.color = Color.green;
            }

            else if (staminaBar.value < 50)
            {
                fill.color = Color.yellow;
            }

            staminaBar.value = stats.getCurrentStamina();




            //animator
            m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y); //clean

            currentStamina = stats.getCurrentStamina(); //clean

            if (stats.getCurrentStamina() < 0) //clean

                stats.setCurrentStamina(0); //clean

            charPos.position = m_Rigidbody2D.position; //clean
            flyVector = new Vector2(charPos.position.x, (charPos.position.y + m_FlyForce)); //clean


            if (Input.GetButtonDown("Jump") /*&& !jumping*/ && stats.getCurrentStamina() > 0 && Time.time > nextFlap && Time.time > nextGlide && !m_Grounded) //Clean
            {
                //jumping = true;
                StartCoroutine(FlyRoutine());
                stats.setCurrentStamina(stats.getCurrentStamina() - flyCost);
            }


        }

        IEnumerator FlyRoutine() //Clean
        {
            //m_Rigidbody2D.velocity = Vector2.zero;
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

            //jumping = false;
        }


        //recharges stamina when activated
        public void StaminaRecharge()
        {
            if (stats.getCurrentStamina() < stats.getMaxStamina())
            {
                stats.setCurrentStamina(currentStamina += (staminaRechargeRate * Time.deltaTime));
            }
            else if (stats.getCurrentStamina() >= stats.getMaxStamina())
            {
                stats.setCurrentStamina(maxStamina);
            }
            //changes slider color
            fill.color = Color.cyan; //Clean
        }


        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
            /*Vector3 theScale =transform.localScale;
            theScale.x *= -1;*/

            if (spriteRenderer.flipX == true)
                spriteRenderer.flipX = false;

            else
                spriteRenderer.flipX = true;

            //transform.localScale = theScale;

        }


        public void Move(float move, bool crouch, bool jump, bool glide) //clean
        {
            Vector2 movement = new Vector2(move, 0.0f);
            // If crouching, check to see if the character can stand up
            if (!crouch && m_Anim.GetBool("Crouch"))
            {
                // If the character has a ceiling preventing them from standing up, keep them crouching
                if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
                {
                    crouch = true;
                }
            }

            // Set whether or not the character is crouching in the animator
            m_Anim.SetBool("Crouch", crouch);

            //only control the player if grounded or airControl is turned on
            if (m_AirControl && !attacker.dashing) //clean
            {

                // Reduce the speed if crouching by the crouchSpeed multiplier
                move = (crouch ? move * m_CrouchSpeed : move);

                // The Speed animator parameter is set to the absolute value of the horizontal input.
                m_Anim.SetFloat("Speed", Mathf.Abs(move));


                // Move the character
                if (m_Rigidbody2D.velocity.x * movement.x < m_MaxSpeed) //clean/remake to add momentum?
                    if (!m_Grounded)
                        m_Rigidbody2D.AddForce(movement.normalized * m_MoveSpeed * Time.deltaTime, ForceMode2D.Impulse);
                    else
                        m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);


                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
                // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
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
            if (stats.getCurrentStamina() > 0 && Input.GetKey(KeyCode.LeftShift) && Time.time > nextFlap && m_Rigidbody2D.velocity.y < -2) //clean
            {
                m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, glideDrag);
                nextGlide = Time.time + glideDelay;
                /*} else {
                    m_Rigidbody2D.gravityScale = normalDrag;*/
            }

            if (Time.time < nextFlap && !m_Grounded) //clean
                flying = true;
            else
                flying = false;

            //resets stamina delay
            if ((stats.getCurrentStamina() >= stats.getMaxStamina() && staminaRechargeDelay <= 0.0f) || (flying && staminaRechargeDelay <= 0.0f)) //clean
            {
                staminaRechargeDelay = baseStaminaRechargeDelay;
            }
            // Stamina recharge
            if (stats.getCurrentStamina() < stats.getMaxStamina()) //clean
                staminaRechargeDelay -= Time.deltaTime;


            if (staminaRechargeDelay <= 0.0f && m_Grounded)
            {



                StaminaRecharge();

            }
        }
    }
}
