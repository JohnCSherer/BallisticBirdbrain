using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunScript : MonoBehaviour
{
    public float ROF = 0.004f;
    public int clipSize = 26;
    public float devianceAngle = 0.1f;
    public Vector3 bulletSpawnOffset;
    private int clip;
    private float cooldown;

    private float reloadTimer = 0.0f;
    private float reloadTime = 1.3f;

    public GameObject bulletPrefab;
    public Transform barrel;
    private Animation anim;
    public Material gunMat;
    public GameObject flash;
    private AudioSource audioSource;

    public Text clipAmount;

    private void Start()
    {
        clip = clipSize;
        anim = GetComponent<Animation>();
        anim.wrapMode = WrapMode.Loop;
        cooldown = 0;
        audioSource = GetComponent<AudioSource>();
        barrel = transform.parent.Find("Barrel");

        clipAmount = GameObject.Find("Canvas").transform.Find("Clip").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(cooldown > 0)
        {
            if (ROF - cooldown > 0.02f)
            {
                flash.SetActive(false);
            }
            cooldown -= Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.R) && clip != clipSize)
        {
            Reload();
        }
        if (reloadTimer <= 0)
        {
            if (Input.GetMouseButton(0))
            {
                PullTrigger();
            }
        }
        else
        {
            reloadTimer -= Time.deltaTime;
            if(reloadTimer <= 0)
            {
                clip = clipSize;
            }
        }
        clipAmount.text = clip + "/" + clipSize;
    }

    private void PullTrigger()
    {
        if (clip > 0)
        {
            if (cooldown <= 0)
            {
                audioSource.Play();
                cooldown = ROF;
                GameObject bullet = Instantiate(bulletPrefab);
                bullet.transform.position = barrel.position;
                bullet.transform.rotation = barrel.rotation;
                bullet.GetComponent<BulletScrpt>().origin = transform.parent.parent.gameObject;
                bullet.transform.localEulerAngles += new Vector3(
                    UnityEngine.Random.Range(-devianceAngle, devianceAngle),
                    UnityEngine.Random.Range(-devianceAngle, devianceAngle),
                    UnityEngine.Random.Range(-devianceAngle, devianceAngle));
                anim.Play("Gun Shoot");
                anim.wrapMode = WrapMode.Once;
                anim.PlayQueued("Gun Idle");
                clip--;
                flash.transform.Rotate(0, 0, UnityEngine.Random.Range(0, 360));
                flash.SetActive(true);
            }
        }
        else
        {
            Reload();
        }
    }

    private void Reload()
    {
        anim.Play("Gun Reload");
        anim.wrapMode = WrapMode.Once;
        anim.PlayQueued("Gun Idle");
        reloadTimer = reloadTime;
    }
}
