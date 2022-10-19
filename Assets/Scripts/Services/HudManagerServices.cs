using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using ServiceLocatorNamespace;

namespace ServiceLocatorNamespace
{
	public interface IHudManagerService: IGameService
	{
		void DrawInventoryHud(Item[] inventory);
		void ShowItemInfo(string itemName);
	}
}

public class HudManagerService : IHudManagerService
{
	public void DrawInventoryHud(Item[] inventory) {
		Debug.Log("drawinventoryhud in service");
		EventManager.TriggerEvent("HudManagerDrawInventoryHud");
	}

	public void ShowItemInfo(string itemName) {
		Debug.Log("Showing item info");
		// add event action call here for ShowItemInfo
	}
}
