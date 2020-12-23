using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomLog : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.StartListening("CustomLogError", LogErrorHandler);
        EventManager.StartListening("CustomLog", LogHandler);
    }
    private void OnDisable()
    {
        EventManager.StopListening("CustomLogError", LogErrorHandler);
        EventManager.StopListening("CustomLog", LogHandler);
    }

    private void LogErrorHandler(string message)
    {
        Debug.Log($"ERROR: {message}");
    }

    private void LogHandler(string message)
    {
        Debug.Log($"OK {message}");
    }
}
