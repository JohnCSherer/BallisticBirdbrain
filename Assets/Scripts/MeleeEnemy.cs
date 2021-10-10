using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Hitable
{
    public GameObject scrapParticles;
    public GameObject player;
    public GameObject home;

    public Transform blade1;
    public Transform blade2;

    public float detectRadius = 200;
    public float damage;
    public float speed = 10;
    public float turnSpeed = 0.2f;

    private float stunTimer = 0;

    private Rigidbody body;
    private Collider capsuleCollider;

    private Vector3 vectorToTarget;

    private float timeLastShot;

    private MaterialPropertyBlock block;
    private Renderer blockRenderer;
    private Color color;

    public bool grounded;
    public float groundCheckDistance = 2.6f;

    private float lastActive;
    public float longestAllowableInactivity = 10;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player");
        lastActive = Time.time;
        timeLastShot = Time.time - 5.1f;
        health = maxHealth;
        block = new MaterialPropertyBlock();
        block.SetColor("_Color", new Color(1 - health / maxHealth, 0, health / maxHealth));

        blockRenderer = GetComponent<Renderer>();
        blockRenderer.SetPropertyBlock(block);

        body = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;
        grounded = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, groundCheckDistance, 1 << 8);

        if (grounded && stunTimer <= 0) {

            if (Vector3.Distance(player.transform.position, transform.position) < detectRadius || Time.time - timeLastShot < 5.0f)
            {
                vectorToTarget = player.transform.position - transform.position;
                FollowTarget();
                lastActive = Time.time;
            }
            else
            {
                body.velocity = new Vector3(0, body.velocity.y, 0);
                transform.eulerAngles += new Vector3(0, Time.deltaTime *turnSpeed, 0);
                lastActive = Time.time;
            }
        }
    }

    private void Update()
    {
        blade1.Rotate(0, 0, Time.deltaTime* 400);
        blade2.Rotate(0, 0, Time.deltaTime * -400);
        if (stunTimer > 0)
            stunTimer -= Time.deltaTime;

    }

    private void FollowTarget()
    {

        Vector2 target = new Vector2(vectorToTarget.x, vectorToTarget.z).normalized;
        float angle = (transform.right.x * -target.x - transform.right.z * target.y);

        //If the angle between itself and forward is <90, it's positive

        int directionSign = Math.Sign(transform.forward.x * target.x + transform.forward.z * target.y);

        float delta = Mathf.Acos(angle) * directionSign / Mathf.PI / 2 * 360;
        float clamped = Mathf.Clamp(delta, -turnSpeed * Time.deltaTime, turnSpeed * Time.deltaTime);
        if (!float.IsNaN(delta))
        {
            transform.eulerAngles += new Vector3(0, clamped, 0);
        }

        if (Mathf.Abs(delta) < 15 || float.IsNaN(delta))
        {
            body.velocity = new Vector3(transform.right.x * Time.deltaTime * -speed * 100, body.velocity.y, transform.right.z * Time.deltaTime * -speed * 100);
        }
        else
        {
            body.velocity = Vector3.zero;
        }
    }

    public override void Damage(float dmg, GameObject damageOrigin)
    {
        if(!damageOrigin.Equals(gameObject) ) {
            health -= dmg;
            if (damageOrigin.name.Equals("Player"))
            {
                timeLastShot = Time.time;
            }
            if (health <= 0)
            {
                GameObject scraps = Instantiate(scrapParticles);
                scraps.transform.position = transform.position;
                transform.parent = null;
                player.GetComponent<TransitionScript>().CheckObjectives();
                GameObject.Destroy(gameObject);
            }
            color = new Color(1 - health / maxHealth, 0, health / maxHealth);
            block.SetColor("_Color", color);
            blockRenderer.SetPropertyBlock(block);
        }
    }

    public override void Stun(float time)
    {
        stunTimer = time;
    }


    void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag("Hitbox"))
        {
            collision.GetComponent<Hitbox>().TakeDamage(this);
        }
    }
}
