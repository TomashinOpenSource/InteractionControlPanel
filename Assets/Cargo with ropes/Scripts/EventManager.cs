using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    private Dictionary<string, UnityEvent> eventDictionary;
    private Dictionary<string, UnityEvent<string>> eventDictionaryBEN;
    private Dictionary<string, UnityEvent<bool>> eventDictionaryBool;

    private static EventManager eventManager;

    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;
                if (!eventManager) Debug.LogError("Needs one EventManager script on scene!!!");
                else eventManager.Init();
            }
            return eventManager;
        }
    }

    private void Init()
    {
        if (eventDictionary == null) eventDictionary = new Dictionary<string, UnityEvent>();
        if (eventDictionaryBEN == null) eventDictionaryBEN = new Dictionary<string, UnityEvent<string>>();
        if (eventDictionaryBool == null) eventDictionaryBool = new Dictionary<string, UnityEvent<bool>>();
    }

    public static void StartListening(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) thisEvent.AddListener(listener);
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StartListening(string eventName, UnityAction<string> listener)
    {
        UnityEvent<string> thisEvent = null;
        if (instance.eventDictionaryBEN.TryGetValue(eventName, out thisEvent)) thisEvent.AddListener(listener);
        else
        {
            thisEvent = new UnityEvent<string>();
            thisEvent.AddListener(listener);
            instance.eventDictionaryBEN.Add(eventName, thisEvent);
        }
    }

    public static void StartListening(string eventName, UnityAction<bool> listener)
    {
        UnityEvent<bool> thisEvent = null;
        if (instance.eventDictionaryBool.TryGetValue(eventName, out thisEvent)) thisEvent.AddListener(listener);
        else
        {
            thisEvent = new UnityEvent<bool>();
            thisEvent.AddListener(listener);
            instance.eventDictionaryBool.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction listener)
    {
        if (eventManager == null) return;
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) thisEvent.RemoveListener(listener);
    }

    public static void StopListening(string eventName, UnityAction<string> listener)
    {
        if (eventManager == null) return;
        UnityEvent<string> thisEvent = null;
        if (instance.eventDictionaryBEN.TryGetValue(eventName, out thisEvent)) thisEvent.RemoveListener(listener);
    }

    public static void StopListening(string eventName, UnityAction<bool> listener)
    {
        if (eventManager == null) return;
        UnityEvent<bool> thisEvent = null;
        if (instance.eventDictionaryBool.TryGetValue(eventName, out thisEvent)) thisEvent.RemoveListener(listener);
    }

    public static void TriggerEvent(string eventName)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
            return;
        }
    }
    public static void TriggerEvent(string eventName, string param)
    {
        UnityEvent<string> thisEvent = null;
        if (instance.eventDictionaryBEN.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(param);
            return;
        }
    }

    public static void TriggerEvent(string eventName, bool param)
    {
        UnityEvent<bool> thisEvent = null;
        if (instance.eventDictionaryBool.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(param);
            return;
        }
    }
}
