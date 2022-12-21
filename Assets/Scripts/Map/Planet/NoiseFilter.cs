using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NoiseFilter
{
	private Noise _noise = new Noise();
	public NetworkVariable<float> Strength = new NetworkVariable<float>();
	public NetworkVariable<int> NumLayers = new NetworkVariable<int>();
	public NetworkVariable<float> BaseRoughness = new NetworkVariable<float>();
	public NetworkVariable<float> Roughness = new NetworkVariable<float>();
	public NetworkVariable<float> Persistence = new NetworkVariable<float>();
	public NetworkVariable<Vector3> Center = new NetworkVariable<Vector3>();
	public NetworkVariable<float> MinValue = new NetworkVariable<float>();


	public NoiseFilter()
	{
		if (NetworkManager.Singleton.IsServer)
		{
			Strength.Value = Random.Range(0.1f, 0.1f);
			NumLayers.Value = Random.Range(5, 6);
			BaseRoughness.Value = Random.Range(1f, 1.25f);
			Roughness.Value = Random.Range(2f, 2.4f);
			Persistence.Value = Random.Range(0.4f, 0.5f);
			Center.Value = new Vector3(
				Random.Range(0, 10),
				Random.Range(0, 10),
				Random.Range(0, 10)
			);
			MinValue.Value = Random.Range(0.5f, 1f);
		}
	}

	public float Evaluate(Vector3 point)
	{
		float noiseValue = 0;
		float frequency = BaseRoughness.Value;
		float amplitude = 1;

		for (int i = 0; i < NumLayers.Value; i++)
		{
			float v = _noise.Evaluate(point * frequency + Center.Value);
			noiseValue += (v + 1) * 0.5f * amplitude;
			frequency *= Roughness.Value;
		}
		
		noiseValue = Mathf.Max(0, noiseValue - MinValue.Value);
		return noiseValue * Strength.Value;
	}
}