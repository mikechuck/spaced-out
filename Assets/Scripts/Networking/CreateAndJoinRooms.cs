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
	public TMP_InputField playerName;
	public TMP_InputField seed;

	public void CreateRoom() {
		DataManager.playerName = playerName.text;
		DataManager.levelSeed = seed.text;
		PhotonNetwork.CreateRoom(createInput.text);
	}

	public void JoinRoom() {
		PhotonNetwork.JoinRoom(joinInput.text);
	}

	public override void OnJoinedRoom() {
		PhotonNetwork.LoadLevel("MainScene");
	}
}
