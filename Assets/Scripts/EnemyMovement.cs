using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

	private Rigidbody rb2d;
	private Vector3 angle;
	private Vector3 dir;
	private float speed = 3f;
	public Buttons button;
	private AudioSource sfx;
	public AudioClip deathSound;
	//private Vector3 scale;
	// Use this for initialization
	void Start ()
	{
		sfx = GetComponent<AudioSource> ();
		rb2d = GetComponent<Rigidbody> ();
		angle = gameObject.transform.eulerAngles;
		//scale = rb2d.transform.localScale;

		if (angle.z == 90)
			dir = new Vector3 (-1f, 0f, 0f);
		else if (angle.z == 0)
			dir = new Vector3 (0f, 1f, 0f);
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		rb2d.velocity = speed * dir;
	}

	void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.tag == "Wall" || col.gameObject.tag == "Enemy" || col.gameObject.tag == "RedDoor" || col.gameObject.tag == "BlueDoor" || col.gameObject.tag == "YellowDoor" || col.gameObject.tag == "GreenDoor") {
			dir *= -1;
			gameObject.transform.eulerAngles = new Vector3 (gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, gameObject.transform.eulerAngles.z + 180);
		} else if (col.gameObject.tag == "Player") {
			sfx.clip = deathSound;
			sfx.Play ();
			Destroy (col.gameObject);
			button.dead = true;
		}
	}
}
