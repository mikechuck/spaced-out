using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteraction : MonoBehaviour {
	public ItemListData itemListData;
    void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Item") {
			ItemData itemData = itemListData.GetItemData(other.gameObject.name);
			Debug.Log(itemData.itemName);
		}
	}
}
