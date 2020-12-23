using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Custom3DButtonEventReciever : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IPointerClickHandler
{
    private Material DefaultMaterial;

    [SerializeField] private Material HighlightMaterial;
    [SerializeField] private Material PressedMaterial;

    public bool isLocked = false;

    private void Start()
    {
        DefaultMaterial = GetComponent<Renderer>().material;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isLocked)
        {
            EventManager.TriggerEvent("ButtonDownEvent", name);
            EventManager.TriggerEvent("CustomLog", $"{name} button pressed");
            GetComponent<Renderer>().material = PressedMaterial;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isLocked) GetComponent<Renderer>().material = HighlightMaterial;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isLocked) GetComponent<Renderer>().material = DefaultMaterial;
    }
}
