using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hitable : MonoBehaviour
{
    public float maxHealth;
    protected float health;

    public virtual void Damage(float dmg, GameObject damageOrigin)
    {
        health -= maxHealth;
    }

    public virtual void Stun(float time)
    {

    }
}