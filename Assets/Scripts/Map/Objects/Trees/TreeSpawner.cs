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
	private int mapSize;
	private float[,] heightMap;
	AnimationCurve heightCurve;
	float heightScale;
	float mapScale;
	GameObject treesParent;

	public float noiseScale = 50f;
	public float lacunarity = 5f;
	public int octaves = 5;
	public float persistence = 0.5f;

	// public void SpawnTrees(MapData mapData, TerrainData terrainData, TextureData textureData, float heightCenterOfMap, int mapChunkSize) {

	// 	mapScale = terrainData.uniformScale;
	// 	heightScale = terrainData.meshHeightMultiplier;
	// 	heightCurve = new AnimationCurve(terrainData.meshHeightCurve.keys);
	// 	heightMap = mapData.heightMap;
	// 	mapSize = heightMap.GetLength(0);
	// 	usedTreePositions = new bool[mapSize, mapSize];
	// 	treesParent = GameObject.Find("Trees");

	// 	//Destroy trees first
	// 	GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");
	// 	foreach(GameObject tree in trees) {
	// 		DestroyImmediate(tree);
	// 	}

	// 	// Create a new noise map to creating groupings for trees
	// 	int randomSeed1 = UnityEngine.Random.Range(0, 10000);
	// 	int randomSeed2 = UnityEngine.Random.Range(0, 10000);
	// 	int randomSeed3 = UnityEngine.Random.Range(0, 10000);
	// 	randomSeed1 = 100;
	// 	randomSeed2 = 200;
	// 	randomSeed3 = 300;
	// 	noiseScale = 50f;
	// 	lacunarity = 5f;
	// 	octaves = 5;
	// 	persistence = 0.5f;
	// 	Vector2 offset = new Vector2(0, 0);
	// 	Noise.NormalizeMode normalizeMode = Noise.NormalizeMode.Local;

	// 	float[,] noiseMap1 = Noise.GenerateNoiseMap(mapSize, mapSize, randomSeed1.ToString(), noiseScale, octaves, persistence, lacunarity, offset, normalizeMode);
	// 	float[,] noiseMap2 = Noise.GenerateNoiseMap(mapSize, mapSize, randomSeed2.ToString(), noiseScale, octaves, persistence, lacunarity, offset, normalizeMode);
	// 	float[,] noiseMap3 = Noise.GenerateNoiseMap(mapSize, mapSize, randomSeed3.ToString(), noiseScale, octaves, persistence, lacunarity, offset, normalizeMode);

	// 	//Set treee spawn bounds
	// 	float lowerBound1 = 0.25f;
	// 	float upperBound1 = 0.51f;

	// 	float lowerBound2 = 0.49f;
	// 	float upperBound2 = 0.65f;

	// 	float lowerBound3 = 0.61f;
	// 	float upperBound3 = 0.9f;

	// 	SpawnTreeType(numberOfTrees1, tree1, lowerBound1, upperBound1, noiseMap1, heightCenterOfMap, mapChunkSize);
	// 	SpawnTreeType(numberOfTrees2, tree2, lowerBound2, upperBound2, noiseMap2, heightCenterOfMap, mapChunkSize);
	// 	SpawnTreeType(numberOfTrees3, tree3, lowerBound3, upperBound3, noiseMap3, heightCenterOfMap, mapChunkSize);
			
	// }

	// private void SpawnTreeType(int numberOfTrees, GameObject treeObject, float lowerBound, float upperBound, float[,] noiseMap, float heightCenterOfMap, int mapChunkSize){
	// 	// Spawn tree 1
	// 	int x = 0;
	// 	while(x < numberOfTrees) {
	// 		int randomX = UnityEngine.Random.Range(0, mapSize);
	// 		int randomZ = UnityEngine.Random.Range(0, mapSize);
	// 		float randomScale = UnityEngine.Random.Range(90f, 110f) / 100f;
	// 		int halfMapChunkSize = mapChunkSize / 2;

	// 		float randomOffset = UnityEngine.Random.Range(0, 10);
	// 		float noiseValue = noiseMap[randomX, randomZ];
 
	// 		if (noiseValue > 0.45f) {
	// 			float locationHeight = heightMap[randomX, randomZ];

	// 			if (locationHeight > lowerBound && locationHeight < upperBound) {
	// 				// Don't spawn trees in launchpad area
	// 				bool zIsInLaunchZone = randomZ >= halfMapChunkSize - 5 && randomZ <= halfMapChunkSize + 5;
	// 				bool xIsInLaunchZone = randomX >= halfMapChunkSize - 5 && randomX <= halfMapChunkSize + 5;
	// 				if (zIsInLaunchZone && xIsInLaunchZone) {
	// 					break;
	// 				}
	// 				// Only spawn tree if position is free to be used
	// 				if (!usedTreePositions[randomX, randomZ]) {
	// 					usedTreePositions[randomX, randomZ] = true;
	// 					float finalX = ((randomX - mapSize/2) * mapScale) + randomOffset;
	// 					float finalY = heightCurve.Evaluate(locationHeight) * heightScale * mapScale;
	// 					float finalZ = (-(randomZ - mapSize/2) * mapScale) + randomOffset;

	// 					GameObject tree = Instantiate(treeObject, new Vector3(finalX, finalY - 3, finalZ), Quaternion.identity);
	// 					tree.transform.SetParent(treesParent.transform);
	// 					tree.tag = "Tree";
	// 					tree.name = treeObject.name;
	// 					tree.transform.localScale = tree.transform.localScale * randomScale * 1.7f;
	// 					//Random rotation for each tree
	// 					var euler = tree.transform.eulerAngles;
	// 					euler.y = Random.Range(0.0f, 360.0f);
	// 					tree.transform.eulerAngles = euler;
	// 				}
	// 			}
	// 			x++;
	// 		}
	// 	}
	// }
}
