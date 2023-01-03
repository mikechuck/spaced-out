using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class HUDManager : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _playersInGameText;
	[SerializeField] private TextMeshProUGUI _itemNameText;
	[SerializeField] private TextMeshProUGUI _playerNameText;
	[SerializeField] private TextMeshProUGUI _playerCoordXText;
	[SerializeField] private TextMeshProUGUI _playerCoordYText;
	[SerializeField] private int _inventorySize = 8;
	private int _selectedInventorySlot = 0;
	private GameObject[] _inventorySlots;
	private Item[] _playerInventory;
	private InventoryManager _inventoryManager;
	private PlayerController _playerController;

	// UI Colors
	[SerializeField] private Color _inventorySlotColor = new Color32(91, 75, 56, 225);
	[SerializeField] private Color _inventorySlotBorderColor = new Color32(56, 42, 30, 225);
	[SerializeField] private Color _selectedInventorySlotBorderColor = new Color32(255, 255, 255, 225);

	#region Lifecycle

	private void Awake() {
		this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
	}

	void Update() {
		ScrollHandler();
		UpdatePlayerCount();
		if (_playerController == null) {
			Destroy(this.gameObject);
			return;
		}
	}

	#endregion

	public void SetPlayerCoordinates(float coordX, float coordY) {
		_playerCoordXText.SetText(((int)coordX).ToString());
		_playerCoordYText.SetText(((int)coordY).ToString());
	}

	public void SetPlayerController(PlayerController playerController) {
		_playerController = playerController;
	}

	public void SetInventoryManager(InventoryManager inventoryManager) {
		_inventoryManager = inventoryManager;
	}

	public void SetPlayerName(string playerName)
	{
		_playerNameText.text = playerName;
	}

	private void ScrollHandler() {
		int scrollValue = -(int)Input.mouseScrollDelta.y;
		int previousSelectorValue = _selectedInventorySlot;

		if (scrollValue != 0) {
			if (_selectedInventorySlot == 0 && scrollValue != -1) {
				_selectedInventorySlot += (int)scrollValue;
			} else if (_selectedInventorySlot == _inventorySize - 1 && scrollValue != 1) {
				_selectedInventorySlot += (int)scrollValue;
			} else if (_selectedInventorySlot > 0 && _selectedInventorySlot < _inventorySize - 1){
				_selectedInventorySlot += (int)scrollValue;
			}
			_inventorySlots[previousSelectorValue].transform.GetChild(0).GetComponent<Image>().color = _inventorySlotBorderColor;
			_inventorySlots[_selectedInventorySlot].transform.GetChild(0).GetComponent<Image>().color = _selectedInventorySlotBorderColor;

			Item selectedItem = _playerInventory[_selectedInventorySlot];
			if (selectedItem != null) {
				_inventoryManager.SpawnSelectedItem(selectedItem);
			}
		}
	}

	public void DrawInventoryHud(Item[] inventory) {
		_playerInventory = inventory;
		_inventorySlots = GameObject.FindGameObjectsWithTag("Inventory Slot");

		for (int i = 0; i < _inventorySlots.Length; i++) {
			Image currentSlot = _inventorySlots[i].transform.GetChild(1).GetComponent<Image>();
			TMP_Text quantityText = _inventorySlots[i].transform.GetChild(2).GetComponent<TMP_Text>();
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
				currentSlot.color = _inventorySlotColor;
				quantityText.text = "";
			}
		}
	}

	public void ShowItemInfo(string itemName) {
		_itemNameText.SetText(itemName);
	}

	private void UpdatePlayerCount()
	{
		_playersInGameText.text = $"Players in game: {PlayersManager.Instance.PlayersInGame}";
	}
}
