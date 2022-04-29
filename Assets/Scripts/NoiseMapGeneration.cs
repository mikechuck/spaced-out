using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMapGeneration : MonoBehaviour
{
	public float[,] GenerateNoiseMap(int mapDepth, int mapWidth, float scale) {
		float [,] noiseMap = new float[mapDepth, mapWidth];

		for (int zIndex = 0; zIndex < mapDepth; zIndex++) {
			for (int xIndex = 0; xIndex < mapWidth; xIndex++) {
				float sampleX = xIndex / scale;
				float sampleZ = zIndex / scale;

				float noise = Mathf.PerlinNoise(sampleX, sampleZ);

				noiseMap[zIndex, xIndex] = noise;
			}
		}

		return noiseMap;
	}
}
