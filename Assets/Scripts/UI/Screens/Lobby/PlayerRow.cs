using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Unity.Collections;

public class PlayerRow : MonoBehaviour
{
	[SerializeField] private TMP_Text _nameText;
	[SerializeField] private TMP_Text _statusText;

	public void SetPlayerName(FixedString128Bytes playerName)
	{
		_nameText.text = Convert.ToString(playerName);
	}

	public void SetPlayerStatus(bool isReady)
	{
		_statusText.text = isReady ? "Ready" : "Not Ready";
	}
}
