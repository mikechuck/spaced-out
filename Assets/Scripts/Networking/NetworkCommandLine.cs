using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class NetworkCommandLine : MonoBehaviour
{
	private NetworkManager netManager;

	void Awake() {
		DontDestroyOnLoad(gameObject);
	}

	void Start()
	{
		netManager = GetComponent<NetworkManager>();

		if (Application.isEditor) return;

		var args = GetCommandlineArgs();

		// if (args.ContainsKey("-build"))
		// {
		// 	List<string> activeScenes = new List<string>();
		// 	foreach(EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
		// 	{
		// 		if (scene.enabled)
		// 		{
		// 			activeScenes.Add(scene.path);
		// 		}
		// 	}
		// 	var options = new BuildPlayerOptions
        //     {
        //         scenes = activeScenes.ToArray(), 
        //         target = BuildTarget.StandaloneWindows, 
        //         locationPathName = "Builds/Windows",
        //     };

        //     BuildPipeline.BuildPlayer(options);
		// }

		if (args.TryGetValue("-mode", out string mlapiValue))
		{
			switch (mlapiValue)
			{
				case "server":
					netManager.StartServer();
					break;
				case "client":
					netManager.StartClient();
					break;
			}
		}
	}

	private Dictionary<string, string> GetCommandlineArgs()
	{
		Dictionary<string, string> argDictionary = new Dictionary<string, string>();

		var args = System.Environment.GetCommandLineArgs();

		for (int i = 0; i < args.Length; ++i)
		{
			var arg = args[i].ToLower();
			if (arg.StartsWith("-"))
			{
				var value = i < args.Length - 1 ? args[i + 1].ToLower() : null;
				value = (value?.StartsWith("-") ?? false) ? null : value;

				argDictionary.Add(arg, value);
			}
		}
		return argDictionary;
	}
}