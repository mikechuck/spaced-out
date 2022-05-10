using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
	public ItemData genericWoodItemData;
	public ItemData axeItemData;

	public ItemData[] items;
	public ListMapping[] itemList;

	public void PickUpItem(string itemType) {
		// items = 
	}
}


public struct Item {
	public ItemData itemData;
	public int quantity;
	public int hp;
	public Item(ItemData itemData, int quantity, int hp) {
		this.itemData = itemData;
		this.quantity = quantity;
		this.hp = hp;
	}

}

public struct ListMapping {
	public string itemName;
	public ItemData itemData;
}