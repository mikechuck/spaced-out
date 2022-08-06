namespace ServiceLocatorSample.ServiceLocator
{
	public static class Bootstrapper
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		public static void Initialize() {
			ServiceLocator.Initialize();
			
			ServiceLocator.Current.Register<IMyGameService>(new InventoryManager());

			// SceneManager.LoadSceneAsync(1, Load)
		}
	}
}
//Leftoff: rewrite the inventorymanager class as an IGameService interface and in the namespace.
// this way it will be retreivable by the servicelocator

// Remove monobehavior from inventorymanager?