using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalControl : MonoBehaviour
{

	public static GlobalControl Instance;

	public int score;
	public int level;

	// Use this for initialization
	void Start ()
	{
		level = int.Parse (SceneManager.GetActiveScene ().name.Remove (0, 5));
		score = 0;
	}

	// Update is called once per frame
	void Update ()
	{

	}

	void Awake ()
	{

		if (Instance == null) {
			DontDestroyOnLoad (gameObject);
			Instance = this;
		} else if (Instance != this) {
			Destroy (gameObject);
		}

	}
}
