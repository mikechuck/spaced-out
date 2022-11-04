using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class MapGenerator : MonoBehaviour
{
	public enum DrawMode {
		NoiseMode,
		Mesh,
		FalloffMap
	}
	public DrawMode drawMode;
	public const int mapChunkSize = 241;
	[Range(0,6)]
	public int editorPreviewLOD = 0;	
	public bool autoUpdate;
	float[,] falloffMap;
	public Material terrainMaterial;
	private GameManager gameManager;
	private float heightCenterOfMap;
	private MapData mapData;
	// Map objects
	public GameObject objectsParent;
	public GameObject launchpad;
	// public GameObject rocket;
	public GameObject launchControls;

	// Data objects
	public TerrainData terrainData;
	public NoiseData noiseData;
	public TextureData textureData;

	public MapData InitiateMapGeneration() {
		falloffMap = FalloffGenerator.GenerateFalloffMap(mapChunkSize);
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

		// Use saved seed, else randomize it
		if (gameManager.levelSeed.Length > 0) {
			noiseData.seed = gameManager.levelSeed;
		} else {
			RandomizeSeed();
		}

		mapData = GenerateMapData(Vector2.zero);
		MapDisplay display = FindObjectOfType<MapDisplay>();

		display.DrawMesh(MeshGenerator.GenerateTerrainMesh(mapData.heightMap, terrainData.meshHeightMultiplier, terrainData.meshHeightCurve, editorPreviewLOD));
		textureData.ApplyToMaterial(terrainMaterial);

		SpawnMapScenery();
		return mapData;
	}

	void OnValuesUpdated() {
		if (!Application.isPlaying) {
			DrawMapInEditor();
		}
	}

	void OnTextureValuesUpdated() {
		textureData.ApplyToMaterial(terrainMaterial);
	}

	public void DrawMapInEditor() {
		mapData = GenerateMapData(Vector2.zero);
		MapDisplay display = FindObjectOfType<MapDisplay>();
		if (drawMode == DrawMode.NoiseMode) {
			display.DrawTexture(TextureGenerator.TextureFromHeightMap(mapData.heightMap));
		} else if (drawMode == DrawMode.Mesh) {
			display.DrawMesh(MeshGenerator.GenerateTerrainMesh(mapData.heightMap, terrainData.meshHeightMultiplier, terrainData.meshHeightCurve, editorPreviewLOD));
		} else if (drawMode == DrawMode.FalloffMap) {
			display.DrawTexture(TextureGenerator.TextureFromHeightMap(FalloffGenerator.GenerateFalloffMap(mapChunkSize)));
		}

		TreeSpawner treeSpawner = FindObjectOfType<TreeSpawner>();
		treeSpawner.SpawnTrees(mapData, terrainData, textureData, heightCenterOfMap, mapChunkSize);
		textureData.ApplyToMaterial(terrainMaterial);
	}

	public void RandomizeSeed() {
		int randomSeed = UnityEngine.Random.Range(0, 100000);
		noiseData.seed = randomSeed.ToString();
	}

	MapData GenerateMapData(Vector2 center) {
		float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, noiseData.seed, noiseData.noiseScale, noiseData.octaves, noiseData.persistence, noiseData.lacunarity, center + noiseData.offset, noiseData.normalizeMode);

		Color[] colorMap = new Color[mapChunkSize * mapChunkSize];
		int halfMapChunkSize = mapChunkSize / 2;
		int centerCirleSize = 5;

		heightCenterOfMap = Mathf.Clamp01(noiseMap[halfMapChunkSize, halfMapChunkSize] - falloffMap[halfMapChunkSize, halfMapChunkSize]);
		for (int y = 0; y< mapChunkSize; y++) {
			for (int x = 0; x < mapChunkSize; x++) {
				if (terrainData.useFalloff) {
					float heightValue = Mathf.Clamp01(noiseMap[x,y] - falloffMap[x,y]);
					bool yIsInRange = y >= halfMapChunkSize - centerCirleSize && y <= halfMapChunkSize + centerCirleSize;
					bool xIsInRange = x >= halfMapChunkSize - centerCirleSize && x <= halfMapChunkSize + centerCirleSize;
					int xMiddle = halfMapChunkSize - x;
					int yMiddle = halfMapChunkSize - y;
					bool isInRange = Math.Sqrt(Math.Pow(xMiddle, 2) + Math.Pow(yMiddle, 2)) <= 5;
					if (isInRange) {
						heightValue = heightCenterOfMap;
					}
					noiseMap[x,y] = heightValue;
				}
			}
		}

		textureData.UpdateMeshHeights(terrainMaterial, terrainData.minHeight, terrainData.maxHeight);

		return new MapData(noiseMap);
	}

	// Built in method that is called whenever variable is changed inside inspector
	void OnValidate() {
		if (terrainData != null) {
			terrainData.OnValuesUpdated -= OnValuesUpdated;
			terrainData.OnValuesUpdated += OnValuesUpdated;
		}
		if (noiseData != null) {
			noiseData.OnValuesUpdated -= OnValuesUpdated;
			noiseData.OnValuesUpdated += OnValuesUpdated;
		}
		if (textureData != null) {
			textureData.OnValuesUpdated -= OnTextureValuesUpdated;
			textureData.OnValuesUpdated += OnTextureValuesUpdated;
		}
		falloffMap = FalloffGenerator.GenerateFalloffMap(mapChunkSize);
	}

	private void SpawnMapScenery() {
		float scaledHeight = terrainData.meshHeightCurve.Evaluate(heightCenterOfMap) * terrainData.meshHeightMultiplier * terrainData.uniformScale;

		// Trees
		TreeSpawner treeSpawner = FindObjectOfType<TreeSpawner>();
		treeSpawner.SpawnTrees(mapData, terrainData, textureData, heightCenterOfMap, mapChunkSize);
		// Launchpad
		GameObject launchpadInstance = Instantiate(launchpad, new Vector3(0, scaledHeight, 0), Quaternion.identity);
		launchpadInstance.transform.SetParent(objectsParent.transform);
		// Rocket
		// GameObject rocketInstance = Instantiate(rocket, new Vector3(0, scaledHeight + 2, 0), Quaternion.identity);
		// rocketInstance.transform.SetParent(objectsParent.transform);
		// Launch Controls
		GameObject launchControlsInstance = Instantiate(launchControls, new Vector3(0f, scaledHeight + 2.5f, -147f), Quaternion.identity);
		launchControlsInstance.transform.SetParent(objectsParent.transform);
	}
}


public struct MapData {
	public readonly float[,] heightMap;

	public MapData(float[,] heightMap) {

		this.heightMap = heightMap;
	}
}