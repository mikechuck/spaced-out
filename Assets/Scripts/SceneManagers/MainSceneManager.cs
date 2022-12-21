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

	#region UI
	void OnGUI()
	{
		GUILayout.BeginArea(new Rect(10, 10, 300, 300));
		StatusLabels();
		GUILayout.EndArea();
	}

	static void StatusLabels()
	{
		var mode = NetworkManager.Singleton.IsHost ? "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";
		GUILayout.Label("Transport: " + NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
		GUILayout.Label("Mode: " + mode);
	}
	#endregion
	
    void Start()
	{
		CreateClientConnection();
		EnableServerDebugging();
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

	private void EnableServerDebugging()
	{
		if (NetworkManager.Singleton.IsServer) 
		{
			gameObject.GetComponent<DebugStuff>().enabled = true;
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

