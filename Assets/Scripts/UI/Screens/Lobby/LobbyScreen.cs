using UnityEngine;
using JetBrains.Annotations;
using Unity.Netcode;
using System.Collections.Generic;
using CustomNetworkVariables;
using System;

public class LobbyScreen : MonoBehaviour
{
	public delegate void MenuActionDelegate();
	private MenuActionDelegate _onBackButton;
	[SerializeField] private GameObject _playersLayoutGroup;
	[SerializeField] private PlayerRow _playerRow;
	private List<PlayerRow> _playerRows = new List<PlayerRow>();
	private void Start()
	{
		if (!NetworkManager.Singleton.IsHost)
		{
			ToastService.Instance.DisplayToast("Connecting to game...");
			NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
			{
				ToastService.Instance.RemoveToast();	
			};

			NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
			{
				ToastService.Instance.DisplayToast("Unable to connect to host");
			};
		}
		else
		{
			CreatePlayerRow(PlayersManager.Instance.PlayersData[0]);
		}

		PlayersManager.Instance.PlayersData.OnListChanged += UpdatePlayersList;
	}

	public void SetBackButtonAction(MenuActionDelegate action)
	{
		_onBackButton = action;
	}

	[UsedImplicitly]
	public void OnBackButton()
	{
		_onBackButton();
	}

	[UsedImplicitly]
	public void OnStartGame()
	{
		// TODO: "start game" button will use networkmanager to change scenes for all clients
		// iff all client are in "ready" status
	}

	public void Show()
	{
		if (!this) return;
		gameObject.SetActive(true);
	}

	public void Hide()
	{
		if (!this) return;
		gameObject.SetActive(false);
	}
	
	public void UpdatePlayersList(NetworkListEvent<PlayerInfoNetVar> changeEvent)
	{
		ResetPlayerList();
		foreach(PlayerInfoNetVar playerInfo in PlayersManager.Instance.PlayersData)
		{
			CreatePlayerRow(playerInfo);
		}
	}

	private void CreatePlayerRow(PlayerInfoNetVar playerData)
	{
		PlayerRow playerRow = Instantiate(_playerRow, _playersLayoutGroup.transform);
		playerRow.SetPlayerName(playerData.PlayerName);
		playerRow.SetPlayerStatus(playerData.IsReady);
		_playerRows.Add(playerRow);
	}

	private void ResetPlayerList()
	{
		foreach(PlayerRow playerRow in _playerRows)
		{
			Debug.Log("destroying player row");
			Destroy(playerRow.gameObject);
		}
		_playerRows.Clear();
	}
}

// leftoff: add player name input to player info