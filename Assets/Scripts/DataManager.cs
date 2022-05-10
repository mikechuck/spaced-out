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
}
