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
				GameObject wood = PhotonNetwork.Instantiate(genericWood.name, new Vector3(finalX+5, finalY + 10, finalZ), Quaternion.identity, 0);
				GameObject axeObject = PhotonNetwork.Instantiate(axe.name, new Vector3(finalX + 3, finalY + 10, finalZ + 1), Quaternion.identity, 0);
				player.name = PhotonNetwork.LocalPlayer.NickName;
				wood.name = "Generic Wood";
				axeObject.name = "Wood Axe";
				break;
			}
		}
	}
}