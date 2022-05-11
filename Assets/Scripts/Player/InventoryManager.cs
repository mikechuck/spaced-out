using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
	public Item[] inventory;
	// public ListMapping[] itemList;
	public ItemListData itemListData;
	private int maxStackSize = 50;
	public GameObject HUD;
	private HUDManager hudManager;

	void Start() {
		hudManager = HUD.GetComponent<HUDManager>();
		inventory = new Item[8];
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
		bool pickedUp = false;
		for (int i = 0; i < inventory.Length; i++) {
			if (inventory[i] != null) {
				if (itemData.itemName == inventory[i].itemData.itemName && itemData.stackable) {
					inventory[i].updateQuantity(inventory[i].quantity + 1);
					pickedUp = true;
					Debug.Log("breaking 1");
					break;
				}
			} else {
				inventory[i] = new Item(itemData, 1, 100);
				pickedUp = true;
				Debug.Log("breaking 2");
				break;
			}
		}

		if (pickedUp) {
			hudManager.DrawInventoryItems(inventory);
			// Destroy(parent);
		} else {
			Debug.Log("inventory full!");
		}

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