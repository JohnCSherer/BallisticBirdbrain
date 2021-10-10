using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialItemScript : MonoBehaviour
{

    public GameObject grenade;
    public float grenadeTrajectory;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void ThrowGrenade()
    {
        GameObject gren = Instantiate(grenade);
        gren.transform.position = transform.position + transform.forward - transform.right;
        gren.transform.rotation = transform.rotation;
        gren.GetComponent<Rigidbody>().velocity = gren.transform.forward * grenadeTrajectory + transform.up*5;
    }
}
