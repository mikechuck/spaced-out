using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
	public MapGenerator mapGenerator;
	private SpawnPlayer spawnPlayer;
	public MapData mapData;
	
    // Start is called before the first frame update
    void Start()
    {
		spawnPlayer = GetComponent<SpawnPlayer>();

        mapData = mapGenerator.InitiateMapGeneration();
		spawnPlayer.Spawn(mapData);
    }
}
