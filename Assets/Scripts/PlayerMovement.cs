using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
	public Inventory inv;
	//private Camera maincam;
	public LayerMask ignoreThis;
	public LayerMask redDoor;
	public LayerMask blueDoor;
	public LayerMask greenDoor;
	public LayerMask yellowDoor;
	public LayerMask fireBlock;
	public LayerMask waterBlock;
	public LayerMask iceBlock;
	public LayerMask floorBlock;
	public RaycastHit hit;
	public Main main;
	public int count;

	private AudioSource sfx;
	public AudioClip footStepSound;
	public AudioClip coinSound;
	public AudioClip keysSound;
	public AudioClip bootsSound;
	public AudioClip iceSound;

	public Vector3 dist;
	public Vector3 dir;
	public bool slip;
	private bool moving;


	//VARIABLES TO CHECK IF COLLECTED ITEM:
	private int RKey;
	private int BKey;
	private int GKey;
	private int YKey;
	private bool fireBoot;
	private bool flipper;
	private bool skates;

	void Start ()
	{
		//maincam = Camera.main;
		slip = false;
		moving = false;

		//SETTING INITIAL 'COIN REMAINING' COUNT:
		count = GameObject.FindGameObjectsWithTag ("Coin").Length;

		//INITIALIZE COLLECTED ITEM COUNT:
		RKey = 0;
		BKey = 0;
		GKey = 0;
		YKey = 0;
		fireBoot = false;
		flipper = false;
		skates = false;

		sfx = GetComponent<AudioSource> ();
	}

	void Update ()
	{
		
		
		if (count == 0) //OPEN EXITDOOR ON COLLECTING ALL COINS
			Destroy (GameObject.Find ("ExitDoor"));

		//DELETE FROM INVENTORY ON USING ALL COLLECTED KEYS:
		if (RKey == 0) {
			Destroy (GameObject.Find ("2"));
			Destroy (GameObject.Find ("2slot"));
			inv.items.Remove (2);
		}
		
		if (BKey == 0) {
			Destroy (GameObject.Find ("0"));
			Destroy (GameObject.Find ("0slot"));
			inv.items.Remove (0);
		}

		if (YKey == 0) {
			Destroy (GameObject.Find ("3"));
			Destroy (GameObject.Find ("3slot"));
			inv.items.Remove (3);
		}

		if (GKey == 0) {
			Destroy (GameObject.Find ("1"));
			Destroy (GameObject.Find ("1slot"));
			inv.items.Remove (1);
		}

		//PLAYER MOVEMENT:
		if (!slip && !moving) {

			if (Input.GetKeyDown (KeyCode.UpArrow)) {
				dist = new Vector3 (0f, 0.76f, 0f);
				dir = Vector3.up;
				Move (dist, Vector3.up, 0);
				sfx.clip = footStepSound;
				sfx.Play ();

			} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
				dist = new Vector3 (0f, -0.76f, 0f);
				dir = Vector3.down;
				Move (dist, Vector3.down, 180);
				sfx.clip = footStepSound;
				sfx.Play ();

			} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				dist = new Vector3 (-0.76f, 0f, 0f);
				dir = Vector3.left;
				Move (dist, Vector3.left, 90);
				sfx.clip = footStepSound;
				sfx.Play ();

			} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
				dist = new Vector3 (0.76f, 0f, 0f);
				dir = Vector3.right;
				Move (dist, Vector3.right, -90);
				sfx.clip = footStepSound;
				sfx.Play ();

			}
		}
	}

	IEnumerator FollowPlayer (Vector3 startPos, Vector3 newPos)
	{
		moving = true;
		float time = 0.1f;
		float i = 0f;
		float rate = 1f / time;
		while (i < 1) {
			i += Time.deltaTime * rate;
			gameObject.transform.position = Vector3.Lerp (startPos, newPos, i);
			yield return null;
		}
		moving = false;
	}

	void Move (Vector3 dist, Vector3 dir, int angle)
	{

		Vector3 startPosition = gameObject.transform.position; //PLAYER CURRENT LOCATION (PCL)
		Vector3 endPosition = gameObject.transform.position + dist; //PLAYER DESIRED LOCATION = PCL + UNIT STEP IN DESIRED DIRECTION 

		//RAYCAST TO DETECT WALLS USING LAYERMASK 'WALL':
		if (!Physics.Raycast (startPosition, dir, 0.76f, ignoreThis)) {
			StartCoroutine (FollowPlayer (startPosition, endPosition));
			gameObject.transform.eulerAngles = new Vector3 (gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, angle);

		} 

		//RAYCAST TO DETECT DOORS:
		else if (Physics.Raycast (startPosition, dir, out hit, 0.76f, redDoor)) {
			if (RKey > 0) { //CONFIRM KEY IS PRESENT IN INVENTORY TO OPEN CORRESPONDING DOOR
				Destroy (hit.transform.gameObject); //DESTROY DOOR
				StartCoroutine (FollowPlayer (startPosition, endPosition));
				gameObject.transform.eulerAngles = new Vector3 (gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, angle);
				RKey -= 1; //REDUCE TOTAL KEY AMOUNT
			}
		} else if (Physics.Raycast (startPosition, dir, out hit, 0.76f, blueDoor)) {
			if (BKey > 0) {
				Destroy (hit.transform.gameObject);
				StartCoroutine (FollowPlayer (startPosition, endPosition));
				gameObject.transform.eulerAngles = new Vector3 (gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, angle);

				BKey -= 1;
			}
		} else if (Physics.Raycast (startPosition, dir, out hit, 0.76f, greenDoor)) {
			if (GKey > 0) { //CONFIRM KEY IS PRESENT IN INVENTORY TO OPEN CORRESPONDING DOOR
				Destroy (hit.transform.gameObject); //DESTROY DOOR
				StartCoroutine (FollowPlayer (startPosition, endPosition));
				gameObject.transform.eulerAngles = new Vector3 (gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, angle);

				GKey -= 1; //REDUCE TOTAL KEY AMOUNT
			}
		} else if (Physics.Raycast (startPosition, dir, out hit, 0.76f, yellowDoor)) {
			if (YKey > 0) {
				Destroy (hit.transform.gameObject);
				StartCoroutine (FollowPlayer (startPosition, endPosition));
				gameObject.transform.eulerAngles = new Vector3 (gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, angle);

				YKey -= 1;
			}
		} 


		//RAYCAST TO DETECT SPECIAL BLOCKS:
		else if (Physics.Raycast (startPosition, dir, out hit, 0.76f, fireBlock)) {
			if (fireBoot) { //CHECK FOR APPROPRIATE BOOTS
				hit.collider.enabled = false; //DISABLE COLLIDER TO ENABLE PLAYER TO MOVE IN INSTEAD OF DELETING BLOCK
				StartCoroutine (FollowPlayer (startPosition, endPosition));
				gameObject.transform.eulerAngles = new Vector3 (gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, angle);

			}
		} else if (Physics.Raycast (startPosition, dir, out hit, 0.76f, waterBlock)) {
			if (flipper) {
				hit.collider.enabled = false;
				StartCoroutine (FollowPlayer (startPosition, endPosition));
				gameObject.transform.eulerAngles = new Vector3 (gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, angle);

			}
		} else if (Physics.Raycast (startPosition, dir, out hit, 0.76f, iceBlock)) {
			if (skates) {
				hit.collider.enabled = false;
				StartCoroutine (FollowPlayer (startPosition, endPosition));
				gameObject.transform.eulerAngles = new Vector3 (gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, angle);

			} 
		}

	}


	void OnTriggerEnter (Collider col)
	{
		//COLLECTING COINS AND ITEMS:
		if (col.gameObject.tag == "Coin") {
			sfx.clip = coinSound;
			sfx.Play ();
			main.score += 5;
			count -= 1;
			Destroy (col.gameObject);
		} else if (col.gameObject.tag == "RedKey") {
			sfx.clip = keysSound;
			sfx.Play ();
			if (RKey == 0) {  //CHECK FOR EXISTING INVENTORY KEY BEFORE INSTANTIATING IN INVENTORY
				inv.CreateLayout (2);
			}
			RKey += 1; //INCREASE KEY COUNT
			Destroy (col.gameObject); //DESTROY IN-GAME KEY
		} else if (col.gameObject.name == "FireBoot") {
			sfx.clip = bootsSound;
			sfx.Play ();
			fireBoot = true;
			Destroy (col.gameObject);
			inv.CreateLayout (4);
		} else if (col.gameObject.tag == "BlueKey") {
			sfx.clip = keysSound;
			sfx.Play ();
			if (BKey == 0) {
				inv.CreateLayout (0);
			}
			BKey += 1;
			Destroy (col.gameObject);
		} else if (col.gameObject.tag == "YellowKey") {
			sfx.clip = keysSound;
			sfx.Play ();
			if (YKey == 0) {
				inv.CreateLayout (3);
			}
			YKey += 1;
			Destroy (col.gameObject);
		} else if (col.gameObject.tag == "GreenKey") {
			sfx.clip = keysSound;
			sfx.Play ();
			if (GKey == 0) {
				inv.CreateLayout (1);
			}
			GKey += 1;
			Destroy (col.gameObject);
		} else if (col.gameObject.name == "Flipper") {
			sfx.clip = bootsSound;
			sfx.Play ();
			flipper = true;
			Destroy (col.gameObject);
			inv.CreateLayout (6);
		} else if (col.gameObject.name == "Skates") {
			sfx.clip = bootsSound;
			sfx.Play ();
			skates = true;
			Destroy (col.gameObject);
			inv.CreateLayout (5);
		} else if (col.gameObject.tag == "Exit") {
			GlobalControl.Instance.score = main.score; //UPDATE FINAL LEVEL SCORE TO GLOBALCONTROL SCRIPT TO NOT GET DESTROYED ON LOADING NEW LEVEL
			GlobalControl.Instance.level += 1;
			SceneManager.LoadScene ("Level" + GlobalControl.Instance.level, LoadSceneMode.Single); //LOAD NEXT LEVEL
		} else if (col.gameObject.tag == "Ice") {
			if (!skates) {
				slip = true;
				sfx.clip = iceSound;
				sfx.Play ();
				GetComponent<Rigidbody> ().velocity = dist * 10f;
			}

		} 
	}

	void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.tag == "Wall" || col.gameObject.tag == "RedDoor" || col.gameObject.tag == "BlueDoor" || col.gameObject.tag == "YellowDoor" || col.gameObject.tag == "GreenDoor") {
			slip = false;
			GetComponent<Rigidbody> ().velocity = new Vector3 (0f, 0f, 0f);
			Vector3 pos = col.transform.position;
			gameObject.transform.position = new Vector3 (pos.x, pos.y, -1f) - dist;
		}

	}

	void OnTriggerStay (Collider col)
	{
		if (col.gameObject.tag == "Ice" && Physics.Raycast (gameObject.transform.position, dir, out hit, 0.50f, floorBlock)) {
			slip = false;
			GetComponent<Rigidbody> ().velocity = new Vector3 (0f, 0f, 0f);
			Vector3 pos = hit.transform.position;
			gameObject.transform.position = new Vector3 (pos.x, pos.y, -1f);
		}

	}


}
