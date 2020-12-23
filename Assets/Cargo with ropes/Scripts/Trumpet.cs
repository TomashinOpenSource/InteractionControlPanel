using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trumpet : MonoBehaviour
{
    public List<Transform> AntirollPadsTouched = new List<Transform>();

    private bool StartCheck = false;
    private float CheckTimer = 0.1f;
    private float cCheckTimer = 0f;

    public void Check()
    {
        StartCheck = true;
    }

    private void OnCollisionEnter(Collision hit)
    {
        if (hit.transform.CompareTag("AntirollPads"))
        {
            AntirollPadsTouched.Add(hit.transform);
        }
    }

    private void OnCollisionExit(Collision collisionInfo)
    {
        if (collisionInfo.transform.CompareTag("AntirollPads"))
        {
            AntirollPadsTouched.Remove(collisionInfo.transform);
        }
    }



    private void FixedUpdate()
    {
        if (StartCheck)
        {
            if (GetComponent<Rigidbody>().velocity == Vector3.zero && GetComponent<Rigidbody>().angularVelocity == Vector3.zero)
            {
                if (cCheckTimer < CheckTimer) cCheckTimer += Time.fixedDeltaTime;
                else
                {
                    if (AntirollPadsTouched.Count > 2)
                    {
                        EventManager.TriggerEvent("TrumpetPositionChecked", true);
                        Debug.Log("Trumpet position is good");
                    }
                    else
                    {
                        EventManager.TriggerEvent("TrumpetPositionChecked", false);
                        Debug.Log("Trumpet position is bad");
                    }

                    StartCheck = false;
                }
            }
            else if (cCheckTimer != 0f) cCheckTimer = 0f;
        }
    }
}
