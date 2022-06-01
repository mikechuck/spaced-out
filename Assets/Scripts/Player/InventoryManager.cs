using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
	public Item[] inventory;
	// public ListMapping[] itemList;
	public ItemListData itemListData;
	private int maxStackSize = 50;
	private GameObject HUD;
	private HUDManager hudManager;
	private GameManager gameManager;

	void Awake() {
		HUD = GameObject.Find("HUD");
		hudManager = HUD.GetComponent<HUDManager>();
		inventory = new Item[8];
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Item") {
			GameObject parent = other.gameObject.transform.parent.gameObject;
			
			// Debug.Log(itemData.itemName);
			PickUpItem(parent);
		}
	}

	public void PickUpItem(GameObject parent) {
		ItemData itemData = itemListData.GetItemData(parent.name);

		Debug.Log("----------");
		Debug.Log("picked up item:");
		Debug.Log(itemData.itemName);
		Debug.Log("by player");
		Debug.Log(gameManager.playerName);
		Debug.Log("----------");

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
			Destroy(parent);
		}

	}
	public void DisplaySelectedItem(ItemData selectedItem) {
		Debug.Log("selecting");
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