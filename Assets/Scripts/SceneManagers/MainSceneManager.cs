using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MainSceneManager : MonoBehaviour
{
	
	[SerializeField] private List<Material> skyboxOptions;
	[SerializeField] private PlanetManager planetManager;
	private SpawnPlayer spawnPlayer;

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
		if (NetworkManager.Singleton.IsServer)
		{
			SetSkybox();
			planetManager.GeneratePlanet();
			// spawn launchpad
			// spawn chests
			// spawn enemies
		}
	}

	private void SetSkybox()
	{
		System.Random r = new System.Random();
		int randomInt = r.Next(skyboxOptions.Count - 1);
		RenderSettings.skybox = skyboxOptions[randomInt];
		// todo: skyboxes are not matching up between client and server,
		// need to set network variable with skybox index and use it on client side
	}
}

