using UnityEngine;
using JetBrains.Annotations;
using System.Collections;
using Unity.Netcode;

public class LobbyScreen : MonoBehaviour
{
	public delegate void MenuActionDelegate();
	private MenuActionDelegate _onBackButton;
	[SerializeField] private GameObject _playersContainer;
	[SerializeField] private GameObject _playerRow;
	[SerializeField] private GameObject _loadingMessagePrefab;

	private void OnEnable()
	{
		if (NetworkManager.Singleton.IsHost)
		{
			ToastService.Instance.DisplayToast("Creating Game...");
			NetworkManager.Singleton.OnServerStarted += () =>
			{
				ToastService.Instance.RemoveToast();	
			};
		}
		else
		{
			ToastService.Instance.DisplayToast("Connecting to Game...");
			NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
			{
				ToastService.Instance.RemoveToast();	
			};
			NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
			{
				ToastService.Instance.DisplayToast("Unable to connect to host.");
				StartCoroutine(ReturnToMenus());
			};
		}

		NetworkManager.Singleton.OnClientConnectedCallback += UpdatePlayersList;
		
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
	
	private void UpdatePlayersList(ulong playerId)
	{
		Debug.Log("new player connected: " + playerId);
	}

	private IEnumerator ReturnToMenus()
	{
		yield return new WaitForSeconds(5);
	}
}