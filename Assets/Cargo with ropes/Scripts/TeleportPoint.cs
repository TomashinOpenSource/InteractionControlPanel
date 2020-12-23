using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TeleportPoint : MonoBehaviour, IPointerClickHandler
{
    private Transform Player;

    private void Start()
    {
        Player = GameObject.Find("ViveRig").transform;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Player.position = new Vector3(transform.position.x, Player.position.y, transform.position.z);
    }
}
