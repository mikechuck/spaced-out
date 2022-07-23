using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TypedEvent: UnityEvent<object>{ }

public class EventManager : MonoBehaviour
{
	private Dictionary<string, UnityEvent> _events;
	private Dictionary<string, TypedEvent> _typedEvents;
	private static EventManager _eventManager;
    
	public static EventManager instance {
		get {
			if (!_eventManager) {
				_eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

				if (!_eventManager) {
					Debug.LogError("There needs to be one active EventManager script on a GameObject");
				} else {
					_eventManager.Init();
				}
			}
			return _eventManager;
		}
	}

	void Init() {
		if (_events == null) {
			_events = new Dictionary<string, UnityEvent>();
		}

		if (_typedEvents == null) {
			_typedEvents = new Dictionary<string, TypedEvent>();
		}
	}

	// Untyped events

	public static void StartListening(string eventName, UnityAction listener) {
		UnityEvent evt = null;
		if (instance._events.TryGetValue(eventName, out evt)) {
			evt.AddListener(listener);
		} else {
			evt = new UnityEvent();
			evt.AddListener(listener);
			instance._events.Add(eventName, evt);
		}
	}

	public static void StopListening(string eventName, UnityAction listener) {
		if (_eventManager == null) return;
		UnityEvent evt = null;
		if (instance._events.TryGetValue(eventName, out evt)) {
			evt.RemoveListener(listener);
		}
	}

	public static void TriggerEvent(string eventName) {
		UnityEvent evt = null;
		if (instance._events.TryGetValue(eventName, out evt)) {
			evt.Invoke();
		}
	}

	// Typed events

	public static void StartListening(string eventName, UnityAction<object> listener) {
		TypedEvent thisEvent = null;
		if (instance._typedEvents.TryGetValue(eventName, out thisEvent)) {
			thisEvent.AddListener(listener);
		} else {
			thisEvent = new TypedEvent();
			thisEvent.AddListener(listener);
			instance._typedEvents.Add(eventName, thisEvent);
		}
	}

	public static void StopListening(string eventName, UnityAction<object> listener) {
		if (_eventManager == null) return;
		TypedEvent thisEvent = null;
		if (instance._typedEvents.TryGetValue(eventName, out thisEvent)) {
			thisEvent.RemoveListener(listener);
		}
	}

	public static void TriggerEvent(string eventName, object data) {
		UnityEvent thisEvent = null;
		if (instance._typedEvents.TryGetValue(eventName, out thisEvent)) {
			thisEvent.Invoke();
		}
	}
}
