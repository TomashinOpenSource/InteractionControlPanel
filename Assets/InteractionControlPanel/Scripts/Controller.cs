using HTC.UnityPlugin.Vive;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [Header("Джойстики для дочерности")]
    public Transform[] grabbers;
    private int lastHand = -1;

    [Header("Пульт")]
    public Transform[] buttons = new Transform[6];
    public Transform finger;
    private float fingerStartZ;
    private int currentButton;

    public int CurrentButton 
    { 
        get => currentButton; 
        set
        {
            if (value == buttons.Length) value = 0;
            if (value < 0) value = buttons.Length - 1;
            currentButton = value;
            OnGripDrop();
            finger.localPosition = buttons[value].localPosition + Vector3.forward * fingerStartZ;
        }
    }

    private void Start()
    {
        fingerStartZ = finger.localPosition.z;
        finger.gameObject.SetActive(GameManager.IsPanelInHand);
        CurrentButton = 0;
        //StartCoroutine(MoveFinger());
    }

    public void OnGrabbed(BasicGrabbable grabbedObj)
    {
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
        finger.gameObject.SetActive(GameManager.IsPanelInHand);
    }
    public void OnDrop()
    {
        if (GameManager.IsPanelInHand)
        {
            transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    public void OnPadStickPressed()
    {
        if (GameManager.IsPanelInHand)
        {
            CurrentButton++;
        }
    }

    public void OnGripPressed()
    {
        if (GameManager.IsPanelInHand)
        {
            finger.localPosition -= Vector3.forward * fingerStartZ;
        }
    }
    public void OnGripDrop()
    {
        if (GameManager.IsPanelInHand)
        {
            finger.localPosition += Vector3.forward * fingerStartZ;
        }
    }

    private IEnumerator MoveFinger()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            CurrentButton++;
        }
    }
}
