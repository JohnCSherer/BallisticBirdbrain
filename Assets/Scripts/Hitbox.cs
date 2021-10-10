using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public float damage = 500;
    public GameObject owner;
    public virtual void TakeDamage(Hitable script)
    {
        script.Damage(damage, owner);
    }
}
