using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionScript : MonoBehaviour
{
    private GameObject objectives;

    // Start is called before the first frame update
    void Start()
    {
        objectives = GameObject.Find("Objectives");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckObjectives()
    {
        if(objectives.transform.childCount == 0)
        {
            if(SceneManager.GetActiveScene().buildIndex <= SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                Debug.Log("End of zone list: " + SceneManager.GetActiveScene().buildIndex + ", " + SceneManager.sceneCountInBuildSettings);
            }
        }
        else
        {
            Debug.Log(objectives.transform.childCount);
        }
    }
}
