using Unity.Netcode;
using UnityEngine;
using System;
using System.Collections.Generic;
using CustomSingletons;

public class PlayersManager : Singleton<PlayersManager>
{

	public struct PlayerInfo : INetworkSerializable
	{
		private string _playerName;
		private bool _isReady;
		public PlayerInfo(string playerName)
		{
			_playerName = playerName;
			_isReady = false;
		}
		public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
		{
			serializer.SerializeValue(ref _playerName);
			serializer.SerializeValue(ref _isReady);
		}
		// LEFTOFF: continue to create a usable data type, try NetworkList instead?
	}

	private NetworkVariable<Dictionary<ulong, PlayerInfo>> _playersData = new NetworkVariable<Dictionary<ulong, PlayerInfo>>();

    public int PlayersInGame
    {
        get
        {
            return _playersData.Value.Count;
        }
    }

	public Dictionary<ulong, PlayerInfo> PlayersData
	{
		get
		{
			return _playersData.Value;
		}
	}

    void Awake() {
		DontDestroyOnLoad(gameObject);
		
    }

    public override void OnNetworkSpawn()
    {
		if (!IsHost) return;

		_playersData.Value = new Dictionary<ulong, PlayerInfo>();
		// _playersData.Value.Add(Convert.ToUInt64(0), new PlayerInfo("Player 0"));

		NetworkManager.Singleton.OnServerStarted += () =>
		{
			Debug.Log("Server started, adding host to players list");
			_playersData.Value.Add(Convert.ToUInt64(0), new PlayerInfo("Player 0"));
			Debug.Log("PlayersData: " + _playersData.Value);
		};

        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
			Debug.Log($"Player {id} connected.");
			_playersData.Value.Add(id, new PlayerInfo("Player " + id));
			Debug.Log("PlayersData: " + _playersData.Value);

        };

        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
			Debug.Log($"Player {id} disconnected.");
			_playersData.Value.Remove(id);
			Debug.Log("PlayersData: " + _playersData.Value);
        };
    }
}