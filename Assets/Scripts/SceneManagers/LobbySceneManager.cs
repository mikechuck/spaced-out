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
		// Will handle room codes and seeds later, for now just load main scene
		SceneManager.LoadScene("MainScene");
	}
}