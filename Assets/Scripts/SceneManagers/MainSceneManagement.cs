using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class MainSceneManagement : MonoBehaviour
{
	public List<Material> skyboxOptions;
	public PlanetManager planetManager;
	
    void Start()
	{
		SetSkybox();
		StartSceneCreation();
    }

	private void StartSceneCreation()
	{
		// call function to create planet
		planetManager.GeneratePlanet();
		// spawn launchpad
		// spawn players
		// spawn chests
		// spawn enemies
	}

	private void SetSkybox()
	{
		System.Random r = new System.Random();
		int randomInt = r.Next(skyboxOptions.Count - 1);
		RenderSettings.skybox = skyboxOptions[randomInt];
	}
}

