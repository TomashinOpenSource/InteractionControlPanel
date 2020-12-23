using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAroundObject : MonoBehaviour
{
    [System.Serializable]
    public class CheckAroundObjectRaycastSettings
    {
        public Transform obj = null;
        public bool isActive = true;
        public float checkDistance = 1f;
        public Vector3 direction = Vector3.right;
        [System.NonSerialized]
        public bool alreadyCheck = false;
    }

    [SerializeField]
    private List<CheckAroundObjectRaycastSettings> CheckedSides;

    public bool EnableRaycastVectorInEditor;

    [SerializeField]
    private float CheckTime;

    private float curCheckTime = 0f;
    private int alreadyCheked = 0;

    private bool stopCheck = false;

    private void OnEnable()
    {
        EventManager.StartListening("StartCheckObject", StartCheck);
        EventManager.StartListening("StopCheckObject", StopCheckHandler);
    }

    private void OnDisable()
    {
        EventManager.StopListening("StartCheckObject", StartCheck);
        EventManager.StopListening("StopCheckObject", StopCheckHandler);
    }

    public void StartCheck()
    {
        foreach (CheckAroundObjectRaycastSettings side in CheckedSides)
        {
            if (side.isActive)
                alreadyCheked++;
        }
        stopCheck = false;
        Debug.Log("CheckStart");
    }

    private void StopCheckHandler()
    {
        stopCheck = true;
    }

    public void StopCheck()
    {
        foreach(CheckAroundObjectRaycastSettings side in CheckedSides)
        {
            side.alreadyCheck = false;
        }
        curCheckTime = 0;
        alreadyCheked = 0;
        Debug.Log("CheckStop");
    }

    #region Gizmos for editor

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (!EnableRaycastVectorInEditor) return;

        Gizmos.color = Color.cyan;

        foreach (CheckAroundObjectRaycastSettings side in CheckedSides)
        {
            if (side.isActive)
            {
                Gizmos.DrawLine(side.obj.position, side.obj.position + side.obj.TransformDirection(side.direction.normalized) * side.checkDistance);
            }
        }

        Gizmos.color = Color.white;
#endif
    }

    #endregion

    #region Work with class by outside script

    public void SetSideActive(CheckAroundObjectRaycastSettings _side, bool status)
    {
        foreach (CheckAroundObjectRaycastSettings side in CheckedSides)
        {
            if (side == _side)
            {
                if (status != side.isActive)
                {
                    if (status) alreadyCheked++;
                    else alreadyCheked--;

                    side.isActive = status;
                }
                break;
            }
        }
    }

    public void SetSideChecked(CheckAroundObjectRaycastSettings _side, bool status)
    {
        foreach (CheckAroundObjectRaycastSettings side in CheckedSides)
        {
            if (side == _side)
            {
                if (status != side.alreadyCheck)
                {
                    if (status) alreadyCheked--;
                    else alreadyCheked++;

                    side.alreadyCheck = status;
                }
                break;
            }
        }
    }

    public CheckAroundObjectRaycastSettings GetSideRaycastElement(Transform t)
    {
        foreach (CheckAroundObjectRaycastSettings _side in CheckedSides)
        {
            if (_side.obj == t)
                return _side;
        }
        return null;
    }

    #endregion

    private void FixedUpdate()
    {
        if (alreadyCheked != 0)
        {
            foreach (CheckAroundObjectRaycastSettings side in CheckedSides)
            {
                if (side.isActive && !side.alreadyCheck)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(side.obj.position, side.obj.TransformDirection(side.direction), out hit, side.checkDistance))
                    {
                        if (hit.transform.CompareTag("Player"))
                        {
                            side.alreadyCheck = true;
                            alreadyCheked--;
                            Debug.Log("Side checked!");
                        }
                    }
                }
            }
            if (curCheckTime < CheckTime) curCheckTime += Time.fixedDeltaTime;
        }

        if (stopCheck)
        {
            if (alreadyCheked > 0)
            {
                EventManager.TriggerEvent("CustomLogError", $"Объект {transform.parent.name} осмотрен не со всех сторон");
            }
            if (curCheckTime < CheckTime)
            {
                EventManager.TriggerEvent("CustomLogError", $"Объект {transform.parent.name} осмотрен недостаточно долго");
            }
            if (alreadyCheked == 0 && curCheckTime >= CheckTime)
            {
                EventManager.TriggerEvent("CustomLog", $"Объект {transform.parent.name} успешно осмотрен");
            }
            stopCheck = false;
            StopCheck();
        }

        #region Debug keys

        if (Input.GetKeyDown(KeyCode.KeypadPlus)) StartCheck();
        if (Input.GetKeyDown(KeyCode.KeypadMinus)) StopCheckHandler();

        #endregion
    }
}
