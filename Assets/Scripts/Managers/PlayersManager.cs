using Unity.Netcode;
using UnityEngine;
using System;
using CustomSingletons;

public class PlayersManager : Singleton<PlayersManager>
{
    private NetworkVariable<int> _playersInGame = new NetworkVariable<int>();

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
        Debug.Log(NetworkManager.Singleton);
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