using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerNameTag : MonoBehaviourPun
{
    [SerializeField]
	private TextMeshProUGUI nameText;

	private void Start() {
		if (photonView.IsMine) {
			return;
		}

		SetName();
	}

	private void SetName() => nameText.text = "test";
}
