using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MainSceneManager : MonoBehaviour
{
	[SerializeField]
	private List<Material> skyboxOptions;
	[SerializeField]
	private PlanetManager planetManager;
	private SpawnPlayer spawnPlayer;
	
    void Start()
	{
		EnableServerDebugging();
		spawnPlayer = gameObject.GetComponent<SpawnPlayer>();
		StartSceneCreation();
    }

	private void EnableServerDebugging()
	{
		if (NetworkManager.Singleton.IsServer) 
		{
			gameObject.GetComponent<DebugStuff>().enabled = true;
		}
	}

	private void StartSceneCreation()
	{	
		if (NetworkManager.Singleton.IsServer)
		{
			SetSkybox();
			planetManager.GeneratePlanet();
			// spawn launchpad
			// spawn chests
			// spawn enemies
		}
		spawnPlayer.Spawn();
		
	}

	private void SetSkybox()
	{
		System.Random r = new System.Random();
		int randomInt = r.Next(skyboxOptions.Count - 1);
		RenderSettings.skybox = skyboxOptions[randomInt];
	}
}

