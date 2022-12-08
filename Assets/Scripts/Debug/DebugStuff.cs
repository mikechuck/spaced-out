using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugStuff : MonoBehaviour
{
	static string myLog;
	private string _output;
	private string _stack;
	private bool _showLogsGUI = true;

	void Update() {
		if (Input.GetKey(KeyCode.Tilde))
		{
			_showLogsGUI = !_showLogsGUI;
		}
	}

	void OnEnable()
	{
		Application.logMessageReceived += Log;
	}

	void OnDisable()
	{
		Application.logMessageReceived -= Log;
	}

	public void Log(string logString, string stackTrace, LogType type)
	{
		_output = logString;
		_stack = stackTrace;
		myLog = _output + "\n" + myLog;
		if (myLog.Length > 5000)
		{
			myLog = myLog.Substring(0, 4000);
		}
	}

	void OnGUI()
	{
		if (_showLogsGUI)
		{
			myLog = GUI.TextArea(new Rect(10, 10, Screen.width - 10, Screen.height - 10), myLog);
		}
	}
}
