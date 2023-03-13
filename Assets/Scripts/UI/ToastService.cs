using Unity.Netcode;
using UnityEngine;
using System;
using CustomSingletons;

public class ToastService : Singleton<ToastService>
{
	[SerializeField] private ToastMessage _toastMessagePrefab;
	private ToastMessage _toastMessage;

	void Awake() {
		DontDestroyOnLoad(gameObject);
    }

	public ToastMessage DisplayToast(string message)
	{
		RemoveToast();
		_toastMessage = Instantiate(_toastMessagePrefab);
		_toastMessage.ToastText = message;
		return _toastMessage;
	}

	public void RemoveToast()
	{
		if (_toastMessage != null)
		{
			Destroy(_toastMessage.gameObject);
		}
	}
}