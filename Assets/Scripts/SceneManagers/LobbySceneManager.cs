using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.Netcode;

public class LobbySceneManager : MonoBehaviour
{
	public TMP_InputField createInput;
	public TMP_InputField joinInput;
	public TMP_InputField playerNameInput;
	public TMP_InputField seedInput;
	private GameManager gameManager;

    private void Awake()
	{
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

		// Will handle room codes and seeds later, for now just load main scene
		if (NetworkManager.Singleton.IsServer)
		{
			SceneManager.LoadScene("MainScene");
		}
	}
	public void OnStartGameButton()
	{
		SceneManager.LoadScene("MainScene");
	}

	// public void OnJoinRoomButton()
	// {
	// 	StartNewClient(joinInput.text);
	// }

	// private void StartNewClient(string id)
	// {
	// 	NetworkManager.Singleton.StartClient();
	// 	SceneManager.LoadScene("MainScene")
	// }
}