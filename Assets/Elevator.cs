using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    private Rigidbody body;
    public float[] HeightSchedule;
    public float speed;
    public float waitTime;

    private float departTime = 0;
    private int scheduleIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        speed *= 20;
    }

    void Awake()
    {
        if (HeightSchedule.Length == 0)
        {
            Debug.LogError("Error, Missing height schedule for elevator");
        }
        departTime = Time.time + waitTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Time.time >= departTime)
        {
            if(transform.position.y < HeightSchedule[scheduleIndex] - speed * Time.deltaTime)
            {
                transform.position += new Vector3(0, speed * Time.deltaTime, 0);
            }else if(transform.position.y > HeightSchedule[scheduleIndex] + speed * Time.deltaTime){
                transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, HeightSchedule[scheduleIndex], transform.position.z);
                departTime = Time.time + waitTime;
                scheduleIndex++;
                if (scheduleIndex >= HeightSchedule.Length)
                    scheduleIndex = 0;
            }
        }
    }
}
