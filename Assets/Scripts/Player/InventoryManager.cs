using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryManager : MonoBehaviour
{
	public Item[] inventory = new Item[8];
	public ItemListData itemListData;
	public GameObject playerRightHand;
	private HUDManager hudManager;
	private GameObject spawnedItem;
	private GameObject player;
	private int maxStackSize = 50;

	public void SetHudManager(HUDManager _hudManager) {
		hudManager = _hudManager;
	}

	public void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Item") {
			// GameObject parent = other.gameObject.transform.parent.gameObject;
			// ItemData itemData = itemListData.GetItemData(parent.name);
			
			// PickUpItem(itemData);
			// PhotonNetwork.Destroy(parent);
		}
	}

	private void PickUpItem(ItemData itemData) {
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
			hudManager.DrawInventoryHud(inventory);
		}
	}

	public void SpawnSelectedItem(Item selectedItem) {
		// if (spawnedItem != null) {
		// 	PhotonNetwork.Destroy(spawnedItem);
		// }
		// GameObject item = PhotonNetwork.Instantiate(selectedItem.itemData.itemName, gameObject.transform.position, Quaternion.identity, 0);
		// item.GetComponent<Rigidbody>().detectCollisions = false;
		// item.GetComponent<Rigidbody>().useGravity = false;
		// item.name = selectedItem.itemData.itemName;
		// // item.transform.position = playerRightHand.transform.position;
		// // item.transform.rotation = playerRightHand.transform.rotation;
		// // item.transform.rotation = playerRightHand.transform.rotation * Quaternion.Euler(selectedItem.itemData.pickupRotation);
		// // item.transform.SetParent(playerRightHand.transform);
		// spawnedItem = item;
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