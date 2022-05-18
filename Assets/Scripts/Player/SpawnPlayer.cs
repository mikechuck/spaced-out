using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayer : MonoBehaviour
{
	public GameObject playerPrefab;
	public TerrainData terrainData;
	public MapData mapData;
	public TextureData textureData;
	public GameObject genericWood;
	public GameObject axe;
	
	public void Spawn(MapData mapData) {
		float mapScale = terrainData.uniformScale;
		float heightScale = terrainData.meshHeightMultiplier;
		AnimationCurve heightCurve = new AnimationCurve(terrainData.meshHeightCurve.keys);
		float[,] heightMap = mapData.heightMap;
		int mapSize = heightMap.GetLength(0);

		//Spawn at random location
		while(true) {
			int randomX = UnityEngine.Random.Range(0, mapSize);
			int randomZ = UnityEngine.Random.Range(0, mapSize);

			float locationHeight = heightMap[randomX, randomZ];

			if (locationHeight > 0.3f && locationHeight < 0.7f) {
				float finalX = ((randomX - mapSize/2) * mapScale);
				float finalZ = (-(randomZ - mapSize/2) * mapScale);
				float finalY = heightCurve.Evaluate(locationHeight) * heightScale * mapScale;

				GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(finalX, finalY + 2, finalZ), Quaternion.identity, 0);
				player.name = PhotonNetwork.LocalPlayer.NickName;

				// Temp spawns for objects
				GameObject wood = PhotonNetwork.Instantiate("Generic Wood", new Vector3(finalX+5, finalY + 10, finalZ), Quaternion.identity, 0);
				GameObject axeObject = PhotonNetwork.Instantiate("Wood Axe", new Vector3(finalX + 3, finalY + 10, finalZ + 1), Quaternion.identity, 0);
				GameObject bucklerObject = PhotonNetwork.Instantiate("Buckler", new Vector3(finalX + 2, finalY + 10, finalZ + 2), Quaternion.identity, 0);
				GameObject ironSwordObject = PhotonNetwork.Instantiate("Iron Sword", new Vector3(finalX + -2, finalY + 10, finalZ + 2), Quaternion.identity, 0);
				GameObject treasureChestObject = PhotonNetwork.Instantiate("Treasure Chest", new Vector3(finalX + -3, finalY + 10, finalZ + 2), Quaternion.identity, 0);
				GameObject woodPickaxeObject = PhotonNetwork.Instantiate("Wood Pickaxe", new Vector3(finalX + -1, finalY + 10, finalZ + -4), Quaternion.identity, 0);
				GameObject woodSwordObject = PhotonNetwork.Instantiate("Wood Sword", new Vector3(finalX + -2, finalY + 10, finalZ + -2), Quaternion.identity, 0);
				wood.name = "Generic Wood";
				axeObject.name = "Wood Axe";
				bucklerObject.name = "Buckler";
				ironSwordObject.name = "Iron Sword";
				treasureChestObject.name = "Treasure Chest";
				woodPickaxeObject.name = "Wood Pickaxe";
				woodSwordObject.name = "Wood Sword";
				break;
			}
		}
	}
}