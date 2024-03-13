using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractiveObject : MonoBehaviour, IInteractive
{
    public LayerMask Interactive_Layer;
    public GameObject BtnObj;
    [HideInInspector]public bool isInteractive =false;
    protected virtual void Start()
    {
        SetActiveBtnObj(false);
    }

    public virtual void SetActiveBtnObj(bool isShow)
    {
        if (BtnObj != null)
        {
            BtnObj.gameObject.SetActive(isShow);
        }
    }
    public virtual bool Start_Action() { return true; }
    public virtual bool End_Action() { return true; }
    public virtual bool Interactive() { return true; }

    public virtual bool UnInteractive() { return true; }
}
