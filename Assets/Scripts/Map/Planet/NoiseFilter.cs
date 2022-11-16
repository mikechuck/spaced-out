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
		// todo1: change these to have random values within a range
		this._strength = 1;
		this._numLayers = 1;
		this._baseRoughness = 1f;
		this._roughness = 2f;
		this._persistence = 0.5f;
		this._center = new Vector3(0, 1, 2);
		this._minValue = 0.05;
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