using Unity.Netcode;
using UnityEngine;
using System;
using CustomSingletons;

public class PlayersManager : Singleton<PlayersManager>
{
	public delegate void OnClientConnected(ulong playerId);
	public static event OnClientConnected OnClientConnectedCallback;
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
			Debug.Log("on connect");
            if (IsServer)
            {
                Debug.Log($"Player {id} connected.");
                _playersInGame.Value++;
            }

			OnClientConnectedCallback?.Invoke(id);
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