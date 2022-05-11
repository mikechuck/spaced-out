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
			ItemData itemData = itemListData.GetItemData(other.gameObject.name);
			// Debug.Log(itemData.itemName);
			PickUpItem(itemData);
		}
	}

	public void PickUpItem(ItemData itemData) {
		// check for maxstacksize, if no items are found without exceeding limit, create new item
		// if (itemData.stackable) {
			// loop through, find same type, increase qty by 1
			bool addedToInventory = false;
			for (int i = 0; i < inventory.Length; i++) {
				if (inventory[i] != null) {
					if (itemData.itemName == inventory[i].itemData.itemName && itemData.stackable) {
						inventory[i].updateQuantity(inventory[i].quantity + 1);
						addedToInventory = true;
						Debug.Log("breaking 1");
						break;
					}
				} else {
					inventory[i] = new Item(itemData, 1, 100);
					addedToInventory = true;
					Debug.Log("breaking 2");
					break;
				}
			}

			if (addedToInventory) {
				hudManager.DrawInventoryItems(inventory);
			} else {
				Debug.Log("inventory full!");
			}
		// } else {
		// 	Item newItem = new Item(itemData, 1, 100);
		// 	inventory.Add(newItem);
		// }

		// for ()

		// hudManager.DrawInventoryItems();
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