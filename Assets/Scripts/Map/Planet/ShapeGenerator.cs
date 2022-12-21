using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ShapeGenerator
{
	private int _numNoiseLayers;
	private NoiseFilter[] _noiseFilters;
	public NetworkVariable<int> PlanetRadius = new NetworkVariable<int>();


	public ShapeGenerator()
	{
		if (NetworkManager.Singleton.IsServer)
		{
			this.PlanetRadius.Value = Random.Range(550, 850);
		}
		_numNoiseLayers = Random.Range(3, 5);
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
		return pointOnUnitSphere * PlanetRadius.Value * (1 + elevation);
	}
}
