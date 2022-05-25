using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
	public MapGenerator mapGenerator;
	private SpawnPlayer spawnPlayer;
	public MapData mapData;
	
    // Start is called before the first frame update
    void Start()
    {
		try {
			spawnPlayer = GetComponent<SpawnPlayer>();

			mapData = mapGenerator.InitiateMapGeneration();
			spawnPlayer.Spawn(mapData);
		} catch (Exception e) {
			Debug.LogError(e);
			PhotonNetwork.LoadLevel("Loading");
		}
    }
}
