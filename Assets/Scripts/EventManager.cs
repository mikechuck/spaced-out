using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DrawInventoryHudEvent : UnityEvent<Item[]> { }
[System.Serializable]
public class SpawnSelectedItemEvent : UnityEvent<Item> {}

public class EventManager : MonoBehaviour
{
	private Dictionary<string, UnityEvent> eventDictionary;
	private Dictionary<string, DrawInventoryHudEvent> drawInventoryHudEventDictionary;
	private Dictionary<string, SpawnSelectedItemEvent> spawnSelectedItemEventDictionary;
	
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
		if (drawInventoryHudEventDictionary == null) {
			drawInventoryHudEventDictionary = new Dictionary<string, DrawInventoryHudEvent>();
		}
		if (spawnSelectedItemEventDictionary == null) {
			spawnSelectedItemEventDictionary = new Dictionary<string, SpawnSelectedItemEvent>();
		}
	}

	// Typeless events
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
		UnityEvent thisEvent = null;
		if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
			thisEvent.Invoke();
		}
	}

	// Item[] type events
	public static void StartListening(string eventName, UnityAction<Item[]> listener) {
		DrawInventoryHudEvent thisEvent = null;
		if (instance.drawInventoryHudEventDictionary.TryGetValue(eventName, out thisEvent)) {
			thisEvent.AddListener(listener);
		} else {
			thisEvent = new DrawInventoryHudEvent();
			thisEvent.AddListener(listener);
			instance.drawInventoryHudEventDictionary.Add(eventName, thisEvent);
		}
	}
	public static void StopListening(string eventName, UnityAction<Item[]>  listener) {
		if (eventManager == null) return;
		DrawInventoryHudEvent thisEvent = null;
		if (instance.drawInventoryHudEventDictionary.TryGetValue(eventName, out thisEvent)) {
			thisEvent.RemoveListener(listener);
		}
	}
	public static void TriggerEvent(string eventName, Item[] data) {
		DrawInventoryHudEvent thisEvent = null;
		if (instance.drawInventoryHudEventDictionary.TryGetValue(eventName, out thisEvent)) {
			thisEvent.Invoke(data);
		}
	}

	// Item type events
	public static void StartListening(string eventName, UnityAction<Item> listener) {
		SpawnSelectedItemEvent thisEvent = null;
		if (instance.spawnSelectedItemEventDictionary.TryGetValue(eventName, out thisEvent)) {
			thisEvent.AddListener(listener);
		} else {
			thisEvent = new SpawnSelectedItemEvent();
			thisEvent.AddListener(listener);
			instance.spawnSelectedItemEventDictionary.Add(eventName, thisEvent);
		}
	}
	public static void StopListening(string eventName, UnityAction<Item>  listener) {
		if (eventManager == null) return;
		SpawnSelectedItemEvent thisEvent = null;
		if (instance.spawnSelectedItemEventDictionary.TryGetValue(eventName, out thisEvent)) {
			thisEvent.RemoveListener(listener);
		}
	}
	public static void TriggerEvent(string eventName, Item data) {
		SpawnSelectedItemEvent thisEvent = null;
		if (instance.spawnSelectedItemEventDictionary.TryGetValue(eventName, out thisEvent)) {
			thisEvent.Invoke(data);
		}
	}
}
