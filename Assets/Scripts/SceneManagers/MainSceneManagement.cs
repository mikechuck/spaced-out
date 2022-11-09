using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class MainSceneManagement : MonoBehaviour
{
	private SpawnPlayer spawnPlayer;
	
    // Start is called before the first frame update
    void Start() {
		StartSceneCreation();
    }

	private void StartSceneCreation() {
		Debug.Log("Creating planet");
		// call function to create planet
		// spawn launchpad
		// spawn players
		// spawn chests
		// spawn enemies
	}
}

