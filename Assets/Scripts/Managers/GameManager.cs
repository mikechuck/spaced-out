using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public string playerName = "Player Name (GM)";
	public string levelSeed = "";
	
    void Awake() {
		DontDestroyOnLoad(gameObject);
    }

	void Start() {
		// if (SceneManager.GetActiveScene().name != "Loading") {
		// 	SceneManager.LoadScene("Loading");
		// }
	}
}
