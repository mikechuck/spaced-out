using Unity.Netcode;
using UnityEngine;
using System;
using CustomSingletons;

public class PlayersManager : Singleton<PlayersManager>
{
    private NetworkVariable<int> _playersInGame = new NetworkVariable<int>();
	// add new netvar to keep track of player data as well (name, status for now)

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
		// LEFTOFF: continue player management work, then display in lobby when it loads
		NetworkManager.Singleton.OnServerStarted += () =>
		{
			if (IsHost)
			{
				Debug.Log("server started test");
			}
			else
			{
				Debug.Log("client test");
			}
		};

        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            if (IsServer)
            {
                Debug.Log($"Player {id} connected.");
                _playersInGame.Value++;
            }
        };

        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
            if (IsServer)
            {
                Debug.Log($"Player {id} disconnected.");
                _playersInGame.Value--;
            }
        };
    }
}