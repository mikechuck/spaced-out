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
		DontDestroyOnLoad(gameObject);
    }

	void Start() {
		Debug.Log("game manager start");
	}
}
