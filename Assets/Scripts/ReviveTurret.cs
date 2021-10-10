using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveTurret : MonoBehaviour
{
    private float reviveTime;
    public Transform playerPos;
    public float playerPrivacy;
    public Turret turretScript;
    // Start is called before the first frame update
    public void SetTime(float t)
    {
        reviveTime = Time.time + t;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > reviveTime && Vector3.Distance(transform.position, playerPos.position) >= playerPrivacy)
        {
            turretScript.enabled = true;
            turretScript.Revive();
            this.enabled = false;
        }

    }
}
