using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class CameraScript : MonoBehaviour
{

    private Transform face;
    private Rigidbody body;

    public float masterSensitivity = 1.0f;
    public float horizontalSensitivity = 1.0f;
    public float verticalSensitivity = 1.0f;
    private static float sensitivityCap = 256;

    public Text victoryText;

    private float deathTime = 0;

    private bool funct = true;
    private bool won = false;

    private string nextSceneName;

    public MovementScript moveScript;
    public GunScript gunScript;
    public AudioSource music;
    public PowerActivation powerScript;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        face = transform.GetChild(0).GetComponent<Transform>();
        body = GetComponent<Rigidbody>();
        moveScript = GetComponent<MovementScript>();
        gunScript = transform.GetChild(0).Find("gun").GetComponent<GunScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(deathTime != 0 && Time.time > deathTime)
        {
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("Game Over");
            Destroy(gameObject);
        }

        if (funct)
        {
            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");
            face.localEulerAngles += new Vector3(Math.Min(Math.Max(-y * horizontalSensitivity * masterSensitivity, -sensitivityCap), sensitivityCap), 0, 0);
            transform.localEulerAngles += new Vector3(0, Math.Max(Math.Min(x * verticalSensitivity * masterSensitivity, sensitivityCap), -sensitivityCap), 0);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            moveScript.enabled = !moveScript.enabled;
            gunScript.enabled = !gunScript.enabled;
            funct = !funct;
            if (music.isPlaying)
                music.Pause();
            else
                music.Play();
            Time.timeScale = 1 - Time.timeScale;
        }

        if (Input.GetMouseButtonDown(0) && won)
        {
            SceneManager.LoadScene(nextSceneName);
            moveScript.enabled = true;
            gunScript.enabled = true;
            powerScript.enabled = true;
            powerScript.Refresh();
            music.Play();
            won = false;
            victoryText.text = "";
        }
    }

    public void CompleteRoom(int bots, string nextScene)
    {
        moveScript.enabled = false;
        gunScript.enabled = false;
        powerScript.enabled = false;
        
        won = true;
        victoryText.text = "Succes!\nBots Destroyed: " + bots + "\nClick to proceed\nto the next chamber...";
        nextSceneName = nextScene;
    }

    public void Die()
    {
        moveScript.enabled = false;
        gunScript.enabled = false;
        GetComponent<AudioSource>().Play();
        deathTime = Time.time + 3.0f;
    }

    public static void CapSensitivity(float value)
    {
        sensitivityCap = value;
    }
}
