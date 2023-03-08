using UnityEngine;
// using Unity.Netcode;
using JetBrains.Annotations;

public class LobbyScreen : MonoBehaviour
{
	public delegate void MenuActionDelegate();
	private MenuActionDelegate _onBackButton;
	[SerializeField] private GameObject _playersContainer;
	[SerializeField] private GameObject _playerRow;

	private void Start()
	{
		// get players list from playerManager
		// loop through and instantiate name per player
		// set player name text
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
}