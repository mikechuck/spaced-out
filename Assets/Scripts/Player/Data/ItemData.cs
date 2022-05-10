using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ItemData: ScriptableObject
{
    public string itemName;
	public GameObject itemPrefab;
	public int damage;
	public bool canEquip;
	public bool stackable;
	public bool canEat;
	public bool canUse;
	public bool canSwing;
}
