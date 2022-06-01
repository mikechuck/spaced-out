using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerNameTag : MonoBehaviourPun
{
    [SerializeField]
	private TextMeshProUGUI nameText;
	private GameManager gameManager;

	private void Awake() {
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	private void Start() {
		if (photonView.IsMine) {
			return;
		}

		SetName(gameManager.playerName);
	}

	private void SetName(string name) {
		nameText.text = name;
	}
}
