using Unity.Netcode;
using UnityEngine;
using System;
using CustomSingletons;

public class PlayersManager : Singleton<PlayersManager>
{
    private NetworkVariable<int> _playersInGame = new NetworkVariable<int>();
	// TODO: add new netvar to keep track of player data as well (name, status for now)

    public int PlayersInGame
    {
        get
        {
            return _playersInGame.Value;
        }
    }

    void Awake() {
		DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
		NetworkManager.Singleton.OnServerStarted += () =>
		{
			if (IsHost)
			{
				Debug.Log("server started test");
				// ToastService.Instance.DisplayToast("Server started test etset tse testet");
			}
			else
			{
				Debug.Log("client test");
			}
		};

        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
			Debug.Log("on connect");
            if (IsServer)
            {
                Debug.Log($"Player {id} connected.");
                _playersInGame.Value++;
            }
        };

        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
			Debug.Log("on disconnect");
            if (IsServer)
            {
                Debug.Log($"Player {id} disconnected.");
                _playersInGame.Value--;
            }
			else
			{
				Debug.Log("Can't connect to host");
			}
        };
    }
}