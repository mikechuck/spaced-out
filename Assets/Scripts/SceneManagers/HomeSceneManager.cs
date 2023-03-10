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
			if (IsHost)
			{
				NetworkManager.Singleton.Shutdown();
			}
			else
			{
				NetworkManager.Singleton.DisconnectClient(NetworkManager.Singleton.LocalClientId);
			}
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
		NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = _userIpAddress;
		NetworkManager.Singleton.StartHost();
		ShowLobbyScreen();

		// TODO For pwd authenticated sessions, see https://docs-multiplayer.unity3d.com/netcode/current/basics/connection-approval/index.html
	}

	public void StartClient(string ipAddress, string password)
	{
		NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = ipAddress;
		if (password != null)
		{
			ushort val = 0;
			if (ushort.TryParse(password, out val))
			{
				NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Port = val;

			}
		}
		NetworkManager.Singleton.StartClient();
		ShowLobbyScreen();
	}
}