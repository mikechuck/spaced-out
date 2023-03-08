using UnityEngine;
using Unity.Netcode;

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
		Debug.Log("show mainmenu screen");
	}
	public void ShowJoinGameScreen()
	{
		HideAllScreens();
		_joinGameScreen = _joinGameScreen != null ? _joinGameScreen : Instantiate(_joinGameScreenPrefab, _mainCanvas.transform);
		_joinGameScreen.SetBackButtonAction(ShowMainMenuScreen);
		_joinGameScreen.Show();
		Debug.Log("showing join game screen");
	}
	public void ShowCreateGameScreen()
	{
		HideAllScreens();
		_createGameScreen = _createGameScreen != null ? _createGameScreen : Instantiate(_createGameScreenPrefab, _mainCanvas.transform);
		_createGameScreen.SetBackButtonAction(ShowMainMenuScreen);
		_createGameScreen.Show();
		Debug.Log("showing create game screen");
	}
	public void ShowLobbyScreen()
	{
		HideAllScreens();
		_lobbyScreen = _lobbyScreen != null ? _lobbyScreen : Instantiate(_lobbyScreenPrefab, _mainCanvas.transform);
		_lobbyScreen.SetBackButtonAction(ShowMainMenuScreen);
		_lobbyScreen.Show();
		Debug.Log("showing lobby screen");
	}

	private void HideAllScreens()
	{
		if (_mainMenuScreen != null) _mainMenuScreen.Hide();
		if (_joinGameScreen != null) _joinGameScreen.Hide();
		if (_createGameScreen != null) _createGameScreen.Hide();
		if (_lobbyScreen != null) _lobbyScreen.Hide();
	}
}