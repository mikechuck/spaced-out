using UnityEngine;
using ServiceLocatorNamespace;
using Photon.Pun;

namespace ServiceLocatorNamespace
{
	public interface IInventoryManagerService: IGameService
	{
		void SpawnSelectedItem(Item selectedItem);
		void PickUpItem(ItemData itemData);
	}
}

public class InventoryManagerService : IInventoryManagerService
{
    public void SpawnSelectedItem(Item selectedItem) {

	}

	public void PickUpItem(ItemData itemData) {

	}
}

