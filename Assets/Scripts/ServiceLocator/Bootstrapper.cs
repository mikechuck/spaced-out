using UnityEngine;
using ServiceLocatorSample.ServiceLocator;

namespace ServiceLocatorSample.ServiceLocator
{
	public static class Bootstrapper
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		public static void Initialize() {
			ServiceLocator.Initialize();
			Debug.Log("Starting...");
			ServiceLocator.Current.Register<IInventoryManager>(new InventoryManager());

			// SceneManager.LoadSceneAsync(1, Load)
		}
	}
}

// Leftoff: figure out how to se current registered services, and call inventorymanager test function