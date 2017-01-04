using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{

	public bool dead;
	public GameObject restartButton;
	public GameObject levelButton;
	public InputField levelSelect;
	private int i;
	// Use this for initialization
	void Start ()
	{
		dead = false;
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (dead) {
			restartButton.SetActive (true);
		} else
			restartButton.SetActive (false);
	}

	public void PlayButton ()
	{
		SceneManager.LoadScene ("Level1", LoadSceneMode.Single);
	}

	public void LevelSelect ()
	{
		i = int.Parse (levelSelect.text);

		SceneManager.LoadScene ("Level" + i, LoadSceneMode.Single);
		GlobalControl.Instance.level = i;
	}

	public void restartLevel ()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name, LoadSceneMode.Single);
	}
}
