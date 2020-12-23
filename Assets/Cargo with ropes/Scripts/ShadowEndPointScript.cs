using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowEndPointScript : MonoBehaviour
{
    public Transform ShadowAnimationTrumpet;

    private bool show = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Trumpet"))
        {
            Show();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Trumpet"))
        {
            Hide();
        }
    }

    private void OnEnable()
    {
        EventManager.StartListening("TrumpetPositionChecked", CargoOnCraneHandler);
        EventManager.StartListening("CargoHooked", CargoHooked);
    }

    private void OnDisable()
    {
        EventManager.StopListening("TrumpetPositionChecked", CargoOnCraneHandler);
        EventManager.StopListening("CargoHooked", CargoHooked);
    }

    private void CargoOnCraneHandler(bool checkResult)
    {
        Hide();
        if (checkResult)
            ShadowAnimationTrumpet.gameObject.SetActive(false);
    }

    private void CargoHooked()
    {
        ShadowAnimationTrumpet.gameObject.SetActive(true);
    }

    private void Show()
    {
        if (!show)
        {
            ShadowAnimationTrumpet.gameObject.SetActive(false);

            GetComponent<Renderer>().enabled = true;
            show = true;
        }
    }

    private void Hide()
    {
        if (show)
        {
            ShadowAnimationTrumpet.gameObject.SetActive(true);

            GetComponent<Renderer>().enabled = false;
            show = false;
        }
    }
}
