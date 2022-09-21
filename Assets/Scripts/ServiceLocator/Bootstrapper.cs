using UnityEngine;
using ServiceLocatorNamespace;

namespace ServiceLocatorNamespace
{
	public static class Bootstrapper
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		public static void Initialize() {
			ServiceLocator.Initialize();
		}
	}
}