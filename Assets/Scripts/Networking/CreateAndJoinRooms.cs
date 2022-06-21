using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
	public TMP_InputField createInput;
	public TMP_InputField joinInput;
	public TMP_InputField playerNameInput;
	public TMP_InputField seedInput;
	private GameObject gameManagerObject;
	private GameManager gameManager;

	private void Awake() {
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	public void CreateRoom() {
		gameManager.levelSeed = seedInput.text;
		PhotonNetwork.CreateRoom(createInput.text);
	}

	public void JoinRoom() {
		PhotonNetwork.JoinRoom(joinInput.text);
	}

	public override void OnJoinedRoom() {
		gameManager.playerName = playerNameInput.text;
		PhotonNetwork.LoadLevel("MainScene");
	}
}
