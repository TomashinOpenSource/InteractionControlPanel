using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum strin_g { ButtonNorthPressed, ButtonSouthPressed, ButtonOstPressed, ButtonWestPressed, ButtonUpPressed, ButtonDownPressed }

public class SelectionManager : MonoBehaviour
{
    [SerializeField]
    private Material HighlightMat;
    [SerializeField]
    private Material DefaultMat;
    [SerializeField]
    private string SelectableTag = "Selectable";

    private Transform SelectedObject = null;

    void Update()
    {
        #region Selection

        var ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selection = hit.transform;
            if (selection.CompareTag(SelectableTag))
            {
                if (SelectedObject == null)
                {
                    var selectionRenderer = selection.GetComponent<Renderer>();
                    if (selectionRenderer != null)
                    {
                        DefaultMat = selectionRenderer.material;
                        selectionRenderer.material = HighlightMat;
                        SelectedObject = selection;
                    }
                }
            }
            else if (SelectedObject != null)
            {
                SelectedObject.GetComponent<Renderer>().material = DefaultMat;
                SelectedObject = null;
            }
        }

        #endregion

        #region MouseEvent

        if (Input.GetKeyDown(KeyCode.Mouse0) && SelectedObject != null)
        {
            EventManager.TriggerEvent("ButtonDownEvent", SelectedObject.name);
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            EventManager.TriggerEvent("StopCraneEvent");
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            EventManager.TriggerEvent("HookEvent");
        }

        #endregion

        #region DevelopKeyBoardControl

        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            EventManager.TriggerEvent("ButtonDownEvent", "ControlPanelNorthButton");
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            EventManager.TriggerEvent("ButtonDownEvent", "ControlPanelSouthButton");
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            EventManager.TriggerEvent("ButtonDownEvent", "ControlPanelWestButton");
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            EventManager.TriggerEvent("ButtonDownEvent", "ControlPanelOstButton");
        }
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            EventManager.TriggerEvent("ButtonDownEvent", "ControlPanelDownButton");
        }
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            EventManager.TriggerEvent("ButtonDownEvent", "ControlPanelUpButton");
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            EventManager.TriggerEvent("HookEvent");
        }

        if (Input.GetKeyUp(KeyCode.Keypad2))
            EventManager.TriggerEvent("StopCraneEvent");
        if (Input.GetKeyUp(KeyCode.Keypad8))
            EventManager.TriggerEvent("StopCraneEvent");
        if (Input.GetKeyUp(KeyCode.Keypad4))
            EventManager.TriggerEvent("StopCraneEvent");
        if (Input.GetKeyUp(KeyCode.Keypad6))
            EventManager.TriggerEvent("StopCraneEvent");
        if (Input.GetKeyUp(KeyCode.Keypad7))
            EventManager.TriggerEvent("StopCraneEvent");
        if (Input.GetKeyUp(KeyCode.Keypad9))
            EventManager.TriggerEvent("StopCraneEvent");

        #endregion
    }
}
