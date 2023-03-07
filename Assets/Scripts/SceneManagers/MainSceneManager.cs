using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MainSceneManager : NetworkBehaviour
{
	[SerializeField] private List<Material> skyboxOptions;
	[SerializeField] private PlanetManager planetManager;
	private SpawnPlayer spawnPlayer;
	private bool _sceneCreated = false;
	public NetworkVariable<int> SkyboxIndex = new NetworkVariable<int>();
	
	void OnEnable()
	{
		Debug.Log("main scene manager enabled");
	}

	void Awake()
	{
		Debug.Log("main scene manager awake");
	}

    void Start()
	{
		Debug.Log("main scene manager start");
		// StartSceneCreation();
    }

	public override void OnNetworkSpawn()
	{
		Debug.Log("main scene manager onnetworkspawn");
		base.OnNetworkSpawn();
	}

	private void StartSceneCreation()
	{	
		Debug.Log("starting scene creation");
		// SetSkybox();
		// planetManager.GeneratePlanet();
		// spawn launchpad
		// spawn chests
		// spawn enemies
	}

	private void SetSkybox()
	{
		Debug.Log("setting skybox");
		if (NetworkManager.Singleton.IsServer)
		{
			System.Random r = new System.Random();
			int randomInt = r.Next(skyboxOptions.Count - 1);

			SkyboxIndex.Value = Random.Range(0, skyboxOptions.Count -1);
			Debug.Log(SkyboxIndex.Value);
		}
		Debug.Log(SkyboxIndex.Value);
		RenderSettings.skybox = skyboxOptions[SkyboxIndex.Value];
	}
}

