using UnityEngine;
using JetBrains.Annotations;
using System.Collections;

public class LobbyScreen : MonoBehaviour
{
	public delegate void MenuActionDelegate();
	private MenuActionDelegate _onBackButton;
	[SerializeField] private GameObject _playersContainer;
	[SerializeField] private GameObject _playerRow;

	private void Start()
	{
		PlayersManager.OnClientConnectedCallback += UpdatePlayersList;
		// get players list from playerManager
		// loop through and instantiate name per player
		// set player name text

		//LEFTOFF: start lobby loading set to true (display loading message)
		// When callbacks are finished in playermanager (server started or client connected),
		// then ifnish loading message
		// display "Disconnecting, lost connection to server" message on disconect callback
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
	
	private void UpdatePlayersList(ulong playerId)
	{
		Debug.Log("new player connected: " + playerId);
	}
}