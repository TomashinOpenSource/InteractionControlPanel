using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContolPanelButton : MonoBehaviour
{
    public ControlPanelButtons button;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finger")
        {
            Debug.Log(button + " pressed");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Finger")
        {
            Debug.Log(button + " unpressed");
        }
    }
}

public enum ControlPanelButtons
{
    ControlPanelNorthButton,
    ControlPanelSouthButton,
    ControlPanelWestButton,
    ControlPanelOstButton,
    ControlPanelDownButton,
    ControlPanelUpButton
}
