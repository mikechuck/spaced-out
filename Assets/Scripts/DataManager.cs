using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
	public static string playerName;
	public static string levelSeed;

	private void Awake() {
		Instance = this;
		DontDestroyOnLoad(gameObject);
	}

	public static void SetPlayerName(string name) {
		Debug.Log("setting player name");
		playerName = name;
	}

	public static void SetLevelSeed(string seed) {
		Debug.Log("setting level seed");
		levelSeed = seed;
	}
}
