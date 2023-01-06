using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class UIBillboard : MonoBehaviour
{
    private Transform mainCameraTransform;

	private void Start() {
		if (NetworkManager.Singleton.IsClient)
		{
			mainCameraTransform = Camera.main.transform;
		}
	}
	
	private void LateUpdate() {
		if (NetworkManager.Singleton.IsClient)
		{
			transform.LookAt(transform.position + mainCameraTransform.rotation * Vector3.forward,
				mainCameraTransform.rotation * Vector3.up);
		}
	}
}
