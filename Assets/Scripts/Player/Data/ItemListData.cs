using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ItemListData: ScriptableObject
{
	[System.Serializable]
	public class ItemListEntry {
		[Space]
        [Tooltip("Name should match how it is spelled in resources folder.")]
        [Space]
		public string itemName;
		public ItemData itemData;
	}
    public ItemListEntry[] items;
	Dictionary<string, ItemData> itemDict = new Dictionary<string, ItemData>();
	public float[] baseStartHeights;

	public ItemData GetItemData(string itemName) {
		PopulateDict();
		return itemDict[itemName];
	}

	private void PopulateDict() {
		// only populate if it hasn't already been done
		if (itemDict.Count == 0) {
			foreach(ItemListEntry item in items) {
				itemDict.Add(item.itemName, item.itemData);
			}
		}
	}
}