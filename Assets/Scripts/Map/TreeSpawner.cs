using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner: MonoBehaviour
{
    public int numberOfTrees;
	public GameObject tree1;
	public GameObject tree2;
	public GameObject tree3;

	public void SpawnTrees(MapData mapData, TerrainData terrainData, TextureData textureData) {
		float mapScale = terrainData.uniformScale;
		float heightScale = terrainData.meshHeightMultiplier;
		AnimationCurve heightCurve = new AnimationCurve(terrainData.meshHeightCurve.keys);
		float[,] heightMap = mapData.heightMap;
		int mapSize = heightMap.GetLength(0);
		
		GameObject treesParent = GameObject.Find("Trees");

		//Destroy trees first
		GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");
		foreach(GameObject tree in trees) {
			DestroyImmediate(tree);
		}

		// Create a new noise map to creating groupings for trees
		int randomSeed = UnityEngine.Random.Range(0, 10000);
		float noiseScale = 50f;
		float lacunarity = 5f;
		int octaves = 5;
		float persistence = 0.5f;
		Vector2 offset = new Vector2(0, 0);
		Noise.NormalizeMode normalizeMode = Noise.NormalizeMode.Local;

		float[,] noiseMap = Noise.GenerateNoiseMap(
			mapSize, 
			mapSize, 
			randomSeed, 
			noiseScale, 
			octaves, 
			persistence,
			lacunarity,
			offset,
			normalizeMode
		);


		//Set treee spawn bounds
		float lowerBound = 0.3f;
		float upperBound = 0.7f;

		int x = 0;
		while(x < numberOfTrees) {
			int randomX = UnityEngine.Random.Range(0, mapSize);
			int randomZ = UnityEngine.Random.Range(0, mapSize);

			float randomOffset = UnityEngine.Random.Range(0, 10);
			float noiseValue = noiseMap[randomX, randomZ];

			if (noiseValue > 0.55f) {
				float locationHeight = heightMap[randomX, randomZ];

				if (locationHeight > lowerBound && locationHeight < upperBound) {
					float finalX = ((randomX - mapSize/2) * mapScale) + randomOffset;
					float finalZ = (-(randomZ - mapSize/2) * mapScale) + randomOffset;
					float finalY = heightCurve.Evaluate(locationHeight) * heightScale * mapScale;

					GameObject tree = Instantiate(tree1, new Vector3(finalX, finalY - 3, finalZ), Quaternion.identity);
					tree.transform.SetParent(treesParent.transform);
					tree.tag = "Tree";
					tree.transform.localScale = new Vector3(5, 5, 5);
					//Random rotation for each tree
					var euler = tree.transform.eulerAngles;
					euler.y = Random.Range(0.0f, .0f);
					tree.transform.eulerAngles = euler;
				}
				x++;
			}
		}
	}
}
