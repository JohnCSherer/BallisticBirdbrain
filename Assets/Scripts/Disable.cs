using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disable : MonoBehaviour
{
    private float DisableTime;
    public float Delay = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        DisableTime = Time.time + Delay;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= DisableTime)
        {
            gameObject.SetActive(false);
        }
    }

    public void SetTimer()
    {
        DisableTime = Time.time + Delay;
    }
}
