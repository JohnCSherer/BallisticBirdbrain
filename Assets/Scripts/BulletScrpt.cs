using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScrpt : MonoBehaviour
{
    public float bulletSpeed = 20.0f;
    public float bulletDamage = 1.0f;
    public GameObject origin;
    public static float despawnDisance = 1000.0f;
    public GameObject sparks;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;
        if(transform.position.magnitude > despawnDisance)
        {
            GameObject.Destroy(gameObject);
        }
        if(Physics.Raycast(transform.position, transform.up * -Time.deltaTime, out hit, bulletSpeed * Time.deltaTime, ~(1 << 9)))
        {
            if (hit.collider.gameObject.CompareTag("hitable"))
            {
                hit.collider.GetComponent<Hitable>().Damage(bulletDamage, origin);
            }
            GameObject sph = Instantiate(sparks);
            sph.transform.position = hit.point;

            GameObject.Destroy(gameObject);
        }
        transform.position += (transform.up * -bulletSpeed * Time.deltaTime);
    }
}
