using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NoiseFilter
{
	private float _strength;
	private int _numLayers;
	private float _baseRoughness;
	private float _roughness;
	private float _persistence;
	private Vector3 _center;
	private float _minValue;
	private Noise _noise = new Noise();

	public NoiseFilter()
	{
		this._strength = Random.Range(0.1f, 0.1f);
		this._numLayers = Random.Range(5, 6);
		this._baseRoughness = Random.Range(1f, 1.25f);
		this._roughness = Random.Range(2f, 2.4f);
		this._persistence = Random.Range(0.4f, 0.5f);
		this._center = new Vector3(
			Random.Range(0, 10),
			Random.Range(0, 10),
			Random.Range(0, 10)
		);
		this._minValue = Random.Range(0.5f, 1f);
	}

	public float Evaluate(Vector3 point)
	{
		float noiseValue = 0;
		float frequency = _baseRoughness;
		float amplitude = 1;

		for (int i = 0; i < _numLayers; i++)
		{
			float v = _noise.Evaluate(point * frequency + _center);
			noiseValue += (v + 1) * 0.5f * amplitude;
			frequency *= _roughness;
			amplitude *= _persistence;
		}
		
		noiseValue = Mathf.Max(0, noiseValue - _minValue);
		return noiseValue * _strength;
	}
}