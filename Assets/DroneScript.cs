using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class DroneScript : Hitable
{
    public float speed = 10;
    public float turnRadius = 50;

    public float detectRadius = 50;

    public float preferedElevation = 15.8f;

    private float returnToRoam = 0;

    public GameObject scrapParticles;

    public Transform target;

    private MaterialPropertyBlock block;
    private Renderer blockRenderer;
    private Color color;

    //Left is forward

    // Start is called before the first frame update
    void Awake()
    {
        health = maxHealth;
        block = new MaterialPropertyBlock();
        block.SetColor("_Color", new Color(1 - health / maxHealth, 0, health / maxHealth));

        blockRenderer = GetComponent<Renderer>();
        blockRenderer.SetPropertyBlock(block);

        target = GameObject.Find("Player").transform;
        Roam();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (returnToRoam <= 0)
        {
            if (Vector3.Distance(transform.position, target.position) < detectRadius)
            {
                Dive();
            }
            else
            {
                Roam();
            }
        } else
        {
            returnToRoam -= Time.deltaTime;
            Return();
        }
    }

    private void Roam()
    {
        transform.localEulerAngles += new Vector3(0, Time.deltaTime * turnRadius * 10, 0);
        transform.Translate(Vector3.left * speed * Time.deltaTime * 20);

        //Return to standard elevation !!
    }

    private void Dive()
    {
        Vector3 vectorToTarget = target.position - transform.position;
        Vector2 target2d = new Vector2(vectorToTarget.x, vectorToTarget.z).normalized;
        float angle = (transform.forward.x * -target2d.x - transform.forward.z * target2d.y);

        if(angle < 0)
        {
            transform.localEulerAngles += new Vector3(0, Time.deltaTime * turnRadius * 10, 0);
        } else
        {
            transform.localEulerAngles -= new Vector3(0, Time.deltaTime * turnRadius * 10, 0);
        }

        float lateralDistance = Mathf.Sqrt(Mathf.Pow(target.position.x - transform.position.x, 2) + Mathf.Pow(target.position.z - transform.position.z, 2));

        float zAngle = Mathf.Atan((transform.position.y - target.position.y)/lateralDistance);

        Debug.Log(zAngle);

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, zAngle * Mathf.Rad2Deg);

        transform.Translate(Vector3.left * speed * Time.deltaTime * 20);

        if(Mathf.Abs(transform.position.y - target.position.y) < 0.5f)
        {
            returnToRoam = 3.0f;
        }
    }

    private void Return()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -15);
        transform.Translate(Vector3.left * speed * Time.deltaTime * 20);
        if (Mathf.Abs(transform.position.y - preferedElevation) < 0.5f)
        {
            returnToRoam = 0.0f;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        }
    }

    public override void Damage(float dmg, GameObject damageOrigin)
    {
        if (!damageOrigin.Equals(gameObject))
        {
            health -= dmg;
            if (health <= 0)
            {
                GameObject scraps = Instantiate(scrapParticles);
                scraps.transform.position = transform.position;
                transform.parent = null;
                GameObject.Find("Player").GetComponent<TransitionScript>().CheckObjectives();
                GameObject.Destroy(gameObject);
            }
            color = new Color(1 - health / maxHealth, 0, health / maxHealth);
            block.SetColor("_Color", color);
            blockRenderer.SetPropertyBlock(block);
        }
    }
}
