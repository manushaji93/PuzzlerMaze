using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{

	public int level;
	public int score;
	public Text scoreText;
	public Text countText;
	string s;
	private Camera maincam;
	public PlayerMovement player;
	private int count1;

	void Start ()
	{
		maincam = Camera.main;
		score = GlobalControl.Instance.score;
//		startTime = Time.time;

	}

	void Update ()
	{	
		if (GlobalControl.Instance.level > 6) {
			Vector3 temp = new Vector3 (player.transform.position.x, player.transform.position.y, maincam.transform.position.z);
			maincam.transform.position = temp;
		}
			
	}

	void FixedUpdate ()
	{
		SetText ();
	}

	void SetText ()
	{
		count1 = player.count;
		scoreText.text = "Score: " + score.ToString ();
		countText.text = "Coins Remaining: " + count1.ToString ();

	}


}