using Unity.Netcode;
using UnityEngine;
using CustomSingletons;
using CustomNetworkVariables;

public class PlayersManager : Singleton<PlayersManager>
{
	private NetworkList<PlayerInfoNetVar> _playersData;

    public int PlayersInGame
    {
        get
        {
            return _playersData.Count;
        }
    }

	public NetworkList<PlayerInfoNetVar> PlayersData
	{
		get
		{
			return _playersData;
		}
	}

    void Awake() {
		DontDestroyOnLoad(gameObject);
		_playersData = new NetworkList<PlayerInfoNetVar>();
    }

    public override void OnNetworkSpawn()
    {
		if (!IsHost) return;

		NetworkManager.Singleton.OnServerStarted += () =>
		{
			_playersData.Clear();
			_playersData.Add(new PlayerInfoNetVar(0, "Player 0"));
		};

        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
			Debug.Log($"Player {id} connected.");
			_playersData.Add(new PlayerInfoNetVar(id, "Player " + id));
			
        };

        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
			Debug.Log($"Player {id} disconnected.");
			for (int i = 0; i < _playersData.Count; i++)
			{
				PlayerInfoNetVar playerInfo = _playersData[i];
				if (playerInfo.PlayerId == id)
				{
					_playersData.RemoveAt(i);
				}
			}
        };
    }
}