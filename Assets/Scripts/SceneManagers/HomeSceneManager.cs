using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using System;
using System.Net;
using System.Net.Sockets;

public class HomeSceneManager : NetworkBehaviour
{
	[SerializeField] private Canvas _mainCanvas;
	[SerializeField] private MainMenuScreen _mainMenuScreenPrefab;
	[SerializeField] private JoinGameScreen _joinGameScreenPrefab;
	[SerializeField] private CreateGameScreen _createGameScreenPrefab;
	[SerializeField] private LobbyScreen _lobbyScreenPrefab;
	private MainMenuScreen _mainMenuScreen;
	private JoinGameScreen _joinGameScreen;
	private CreateGameScreen _createGameScreen;
	private LobbyScreen _lobbyScreen;
	private GameManager _gameManager;
	private string _userIpAddress;

	private void Awake()
	{
		var host = Dns.GetHostEntry(Dns.GetHostName());
		foreach (var ip in host.AddressList)
         {
             if (ip.AddressFamily == AddressFamily.InterNetwork)
             {
                 _userIpAddress = ip.ToString();
				 break;
             }
         }
	}

	private void Start()
	{
		ShowMainMenuScreen();
	}

	public void ShowMainMenuScreen()
	{
		HideAllScreens();
		_mainMenuScreen = _mainMenuScreen != null ? _mainMenuScreen : Instantiate(_mainMenuScreenPrefab, _mainCanvas.transform);
		_mainMenuScreen.SetJoinGameAction(ShowJoinGameScreen);
		_mainMenuScreen.SetCreateGameAction(ShowCreateGameScreen);
		_mainMenuScreen.Show();
	}

	public void ShowJoinGameScreen()
	{
		HideAllScreens();
		_joinGameScreen = _joinGameScreen != null ? _joinGameScreen : Instantiate(_joinGameScreenPrefab, _mainCanvas.transform);
		_joinGameScreen.SetBackButtonAction(ShowMainMenuScreen);
		_joinGameScreen.SetJoinGameButtonAction(StartClient);
		_joinGameScreen.Show();
	}

	public void ShowCreateGameScreen()
	{
		HideAllScreens();
		_createGameScreen = _createGameScreen != null ? _createGameScreen : Instantiate(_createGameScreenPrefab, _mainCanvas.transform);
		_createGameScreen.SetBackButtonAction(ShowMainMenuScreen);
		_createGameScreen.SetCreateGameButtonAction(StartHost);
		_createGameScreen.SetIpAddress(_userIpAddress);
		_createGameScreen.Show();
	}

	public void ShowLobbyScreen()
	{
		HideAllScreens();
		_lobbyScreen = _lobbyScreen != null ? _lobbyScreen : Instantiate(_lobbyScreenPrefab, _mainCanvas.transform);
		_lobbyScreen.SetBackButtonAction(() =>
		{
			NetworkManager.Singleton.Shutdown();
			Debug.Log("calling network shutdown");
			ShowMainMenuScreen();
		});
		_lobbyScreen.Show();
	}

	private void HideAllScreens()
	{
		if (_mainMenuScreen != null) _mainMenuScreen.Hide();
		if (_joinGameScreen != null) _joinGameScreen.Hide();
		if (_createGameScreen != null) _createGameScreen.Hide();
		if (_lobbyScreen != null) _lobbyScreen.Hide();
	}

	public void StartHost(string password)
	{
		NetworkManager.Singleton.StartHost();
		ShowLobbyScreen();
	}

	public void StartClient(string ipAddress, string password)
	{
		NetworkManager.Singleton.StartClient();
		ShowLobbyScreen();
	}
}