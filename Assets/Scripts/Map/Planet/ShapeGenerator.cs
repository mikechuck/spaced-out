using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShapeGenerator
{
	private float _planetRadius = 1;
	private int _numNoiseLayers;
	private NoiseFilter[] _noiseFilters;

	public ShapeGenerator()
	{
		// todo1: change to random num of noise layers
		_numNoiseLayers = 4;
		CreateNoiseFilters();
	}

	private void CreateNoiseFilters()
	{
		_noiseFilters = new NoiseFilter[_numNoiseLayers];
		for (int i = 0; i< _numNoiseLayers; i++)
		{
			_noiseFilters[i] = new NoiseFilter();
		}
	}

	public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
	{
		float firstLayerValue = 0;
		float elevation = 0;

		firstLayerValue = _noiseFilters[0].Evaluate(pointOnUnitSphere);
		elevation = firstLayerValue;

		for (int i = 1; i < _numNoiseLayers; i++)
		{
			elevation += _noiseFilters[i].Evaluate(pointOnUnitSphere) * firstLayerValue;
		}
		return pointOnUnitSphere * _planetRadius * (1 + elevation);
	}
}
