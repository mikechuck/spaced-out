using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

public class InventoryManager : MonoBehaviour
{
	public Item[] inventory;
	// public ListMapping[] itemList;
	public ItemListData itemListData;
	public GameObject playerRightHand;
	private int maxStackSize = 50;
	private GameObject player;
	private GameObject spawnedItem;
	private UnityAction<Item> spawnSelectedItemAction;


	void Awake() {
		inventory = new Item[8];
		spawnSelectedItemAction = new UnityAction<Item>(SpawnSelectedItem);
	}

	void OnEnable() {
		EventManager.StartListening("SpawnSelectedItem", spawnSelectedItemAction);
	}

	void OnDisable() {
		EventManager.StopListening("SpawnSelectedItem", spawnSelectedItemAction);
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
			EventManager.TriggerEvent("DrawInventoryHud", inventory);
			Destroy(parent);
		}

	}
	public void SpawnSelectedItem(Item selectedItem) {
		if (spawnedItem != null) {
			PhotonNetwork.Destroy(spawnedItem);
		}
		GameObject item = PhotonNetwork.Instantiate(selectedItem.itemData.itemName, gameObject.transform.position, Quaternion.identity, 0);
		item.GetComponent<Rigidbody>().detectCollisions = false;
		item.GetComponent<Rigidbody>().useGravity = false;
		item.name = selectedItem.itemData.itemName;
		item.transform.SetParent(playerRightHand.transform);
		spawnedItem = item;
	}

	private void _OnTest() {
		Debug.Log("recieved test event! (picked up item)");
	}

	private void _DisplayItem() {
		Debug.Log("displayign item on scroll");
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