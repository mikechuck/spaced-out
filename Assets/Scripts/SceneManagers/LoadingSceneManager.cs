using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class LoadingSceneManager : MonoBehaviour
{
	public void Start()
	{
		SceneManager.LoadScene("Home");
	}
}

