using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBox : Hitbox
{
    public float force;
    private ArrayList alreadyDamaged;
    public float duration;
    private float endTime;
    private float range;

    private void Awake()
    {
        Reset();
    }

    public void Reset()
    {
        alreadyDamaged = new ArrayList();
        endTime = Time.time + duration;
        range = GetComponent<Collider>().bounds.extents.x * 2;
    }

    private void Update()
    {
        if (Time.time >= endTime)
        {
            alreadyDamaged = new ArrayList();
            gameObject.SetActive(false);
        }
    }
    public override void TakeDamage(Hitable script)
    {
        script.Stun(1.0f);
        Rigidbody body = script.GetComponent<Rigidbody>();
        if (body != null && !script.gameObject.Equals(owner) && !alreadyDamaged.Contains(script))
        {
            
            body.velocity += ( (body.position - transform.position + Vector3.up * 4).normalized) * force * 2;
            if(!body.isKinematic)
                body.position += Vector3.up*0.5f;
            alreadyDamaged.Add(script);
        }
        
        if (script.name.Equals("Turret"))
        {
            script.Damage(damage * (Vector3.Distance(script.transform.position, transform.position) / range)*8, owner);
        }
        else
        {
            script.Damage(damage * (Vector3.Distance(script.transform.position, transform.position) / range), owner);
        }
    }
}
