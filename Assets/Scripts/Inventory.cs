using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory : MonoBehaviour
{

	private RectTransform inventoryRect;
	private float inventoryWidth, inventoryHeight;
	private float slotPaddingLeft, slotPaddingTop;
	private float slotSize;
	private float borderSize;
	public GameObject invBlueKey;
	public GameObject invGreenKey;
	public GameObject invRedKey;
	public GameObject invYellowKey;
	public GameObject invFireBoot;
	public GameObject invSkates;
	public GameObject invFlipper;
	public GameObject invSlot;
	private GameObject newSlot;
	private GameObject slotBorder;
	public List<int> items;

	// Use this for initialization
	void Start ()
	{
		gameObject.SetActive (false);

		slotPaddingLeft = 10;
		slotPaddingTop = 10;
		slotSize = 50;
		borderSize = 75;
	}

	// Update is called once per frame
	void Update ()
	{
		//items.TrimExcess ();
	}

	public void CreateLayout (int i)
	{
		gameObject.SetActive (true);
//		inventoryWidth = slotPaddingLeft + borderSize + slotPaddingLeft;
//		inventoryHeight = (items.Count+1) * (slotPaddingTop + borderSize) + slotPaddingTop;

		inventoryHeight = 0f;
		inventoryWidth = 0f;

		inventoryRect = GetComponent<RectTransform> ();

		inventoryRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, inventoryWidth);
		inventoryRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, inventoryHeight);

		items.Add (i);

		DrawInv ();


	}

	public void DrawInv ()
	{

		for (int j = 0; j < items.Count - 1; j++) {
			Destroy (GameObject.Find (items [j].ToString ()));
			Destroy (GameObject.Find (items [j].ToString () + "slot"));
		}

		for (int j = 0; j < items.Count; j++) {

			if (items [j] == 0) {
				slotBorder = (GameObject)Instantiate (invSlot);
				newSlot = (GameObject)Instantiate (invBlueKey);
			} else if (items [j] == 1) {
				slotBorder = (GameObject)Instantiate (invSlot);
				newSlot = (GameObject)Instantiate (invGreenKey);
			} else if (items [j] == 2) {
				slotBorder = (GameObject)Instantiate (invSlot);
				newSlot = (GameObject)Instantiate (invRedKey);
			} else if (items [j] == 3) {
				slotBorder = (GameObject)Instantiate (invSlot);
				newSlot = (GameObject)Instantiate (invYellowKey);
			} else if (items [j] == 4) {
				slotBorder = (GameObject)Instantiate (invSlot);
				newSlot = (GameObject)Instantiate (invFireBoot);
			} else if (items [j] == 5) {
				slotBorder = (GameObject)Instantiate (invSlot);
				newSlot = (GameObject)Instantiate (invSkates);
			} else if (items [j] == 6) {
				slotBorder = (GameObject)Instantiate (invSlot);
				newSlot = (GameObject)Instantiate (invFlipper);
			}

			RectTransform invslotRect = slotBorder.GetComponent<RectTransform> ();
			slotBorder.name = items [j].ToString () + "slot";
			slotBorder.transform.SetParent (this.transform.parent);
			invslotRect.localPosition = inventoryRect.localPosition + new Vector3 (slotPaddingLeft, (-slotPaddingTop * (j + 1)) - (borderSize * j)); 
			invslotRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, borderSize);
			invslotRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, borderSize);

			RectTransform slotRect = newSlot.GetComponent<RectTransform> ();
			newSlot.name = items [j].ToString ();
			newSlot.transform.SetParent (this.transform.parent);
			slotRect.localPosition = inventoryRect.localPosition + new Vector3 (slotPaddingLeft + ((borderSize - slotSize) / 2), (-slotPaddingTop * (j + 1)) - (borderSize * j) - ((borderSize - slotSize) / 2));
			slotRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, slotSize);
			slotRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, slotSize);
		}
	}

}
