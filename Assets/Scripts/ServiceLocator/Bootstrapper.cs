using UnityEngine;
using ServiceLocatorNamespace;

namespace ServiceLocatorNamespace
{
	public static class Bootstrapper
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		public static void Initialize() {
			// ServiceLocator.Initialize();
			// ServiceLocator.Current.Register<IInventoryManagerService>(new InventoryManagerService());
			// ServiceLocator.Current.Register<IHudManagerService>(new HudManagerService());
		}
	}
}