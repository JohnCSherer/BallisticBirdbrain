using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float delay;
    private float endTime;
    public GameObject explosion;
    public GameObject particles;
    private Component body;
    // Start is called before the first frame update
    void Awake()
    {
        endTime = Time.time + delay;
        body = GetComponent<Rigidbody>();
        explosion.GetComponent<ExplosionBox>().owner = GameObject.Find("Explosives");
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > endTime)
        {
            particles.SetActive(true);
            explosion.SetActive(true);
            particles.transform.SetParent(null);
            explosion.transform.SetParent(null);
            GameObject.Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!collision.gameObject.name.Equals("Player"))
        {
            particles.SetActive(true);
            explosion.SetActive(true);
            particles.transform.SetParent(null);
            explosion.transform.SetParent(null);
            GameObject.Destroy(gameObject);
        }
    }
}
