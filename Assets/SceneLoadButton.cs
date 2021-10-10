using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoadButton : MonoBehaviour
{
	public Button yourButton;
	public string sceneName;

	public GameObject[] enable;
	public GameObject[] disable;
	void Start()
	{
		yourButton.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick()
	{
		if (!sceneName.Equals(""))
        {
			SceneManager.LoadScene(sceneName);
        }
		foreach(GameObject g in enable)
        {
			g.SetActive(true);
        }
		foreach(GameObject g in disable)
        {
			g.SetActive(false);
        }
	}
}
