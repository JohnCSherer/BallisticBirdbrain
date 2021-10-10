using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Hitable
{

    public GameObject joint;
    public GameObject chassis;
    public GameObject bullet;
    public GameObject scraps;
    public GameObject player;
   
    public float ROF;
    private float shotTimer = 0.0f;
    public float detectionRadius;
    public float turningSpeed;

    private MaterialPropertyBlock block;
    private Renderer[] blockRenderers;
    private Color color;

    private ReviveTurret reviveScript;

    public float reviveTime = 10;
    public float playerPrivacy = 40;

    private MeshRenderer[] meshRenderers;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player");
        reviveScript = GetComponent<ReviveTurret>();
        reviveScript.turretScript = this;
        reviveScript.playerPos = player.transform;
        reviveScript.playerPrivacy = playerPrivacy;

        health = maxHealth;
        block = new MaterialPropertyBlock();
        block.SetColor("_Color", new Color(1 - health / maxHealth, 0, health / maxHealth));
        
        blockRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer b in blockRenderers){
            b.SetPropertyBlock(block);
        }


        meshRenderers = new MeshRenderer[3];
        meshRenderers[0] = chassis.GetComponent<MeshRenderer>();
        meshRenderers[1] = joint.GetComponent<MeshRenderer>();
        meshRenderers[2] = joint.transform.GetChild(0).GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < detectionRadius && player.transform.position.y - transform.position.y > -3)
        {

            joint.transform.LookAt(player.transform.position + Vector3.up * 0.55f);

            chassis.transform.LookAt(player.transform.position);
            chassis.transform.eulerAngles = new Vector3(-90, chassis.transform.eulerAngles.y, 0);
            shotTimer += Time.deltaTime;
            if(shotTimer > ROF)
            {
                shotTimer = 0;
                GameObject bul = Instantiate(bullet);
                bul.transform.position = joint.transform.position;
                bul.transform.rotation = joint.transform.rotation;
                bul.transform.eulerAngles += new Vector3(-90, 0, 0);
                bul.GetComponent<BulletScrpt>().origin = gameObject;
            }
        }
        else
        {
            shotTimer = 0;
        }
    }

    public void Revive()
    {
        foreach (MeshRenderer m in meshRenderers)
        {
            m.enabled = true;
        }
        health = maxHealth;

        block.SetColor("_Color", new Color(1 - health / maxHealth, 0, health / maxHealth));

        blockRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer b in blockRenderers)
        {
            b.SetPropertyBlock(block);
        }
    }

    public override void Damage(float dmg, GameObject damageOrigin)
    {
        if (enabled)
        {
            health -= dmg;
            if (health <= 0)
            {
                GameObject particles = Instantiate(scraps);
                particles.transform.position = transform.position;
                foreach (MeshRenderer m in meshRenderers)
                {
                    m.enabled = false;
                }
                reviveScript.enabled = true;
                reviveScript.SetTime(reviveTime);
                enabled = false;
                transform.parent = null;
                player.GetComponent<TransitionScript>().CheckObjectives();
            }
            color = new Color(1 - health / maxHealth, 0, health / maxHealth);
            block.SetColor("_Color", color);
            foreach (Renderer b in blockRenderers)
            {
                b.SetPropertyBlock(block);
            }
        }
    }

    void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag("Hitbox"))
        {
            collision.GetComponent<Hitbox>().TakeDamage(this);
        }
    }
}
