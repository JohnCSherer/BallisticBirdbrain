using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteSelf : MonoBehaviour
{
    private float endTime;
    public float delay = 2;
    // Start is called before the first frame update
    void Awake()
    { 
        endTime = Time.time + delay;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > endTime)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
