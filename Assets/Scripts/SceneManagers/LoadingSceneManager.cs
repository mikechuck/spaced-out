using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class LoadingSceneManager : MonoBehaviour
{	
    void Start()
	{
		// if (!NetworkManager.Singleton.IsServer)
		// {
		// 	Debug.Log("starting client");
		// 	// NetworkManager.Singleton.StartClient();
		// }
		// Debug.Log("loading lobby");
		SceneManager.LoadScene("Lobby");
    }
}

