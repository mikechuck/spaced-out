using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InventoryManager : MonoBehaviour
{
	public Item[] inventory;
	// public ListMapping[] itemList;
	public ItemListData itemListData;
	public GameObject playerRightHand;
	private int maxStackSize = 50;
	private GameObject HUD;
	private HUDManager hudManager;
	private GameManager gameManager;
	private GameObject player;

	void Awake() {
		HUD = GameObject.Find("HUD");
		hudManager = HUD.GetComponent<HUDManager>();
		inventory = new Item[8];
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		// player = GameObject.Find("Player 1");
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Item") {
			GameObject parent = other.gameObject.transform.parent.gameObject;
			
			PickUpItem(parent);
		}
	}

	public void PickUpItem(GameObject parent) {
		ItemData itemData = itemListData.GetItemData(parent.name);

		bool pickedUp = false;
		for (int i = 0; i < inventory.Length; i++) {
			if (inventory[i] != null) {
				if (itemData.itemName == inventory[i].itemData.itemName && itemData.stackable) {
					inventory[i].updateQuantity(inventory[i].quantity + 1);
					pickedUp = true;
					break;
				}
			} else {
				inventory[i] = new Item(itemData, 1, 100);
				pickedUp = true;
				break;
			}
		}

		if (pickedUp) {
			hudManager.DrawInventoryItems(inventory);
			// DisplaySelectedItem(itemData);
			Destroy(parent);
		}

	}
	public void DisplaySelectedItem(ItemData itemData) {
		GameObject item = PhotonNetwork.Instantiate(itemData.itemName, gameObject.transform.position, Quaternion.identity, 0);
		item.GetComponent<Rigidbody>().detectCollisions = false;
		item.GetComponent<Rigidbody>().useGravity = false;
		item.name = itemData.itemName;
		item.transform.SetParent(playerRightHand.transform);
	}
}


public class Item {
	public ItemData itemData;
	public int quantity;
	public int hp;
	public Item(ItemData itemData, int quantity, int hp) {
		this.itemData = itemData;
		this.quantity = quantity;
		this.hp = hp;
	}
	public void updateQuantity(int newValue) {
		this.quantity = newValue;
	}
}