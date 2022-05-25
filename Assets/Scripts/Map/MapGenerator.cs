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

	// Data objects
	public TerrainData terrainData;
	public NoiseData noiseData;
	public TextureData textureData;

	public MapData InitiateMapGeneration() {
		falloffMap = FalloffGenerator.GenerateFalloffMap(mapChunkSize);

		// Use saved seed, else randomize it
		if (DataManager.levelSeed.Length > 0) {
			noiseData.seed = DataManager.levelSeed;
		} else {
			RandomizeSeed();
		}
	

		MapData mapData = GenerateMapData(Vector2.zero);
		MapDisplay display = FindObjectOfType<MapDisplay>();

		display.DrawMesh(MeshGenerator.GenerateTerrainMesh(mapData.heightMap, terrainData.meshHeightMultiplier, terrainData.meshHeightCurve, editorPreviewLOD));
		textureData.ApplyToMaterial(terrainMaterial);

		TreeSpawner treeSpawner = FindObjectOfType<TreeSpawner>();
		treeSpawner.SpawnTrees(mapData, terrainData, textureData);
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
		MapData mapData = GenerateMapData(Vector2.zero);
		MapDisplay display = FindObjectOfType<MapDisplay>();
		if (drawMode == DrawMode.NoiseMode) {
			display.DrawTexture(TextureGenerator.TextureFromHeightMap(mapData.heightMap));
		} else if (drawMode == DrawMode.Mesh) {
			display.DrawMesh(MeshGenerator.GenerateTerrainMesh(mapData.heightMap, terrainData.meshHeightMultiplier, terrainData.meshHeightCurve, editorPreviewLOD));
		} else if (drawMode == DrawMode.FalloffMap) {
			display.DrawTexture(TextureGenerator.TextureFromHeightMap(FalloffGenerator.GenerateFalloffMap(mapChunkSize)));
		}

		textureData.ApplyToMaterial(terrainMaterial);

		TreeSpawner treeSpawner = FindObjectOfType<TreeSpawner>();
		treeSpawner.SpawnTrees(mapData, terrainData, textureData);
	}

	public void RandomizeSeed() {
		int randomSeed = UnityEngine.Random.Range(0, 10000);
		noiseData.seed = randomSeed.ToString();
	}

	MapData GenerateMapData(Vector2 center) {
		float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, noiseData.seed, noiseData.noiseScale, noiseData.octaves, noiseData.persistence, noiseData.lacunarity, center + noiseData.offset, noiseData.normalizeMode);

		Color[] colorMap = new Color[mapChunkSize * mapChunkSize];
		for (int y = 0; y< mapChunkSize; y++) {
			for (int x = 0; x < mapChunkSize; x++) {
				if (terrainData.useFalloff) {
					noiseMap[x,y] = Mathf.Clamp01(noiseMap[x,y] - falloffMap[x,y]);
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
}


public struct MapData {
	public readonly float[,] heightMap;

	public MapData(float[,] heightMap) {

		this.heightMap = heightMap;
	}
}