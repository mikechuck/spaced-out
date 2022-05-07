using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
	public MapGenerator mapGenerator;
	public GameObject playerPrefab;
	public TerrainData terrainData;
	public MapData mapData;
	public TextureData textureData;
	
    // Start is called before the first frame update
    void Start()
    {
		Debug.Log("starting");
        mapData = mapGenerator.InitiateMapGeneration();
		SpawnPlayer();
    }

	void SpawnPlayer() {
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

				PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(finalX, finalY + 2, finalZ), Quaternion.identity);

				//Random rotation for the player
				// float rotation = UnityEngine.Random.Range(0f, 360f);
				// player.transform.Rotate(0.0f, rotation, 0.0f);
				break;
			}
		}
	}
}
