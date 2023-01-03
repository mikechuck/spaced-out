using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MainSceneManager : NetworkBehaviour
{
	[SerializeField] private List<Material> skyboxOptions;
	[SerializeField] private PlanetManager planetManager;
	private SpawnPlayer spawnPlayer;
	public NetworkVariable<int> SkyboxIndex = new NetworkVariable<int>();
	
    void Start()
	{
		CreateClientConnection();
    }

	public override void OnNetworkSpawn()
	{
		StartSceneCreation();
	}

	private void CreateClientConnection()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			NetworkManager.Singleton.StartClient();
		}
	}

	private void StartSceneCreation()
	{	
		
		SetSkybox();
		planetManager.GeneratePlanet();
		// spawn launchpad
		// spawn chests
		// spawn enemies
	}

	private void SetSkybox()
	{
		if (NetworkManager.Singleton.IsServer)
		{
			System.Random r = new System.Random();
			int randomInt = r.Next(skyboxOptions.Count - 1);

			SkyboxIndex.Value = Random.Range(0, skyboxOptions.Count -1);
		}
		RenderSettings.skybox = skyboxOptions[SkyboxIndex.Value];
	}
}

