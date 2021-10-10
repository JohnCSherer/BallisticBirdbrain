using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementScript : Hitable
{
    private Rigidbody body;
    private CapsuleCollider capsuleCollider;
    public float speedMult = 10.0f;
    public float waterSpeed = 10.0f;
    public float airControl = 5;
    public float acceleration = 1.0f;
    public float groundCheckDistance = 4.0f;
    public float ladderSpeed = 5.0f;
    private bool grounded;
    private bool onLadder;
    private Vector3 ladderNormal;

    public float waterLevel = -100;

    public float jumpStrength;
    private Vector3 jumpVector;
    private float jumpTimer;
    private float jumpCooldown = 0.2f;

    public GameObject PoundHitbox;
    public GameObject PoundParticles;
    public GameObject ShockwaveHitbox;
    public GameObject ChargeBox;

    public CameraScript cam;

    public GameObject face;

    public Image Portrait;
    public Sprite neutral;
    public Sprite hurt;

    public Image HUD;
    public Sprite hud;
    public Sprite cracked;
    public Sprite damaged;
    public Sprite red;

    private Vector3 lastPosition;

    private float speedBoostTimer = 0;
    private float poundTimer = 0;
    private float poundTimerInitial = 0;

    public float invulnerabilityTime = 1.0f;
    private float invulnTimer = 0.0f;


    private Vector3 targetVelocity;

    public Text text;

    public Image healthbar;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        jumpVector = new Vector3(0, jumpStrength, 0);
        health = maxHealth;
    }

    private void Awake()
    {
        cam = GetComponent<CameraScript>();
        Portrait = GameObject.Find("Canvas").transform.Find("Portrait").GetComponent<Image>();
        healthbar = GameObject.Find("Canvas").transform.Find("HealthBar").GetComponent<Image>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (jumpTimer <= 0)
        {
            grounded = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out RaycastHit hit, groundCheckDistance, 1 << 8);
            if (grounded && !onLadder && transform.position.y > waterLevel)
            {
                transform.position = new Vector3(transform.position.x, hit.point.y + capsuleCollider.height / 2 + 0.5f, transform.position.z);
                body.velocity = new Vector3(0, 0, 0);
            }
        }

        targetVelocity = (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")) * speedMult;

        if(targetVelocity.magnitude > speedMult)
        {
            targetVelocity.Normalize();
            targetVelocity *= speedMult;
        }


        if (speedBoostTimer > 0)
        {
            if(body.velocity.magnitude <= 2 * Time.deltaTime)
            {

            }
            body.velocity = transform.forward * 4000 * Time.deltaTime;
        }
        else if(poundTimer > 0)
        {
            if (grounded)
            {
                ShockwaveHitbox.GetComponent<SphereCollider>().radius = (poundTimerInitial - poundTimer) * 2;
                PoundParticles.GetComponent<ParticleSystem>().emission.SetBurst(0, new ParticleSystem.Burst(0, (short)((poundTimerInitial - poundTimer) * 100)));
                poundTimer = 0;
                invulnTimer = 0.4f;
                ShockwaveHitbox.SetActive(true);
                PoundParticles.SetActive(false);
                PoundParticles.SetActive(true);
                PoundParticles.GetComponent<Disable>().SetTimer();
                PoundHitbox.SetActive(false);
            }
            else
            {
                body.velocity = Vector3.down * Time.deltaTime * 3500 * (poundTimerInitial - poundTimer - 0.2f) ;
            }
        }
        else
        {
            if (grounded || onLadder || transform.position.y <= waterLevel) { /* USED TO SET YVEL TO 0 */
                body.velocity = new Vector3(targetVelocity.x * Time.deltaTime * 100, body.velocity.y, targetVelocity.z * Time.deltaTime * 100);
                if (onLadder)
                {
                    float angle = Vector3.Angle(ladderNormal, body.velocity.normalized);
                    if (angle >= 90)
                    {
                        body.velocity = new Vector3(body.velocity.x, ladderSpeed * Time.deltaTime * 100, body.velocity.z);
                    }
                    else
                    {
                        body.velocity = new Vector3(body.velocity.x, -ladderSpeed * Time.deltaTime * 100, body.velocity.z);
                    }
                }
            }
            else
            {
                targetVelocity *= airControl;
                body.velocity += new Vector3(targetVelocity.x * Time.deltaTime, 0, targetVelocity.z * Time.deltaTime);

                Vector2 horizSpeed = new Vector2(body.velocity.x, body.velocity.z);

                if (horizSpeed.magnitude > 100 * speedMult * Time.deltaTime)
                {
                    horizSpeed *= 0.8f;
                    body.velocity = new Vector3(horizSpeed.x, body.velocity.y, horizSpeed.y);
                }
            }
        }

        if(Math.Abs(Input.GetAxis("Horizontal")) < 0.5 && Math.Abs(Input.GetAxis("Vertical")) < 0.5)
        {

        }
    }

    private void Update()
    {
  
        if (Input.GetKeyDown(KeyCode.Space))
        {

            if (transform.position.y <= waterLevel)
            {
                if (body.velocity.y <= waterSpeed * 100 * Time.deltaTime)
                {
                    body.velocity = new Vector3(0, waterSpeed * 100 * Time.deltaTime,0 );
                    Debug.Log("up");
                }
            } else if (grounded && jumpTimer <= 0) {
                body.velocity += jumpVector;
                jumpTimer = jumpCooldown;
                grounded = false;
            }
        }
        if (jumpTimer > 0)
            jumpTimer -= Time.deltaTime;
        if (invulnTimer > 0)
        {
            invulnTimer -= Time.deltaTime;
            if (invulnTimer <= 0)
                Portrait.sprite = neutral;
        }
        if (poundTimer > 0)
            poundTimer -= Time.deltaTime;
        if (speedBoostTimer > 0)
        {
            speedBoostTimer -= Time.deltaTime;
            if(speedBoostTimer <= 0)
            {
                CameraScript.CapSensitivity(256);
            }
        }

        
    }

    public void SpeedBoost()
    {
        speedBoostTimer = 0.8f;
        invulnTimer = 0.8f;
        CameraScript.CapSensitivity(0.3f);
        ChargeBox.SetActive(true);
        ChargeBox.GetComponent<ExplosionBox>().Reset();
    }

    public void Pound()
    {
        poundTimer = 4.0f;
        poundTimerInitial = 4.0f;
        invulnTimer = 2.0f;
        PoundHitbox.SetActive(true);
        jumpTimer = 0;
        grounded = false;
        transform.position += Vector3.up;
    }

    public override void Damage(float dmg, GameObject damageOrigin)
    {
        if (invulnTimer <= 0)
        {
            health = health - dmg;
            Portrait.sprite = hurt;

            if (health <= 0)
            {
                healthbar.rectTransform.localScale = new Vector3(0.0f, 1, 1);
                CameraScript.CapSensitivity(256);
                cam.Die();
                this.enabled = false;
            }
            healthbar.rectTransform.localScale = new Vector3(health / maxHealth, 1, 1);
            invulnTimer = invulnerabilityTime;

            if(health / maxHealth < 0.5)
            {
                if(health / maxHealth < 0.25)
                {
                }
            }
        }
    }

    void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag("Hitbox"))
        {
            collision.GetComponent<Hitbox>().TakeDamage(this);
        } 
        else if (collision.CompareTag("Ladder"))
        {
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            onLadder = true;
            ladderNormal = other.transform.forward;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            onLadder = false;
            body.velocity = new Vector3(body.velocity.x, ladderSpeed * Time.deltaTime * 10, body.velocity.z);
        }
    }
}
