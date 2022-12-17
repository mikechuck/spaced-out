using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class PlayerNameTag : NetworkBehaviour
{
    [SerializeField]
	private TextMeshProUGUI nameText;
	private GameManager gameManager;

	private void Awake() {
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	private void Start() {
		if (IsOwner) {
			return;
		}

		SetName(gameManager.playerName);
	}

	private void SetName(string name) {
		nameText.text = name;
	}
}
