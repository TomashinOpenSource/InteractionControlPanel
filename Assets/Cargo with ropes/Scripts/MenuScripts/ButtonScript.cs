using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    [SerializeField]
    private RectTransform Text;

    private void Start()
    {
        SetTextScaleToNormal();
    }

    private void SetTextScaleToNormal()
    {
        Text.localScale = new Vector3(1 / transform.localScale.x, 1 / transform.localScale.y, 1 / transform.localScale.z);
    }

    public void SetText(string value)
    {
        Text.GetComponent<TextMesh>().text = value;
    }
}
