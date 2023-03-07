using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class LoadingSceneManager : NetworkBehaviour
{
	public override void OnNetworkSpawn()
	{
		if (IsServer)
		{
			NetworkManager.SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
		}
	}
}

