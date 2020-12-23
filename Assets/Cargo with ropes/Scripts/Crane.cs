using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crane : MonoBehaviour
{
    private bool MoveNorth = false;
    private bool MoveSouth = false;
    private bool MoveOst = false;
    private bool MoveWest = false;
    private bool MoveUp = false;
    private bool MoveDown = false;

    public float NorthSpeed;
    public float SouthSpeed;
    public float WestSpeed;
    public float OstSpeed;
    public float UpSpeed;
    public float DownSpeed;

    public float MaxNorth;
    public float MaxSouth;
    public float MaxOst;
    public float MaxWest;
    public float MaxUp;
    public float MaxDown;

    public Transform HookTransform;
    public Transform WinchTransform;
    public Transform CraneTransform;
    public Transform BobbinTransform;
    public Transform HookWireTransform;

    public Transform BobbinAS;
    public Transform CraneAS;
    public Transform WinchAS;
    public Transform HookAS;

    private bool HookReleased = false;
    private Transform HookedTrumpet;
    public float HookedDistance;
    public Transform HookCheckTransform;

    void OnEnable()
    {
        EventManager.StartListening("ButtonDownEvent", PanelButtonDownHandler);
        EventManager.StartListening("StopCraneEvent", StopHandler);
        EventManager.StartListening("HookEvent", HookHandler);
    }

    void OnDisable()
    {
        EventManager.StopListening("ButtonDownEvent", PanelButtonDownHandler);
        EventManager.StopListening("StopCraneEvent", StopHandler);
        EventManager.StopListening("HookEvent", HookHandler);
    }

    private void PanelButtonDownHandler(string _buttonName)
    {
        if (_buttonName == "ControlPanelNorthButton")
        {
            MoveNorth = true;
            CraneAS.GetComponent<AudioSource>().Play();
        }
        else if (_buttonName == "ControlPanelSouthButton")
        {
            MoveSouth = true;
            CraneAS.GetComponent<AudioSource>().Play();
        }
        else if (_buttonName == "ControlPanelWestButton")
        {
            MoveWest = true;
            CraneAS.GetComponent<AudioSource>().Play();
        }
        else if (_buttonName == "ControlPanelOstButton")
        {
            MoveOst = true;
            CraneAS.GetComponent<AudioSource>().Play();
        }
        else if (_buttonName == "ControlPanelUpButton")
        {
            MoveUp = true;
            CraneAS.GetComponent<AudioSource>().Play();
        }
        else if (_buttonName == "ControlPanelDownButton")
        {
            MoveDown = true;
            CraneAS.GetComponent<AudioSource>().Play();
        }
    }

    private void StopHandler()
    {
        MoveNorth = false;
        MoveSouth = false;
        MoveOst = false;
        MoveWest = false;
        MoveUp = false;
        MoveDown = false;
        BobbinAS.GetComponent<AudioSource>().Stop();
        CraneAS.GetComponent<AudioSource>().Stop();
        WinchAS.GetComponent<AudioSource>().Stop();
    }

    private void HookHandler()
    {
        if (HookReleased)
        {
            if (HookedTrumpet != null)
            {
                HookAS.GetComponent<AudioSource>().Play();
                HookedTrumpet.GetComponent<Trumpet>().Check();
                HookedTrumpet.GetComponent<Rigidbody>().useGravity = true;
                HookedTrumpet.SetParent(null);
                HookedTrumpet = null;
                HookReleased = false;
                Debug.Log("Cargo released");
            }
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(HookCheckTransform.position, Vector3.down, out hit, HookedDistance))
            {
                var selection = hit.transform;
                if (selection.CompareTag("Trumpet"))
                {
                    HookAS.GetComponent<AudioSource>().Play();
                    selection.SetParent(HookTransform);
                    HookedTrumpet = selection;
                    HookedTrumpet.GetComponent<Rigidbody>().useGravity = false;
                    HookReleased = true;
                    EventManager.TriggerEvent("CargoHooked");
                    Debug.Log("Cargo hooked");
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (MoveNorth && CraneTransform.position.z > MaxNorth)
            CraneTransform.Translate(Vector3.back * NorthSpeed);

        if (MoveSouth && CraneTransform.position.z < MaxSouth)
            CraneTransform.Translate(Vector3.forward * SouthSpeed);

        if (MoveWest && WinchTransform.localPosition.x < MaxWest)
            WinchTransform.Translate(Vector3.right * WestSpeed);

        if (MoveOst && WinchTransform.localPosition.x > MaxOst)
            WinchTransform.Translate(Vector3.left * OstSpeed);

        if (MoveUp && HookTransform.localPosition.y < MaxUp)
        {
            HookTransform.Translate(Vector3.up * UpSpeed);
            BobbinTransform.Rotate(Vector3.right, Space.Self);
            HookWireTransform.localScale -= Vector3.up * UpSpeed / 5;
        }

        if (MoveDown && HookTransform.localPosition.y > MaxDown)
        {
            HookTransform.Translate(Vector3.down * DownSpeed);
            BobbinTransform.Rotate(Vector3.left, Space.Self);
            HookWireTransform.localScale += Vector3.up * UpSpeed / 5;
        }
    }
}