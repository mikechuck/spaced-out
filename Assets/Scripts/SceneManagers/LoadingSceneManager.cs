using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class LoadingSceneManager : MonoBehaviour
{	
    void Start()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			NetworkManager.Singleton.StartClient();
		}
		SceneManager.LoadScene("Lobby");
    }
}

