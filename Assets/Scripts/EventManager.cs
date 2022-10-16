using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class EventManager : MonoBehaviour
{
	private Dictionary<string, UnityEvent> eventDictionary;
	
	private static EventManager eventManager;
    
	public static EventManager instance {
		get {
			if (!eventManager) {
				eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

				if (!eventManager) {
					Debug.LogError("There needs to be one active EventManager script on a GameObject");
				} else {
					eventManager.Init();
				}
			}
			return eventManager;
		}
	}

	void Init() {
		if (eventDictionary == null) {
			eventDictionary = new Dictionary<string, UnityEvent>();
		}
	}

	public static void StartListening(string eventName, UnityAction listener) {
		UnityEvent thisEvent = null;
		if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
			thisEvent.AddListener(listener);
		} else {
			thisEvent = new UnityEvent();
			thisEvent.AddListener(listener);
			instance.eventDictionary.Add(eventName, thisEvent);
		}
	}
	public static void StopListening(string eventName, UnityAction listener) {
		if (eventManager == null) return;
		UnityEvent thisEvent = null;
		if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
			thisEvent.RemoveListener(listener);
		}
	}
	public static void TriggerEvent(string eventName) {
		Debug.Log("triggering");
		Debug.Log(eventName);
		foreach(KeyValuePair<string, UnityEvent> item in instance.eventDictionary) {
			Debug.Log(item.Key);
		}
		UnityEvent thisEvent = null;
		if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
			Debug.Log("found match");
			thisEvent.Invoke();
		}
	}
}
