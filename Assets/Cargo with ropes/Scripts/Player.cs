using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    private Rigidbody RB;
    public Transform CameraTransform;

    public float Speed;

    public float MouseSens = 100f;
    float xRotation = 0f;

    private void Start()
    {
        RB = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        #region Move

        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(horizontalMove, 0f, verticalMove);

        transform.Translate(move * Speed * Time.fixedDeltaTime);

        #endregion

        #region CameraRotate

        float MouseX = Input.GetAxis("Mouse X") * MouseSens * Time.fixedDeltaTime;
        float MouseY = Input.GetAxis("Mouse Y") * MouseSens * Time.fixedDeltaTime;

        xRotation -= MouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        CameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * MouseX);

        #endregion
    }
}
