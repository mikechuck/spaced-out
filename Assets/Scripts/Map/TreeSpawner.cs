using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner: MonoBehaviour
{
    public int numberOfTrees1;
    public int numberOfTrees2;
    public int numberOfTrees3;
	public GameObject tree1;
	public GameObject tree2;
	public GameObject tree3;
	private bool[,] usedTreePositions;

	public void SpawnTrees(MapData mapData, TerrainData terrainData, TextureData textureData) {

		float mapScale = terrainData.uniformScale;
		float heightScale = terrainData.meshHeightMultiplier;
		AnimationCurve heightCurve = new AnimationCurve(terrainData.meshHeightCurve.keys);
		float[,] heightMap = mapData.heightMap;
		int mapSize = heightMap.GetLength(0);
		usedTreePositions = new bool[mapSize, mapSize];
		
		GameObject treesParent = GameObject.Find("Trees");

		//Destroy trees first
		GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");
		foreach(GameObject tree in trees) {
			DestroyImmediate(tree);
		}

		// Create a new noise map to creating groupings for trees
		int randomSeed1 = UnityEngine.Random.Range(0, 10000);
		int randomSeed2 = UnityEngine.Random.Range(0, 10000);
		int randomSeed3 = UnityEngine.Random.Range(0, 10000);
		float noiseScale = 50f;
		float lacunarity = 5f;
		int octaves = 5;
		float persistence = 0.5f;
		Vector2 offset = new Vector2(0, 0);
		Noise.NormalizeMode normalizeMode = Noise.NormalizeMode.Local;

		float[,] noiseMap1 = Noise.GenerateNoiseMap(mapSize, mapSize, randomSeed1.ToString(), noiseScale, octaves, persistence, lacunarity, offset, normalizeMode);
		float[,] noiseMap2 = Noise.GenerateNoiseMap(mapSize, mapSize, randomSeed2.ToString(), noiseScale, octaves, persistence, lacunarity, offset, normalizeMode);
		float[,] noiseMap3 = Noise.GenerateNoiseMap(mapSize, mapSize, randomSeed3.ToString(), noiseScale, octaves, persistence, lacunarity, offset, normalizeMode);

		//Set treee spawn bounds
		float lowerBound1 = 0.3f;
		float upperBound1 = 0.5f;

		float lowerBound2 = 0.5f;
		float upperBound2 = 0.7f;

		float lowerBound3 = 0.7f;
		float upperBound3 = 0.9f;

		// Spawn tree 1
		int x = 0;
		while(x < numberOfTrees1) {
			int randomX = UnityEngine.Random.Range(0, mapSize);
			int randomZ = UnityEngine.Random.Range(0, mapSize);

			float randomOffset = UnityEngine.Random.Range(0, 10);
			float noiseValue = noiseMap1[randomX, randomZ];

			if (noiseValue > 0.6f) {
				float locationHeight = heightMap[randomX, randomZ];

				if (locationHeight > lowerBound1 && locationHeight < upperBound1) {
					// Only spawn tree is position is free to be used
					if (!usedTreePositions[randomX, randomZ]) {
						usedTreePositions[randomX, randomZ] = true;
						float finalX = ((randomX - mapSize/2) * mapScale) + randomOffset;
						float finalZ = (-(randomZ - mapSize/2) * mapScale) + randomOffset;
						float finalY = heightCurve.Evaluate(locationHeight) * heightScale * mapScale;

						GameObject tree = Instantiate(tree1, new Vector3(finalX, finalY - 3, finalZ), Quaternion.identity);
						tree.transform.SetParent(treesParent.transform);
						tree.tag = "Tree";
						//Random rotation for each tree
						var euler = tree.transform.eulerAngles;
						euler.y = Random.Range(0.0f, 360.0f);
						tree.transform.eulerAngles = euler;
					}
				}
				x++;
			}
		}

		// Spawn tree 2
		x = 0;
		while(x < numberOfTrees2) {
			int randomX = UnityEngine.Random.Range(0, mapSize);
			int randomZ = UnityEngine.Random.Range(0, mapSize);

			float randomOffset = UnityEngine.Random.Range(0, 10);
			float noiseValue = noiseMap2[randomX, randomZ];

			if (noiseValue > 0.6f) {
				float locationHeight = heightMap[randomX, randomZ];

				if (locationHeight > lowerBound2 && locationHeight < upperBound2) {

					// Only spawn tree is position is free to be used
					if (!usedTreePositions[randomX, randomZ]) {
						usedTreePositions[randomX, randomZ] = true;
						float finalX = ((randomX - mapSize/2) * mapScale) + randomOffset;
						float finalZ = (-(randomZ - mapSize/2) * mapScale) + randomOffset;
						float finalY = heightCurve.Evaluate(locationHeight) * heightScale * mapScale;

						GameObject tree = Instantiate(tree2, new Vector3(finalX, finalY - 3, finalZ), Quaternion.identity);
						tree.transform.SetParent(treesParent.transform);
						tree.tag = "Tree";
						tree.transform.localScale = new Vector3(7, 7, 7);
						//Random rotation for each tree
						var euler = tree.transform.eulerAngles;
						euler.y = Random.Range(0.0f, 360.0f);
						tree.transform.eulerAngles = euler;
					}
				}
				x++;
			}
		}

		// Spawn tree 3
		x = 0;
		while(x < numberOfTrees3) {
			int randomX = UnityEngine.Random.Range(0, mapSize);
			int randomZ = UnityEngine.Random.Range(0, mapSize);

			float randomOffset = UnityEngine.Random.Range(0, 10);
			float noiseValue = noiseMap3[randomX, randomZ];

			if (noiseValue > 0.6f) {
				float locationHeight = heightMap[randomX, randomZ];

				if (locationHeight > lowerBound3 && locationHeight < upperBound3) {
					// Only spawn tree is position is free to be used
					if (!usedTreePositions[randomX, randomZ]) {
						usedTreePositions[randomX, randomZ] = true;
						float finalX = ((randomX - mapSize/2) * mapScale) + randomOffset;
						float finalZ = (-(randomZ - mapSize/2) * mapScale) + randomOffset;
						float finalY = heightCurve.Evaluate(locationHeight) * heightScale * mapScale;

						GameObject tree = Instantiate(tree3, new Vector3(finalX, finalY - 3, finalZ), Quaternion.identity);
						tree.transform.SetParent(treesParent.transform);
						tree.tag = "Tree";
						tree.transform.localScale = new Vector3(8, 8, 8);
						//Random rotation for each tree
						var euler = tree.transform.eulerAngles;
						euler.y = Random.Range(0.0f, 360.0f);
						tree.transform.eulerAngles = euler;
					}
				}
				x++;
			}
		}
	}
}
