using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class HUDManager : MonoBehaviour
{
	public TextMeshProUGUI itemNameText;
	public TextMeshProUGUI playerNameText;
	public TextMeshProUGUI playerCoordXText;
	public TextMeshProUGUI playerCoordYText;
	public GameObject minimapContent;
	//leftoff: add logic for minimap pings and info
	public int inventorySize = 8;
	public float inventoryScale = 1f;
	private Sprite[] inventorySprites;
	private int selectedInventorySlot = 0;
	private GameObject[] inventorySlots;
	private Item[] playerInventory;
	private InventoryManager inventoryManager;
	private PlayerController playerController;

	// UI Colors
	public Color inventorySlotColor = new Color32(91, 75, 56, 225);
	public Color inventorySlotBorderColor = new Color32(56, 42, 30, 225);
	public Color selectedInventorySlotBorderColor = new Color32(255, 255, 255, 225);

	private void Awake() {
		this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
	}

	void Update() {
		ScrollHandler();
		DrawPlayerIcons();
		if (playerController == null) {
			Destroy(this.gameObject);
			return;
		}
	}

	public void SetPlayerCoordinates(float coordX, float coordY) {
		playerCoordXText.SetText(((int)coordX).ToString());
		playerCoordYText.SetText(((int)coordY).ToString());
	}

	public void DrawPlayerIcons() {
		// foreach (var player in PhotonNetwork.PlayerList) {
		// 	// Debug.Log(player.NickName);
		// }
		// get list of players from PUN
		// get coord data from list
	}

	public void SetPlayerController(PlayerController _playerController) {
		playerController = _playerController;
		if (playerNameText != null) {
			// playerNameText.text = playerController.photonView.Owner.NickName;
			playerNameText.text = "playernametext";
		}
	}

	public void SetInventoryManager(InventoryManager _inventoryManager) {
		inventoryManager = _inventoryManager;
	}

	private void ScrollHandler() {
		int scrollValue = -(int)Input.mouseScrollDelta.y;
		int previousSelectorValue = selectedInventorySlot;

		if (scrollValue != 0) {
			if (selectedInventorySlot == 0 && scrollValue != -1) {
				selectedInventorySlot += (int)scrollValue;
			} else if (selectedInventorySlot == inventorySize - 1 && scrollValue != 1) {
				selectedInventorySlot += (int)scrollValue;
			} else if (selectedInventorySlot > 0 && selectedInventorySlot < inventorySize - 1){
				selectedInventorySlot += (int)scrollValue;
			}
			inventorySlots[previousSelectorValue].transform.GetChild(0).GetComponent<Image>().color = inventorySlotBorderColor;
			inventorySlots[selectedInventorySlot].transform.GetChild(0).GetComponent<Image>().color = selectedInventorySlotBorderColor;

			Item selectedItem = playerInventory[selectedInventorySlot];
			if (selectedItem != null) {
				inventoryManager.SpawnSelectedItem(selectedItem);
			}
		}
	}

	public void DrawInventoryHud(Item[] inventory) {
		playerInventory = inventory;
		inventorySlots = GameObject.FindGameObjectsWithTag("Inventory Slot");

		for (int i = 0; i < inventorySlots.Length; i++) {
			Image currentSlot = inventorySlots[i].transform.GetChild(1).GetComponent<Image>();
			TMP_Text quantityText = inventorySlots[i].transform.GetChild(2).GetComponent<TMP_Text>();
			if (inventory[i] != null) {
				currentSlot.enabled = true;
        		currentSlot.sprite = inventory[i].itemData.itemImage;
				currentSlot.color = new Color(255, 255, 255, 255);
				if (inventory[i].quantity > 1) {
					quantityText.text = inventory[i].quantity.ToString();
				} else {
					quantityText.text = "";
				}
			} else {
				currentSlot.color = inventorySlotColor;
				quantityText.text = "";
			}
		}
	}

	public void ShowItemInfo(string itemName) {
		itemNameText.SetText(itemName);
	}
}
