using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
	private Sprite[] inventorySprites = new Sprite[8];
	
	void OnGUI() {
		CreateInventoryBoxes();
	}

	void Start() {
		
	}

	private void CreateInventoryBoxes() {
		// Spawn all 8 inventory slots
		DrawQuad(new Rect(250f, 500f, 100f, 100f), new Color(255, 255, 255));
		DrawQuad(new Rect(375f, 500f, 100f, 100f), new Color(255, 255, 255));
		DrawQuad(new Rect(500f, 500f, 100f, 100f), new Color(255, 255, 255));
		DrawQuad(new Rect(625f, 500f, 100f, 100f), new Color(255, 255, 255));
		DrawQuad(new Rect(750f, 500f, 100f, 100f), new Color(255, 255, 255));
		DrawQuad(new Rect(875f, 500f, 100f, 100f), new Color(255, 255, 255));
		DrawQuad(new Rect(1000f, 500f, 100f, 100f), new Color(255, 255, 255));
		DrawQuad(new Rect(1125f, 500f, 100f, 100f), new Color(255, 255, 255));
	}

	void DrawQuad(Rect position, Color color) {
		Texture2D texture = new Texture2D(1, 1);
		texture.SetPixel(0,0,color);
		texture.Apply();
		GUI.skin.box.normal.background = texture;
		GUI.Box(position, GUIContent.none);
	}

	public void DrawInventoryItems(Item[] inventory) {
		GameObject hudGameObject = GameObject.Find("HUD");
		GameObject[] inventorySlots = GameObject.FindGameObjectsWithTag("Inventory Slot");

		for (int i = 0; i < inventorySlots.Length; i++) {
			Image currentSlot = inventorySlots[i].GetComponent<Image>();
			if (inventory[i] != null) {
				currentSlot.enabled = true;
        		currentSlot.sprite = inventory[i].itemData.itemImage;
			} else {
				currentSlot.color = new Color32(91, 75, 56, 225);
			}
		}
	}
}
