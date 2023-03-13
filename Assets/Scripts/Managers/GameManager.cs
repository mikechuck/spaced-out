using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class GameManager : MonoBehaviour
{
	public string playerName = "Player Name (GM)";
	public string levelSeed = "";
	
    void Awake() {
		Application.runInBackground = true;
		Application.targetFrameRate = 60;
		Screen.SetResolution(1400, 800, FullScreenMode.Windowed);
		DontDestroyOnLoad(gameObject);
    }

	void Start() {
		
	}
}
