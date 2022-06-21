using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TreeState : MonoBehaviour
{
    public int hp = 100;
	public GameObject itemDropped;
	
	public void DecreaseHP(int damage) {
		hp -= damage;

		if (hp <= 0) {
			Vector3 position = gameObject.transform.position;
			GameObject spawnedWood = PhotonNetwork.Instantiate(itemDropped.name, new Vector3(position.x, position.y + 10, position.z), Quaternion.identity, 0);
			spawnedWood.name = itemDropped.name;
			Destroy(gameObject);
		}
	}
}
