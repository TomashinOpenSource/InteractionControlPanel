using HTC.UnityPlugin.Vive;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Controller : MonoBehaviour
{
    [Header("Джойстики для дочерности")]
    public Transform[] grabbers;
    private int lastHand = -1;

    [Header("Руки")]
    public PlayAnimator[] hands;

    [Header("Пульт")]
    public Custom3DButtonEventReciever[] buttons = new Custom3DButtonEventReciever[6];
    private int currentButton;
    private Custom3DButtonEventReciever lastButton;
    PointerEventData pointerEventData;

    public int CurrentButton 
    { 
        get => currentButton; 
        set
        {
            currentButton = value;
            if (value < 0) return;
            if (currentButton == buttons.Length) currentButton = 0;
            if (currentButton < 0) currentButton = buttons.Length - 1;
            OnButtonPressed(false);
            if (lastButton != null) lastButton.OnPointerExit(pointerEventData);
            buttons[currentButton].OnPointerEnter(pointerEventData);
            lastButton = buttons[currentButton];
        }
    }

    private void Start()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        CurrentButton = -1;
    }

    public void OnGrabbed(BasicGrabbable grabbedObj)
    {
        Debug.Log("OnGrabbed");
        ViveColliderButtonEventData viveEventData;
        if (!grabbedObj.grabbedEvent.TryGetViveButtonEventData(out viveEventData)) { return; }
        int currentHand = viveEventData.viveRole.ToRole<HandRole>() == HandRole.RightHand ? 0 : 1;
        if (!GameManager.IsPanelInHand)
        {
            grabbedObj.transform.SetParent(grabbers[currentHand]);
            GameManager.IsPanelInHand = true;
            lastHand = currentHand;
        }
        else
        {
            GameManager.IsPanelInHand = false;
            if (lastHand == currentHand)
            {
                transform.parent = null;
            }
            else
            {
                OnGrabbed(grabbedObj);
            }
        }
    }
    public void OnDrop()
    {
        if (GameManager.IsPanelInHand)
        {
            transform.localPosition = new Vector3(0, 0, 0);
            transform.localRotation = Quaternion.Euler(-70, 180, 0);
            GetComponent<Rigidbody>().isKinematic = true;
        }
        else GetComponent<Rigidbody>().isKinematic = false;
        hands[lastHand].PlayAnimation(GameManager.IsPanelInHand);
    }

    public void OnChangeButton()
    {
        if (GameManager.IsPanelInHand)
        {
            CurrentButton++;
        }
    }

    public void OnButtonPressed(bool press)
    {
        if (GameManager.IsPanelInHand)
        {
            if (press) buttons[CurrentButton].OnPointerClick(pointerEventData);
            else
            {
                EventManager.TriggerEvent("StopCraneEvent");
                buttons[CurrentButton].OnPointerEnter(pointerEventData);
            }
        }
    }
}
