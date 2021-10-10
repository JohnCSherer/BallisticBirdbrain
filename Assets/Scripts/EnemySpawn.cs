using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemySpawn : MonoBehaviour
{
    public int NumberOfBots = 20;
    public float duration = 20;
    public int botsWrecked = 0;
    private int botsDeployed = 0;
    public GameObject botTemplate;
    public float rovingRange = 60;
    private float startTime;

    private CameraScript cam;

    public Text victoryText;
    private bool won = false;
    public string nextScene;

    private void Awake()
    {
        cam = GameObject.Find("Player").GetComponent<CameraScript>();
        startTime = Time.time;
        won = false;
    }

    void Update()
    {
        if (botsDeployed < (int) Mathf.Min((Time.time - startTime) / duration, 1) * NumberOfBots)
        {
            SpawnBot();
            botsDeployed++;
        }

        if (Input.GetMouseButtonDown(0) && won)
        {
            SceneManager.LoadScene(nextScene);
        }

        
    }

    public void SpawnBot()
    {
        GameObject thisBot = Instantiate(botTemplate);
        

        thisBot.transform.position = transform.GetChild(UnityEngine.Random.Range(0, transform.childCount)).position;
    }

    public void HasDied()
    {
        botsWrecked += 1;
        if(botsWrecked == NumberOfBots)
        {
            cam.CompleteRoom(botsWrecked, nextScene);
            
        }
    }
}
