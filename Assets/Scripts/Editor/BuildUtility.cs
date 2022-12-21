using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

public class BuildUtility
{
	[MenuItem("BuildUtility/Build and Start Server")]
	static void BuildPlayer ()
	{
		string playerPath = "Builds/Windows/Spaced Out.exe";
		BuildTarget buildTarget = BuildTarget.StandaloneWindows;
		BuildOptions buildOptions = BuildOptions.None;
		BuildReport report = BuildPipeline.BuildPlayer(GetScenePaths(), playerPath, buildTarget, buildOptions);
		BuildSummary summary = report.summary;

		if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
			StartServer();
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
	}

	static string[] GetScenePaths()
	{
		string[] scenes = new string[EditorBuildSettings.scenes.Length];
		for(int i = 0; i < scenes.Length; i++) {
			scenes[i] = EditorBuildSettings.scenes[i].path;
		}
		return scenes;
	}
	static string GetProjectFolderPath()
	{
		var s = Application.dataPath;
		s = s.Substring ( s.Length - 7, 7);
		return s;
	}

	[MenuItem("BuildUtility/Start Server")]
	private static void StartServer()
	{
		System.Diagnostics.Process process = new System.Diagnostics.Process();
		System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
		startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
		startInfo.FileName = "cmd.exe";
		startInfo.Arguments = "/C npm run start-server";
		process.StartInfo = startInfo;
		process.Start();
	}
}