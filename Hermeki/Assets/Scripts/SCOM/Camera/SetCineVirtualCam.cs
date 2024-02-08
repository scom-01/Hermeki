using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SetCineVirtualCam : MonoBehaviour
{
    public Player Player
    {
        get
        {
            if (GameManager.Inst?.StageManager == null)
                return null;
            return GameManager.Inst.StageManager.player;
        }
    }

    private CinemachineVirtualCamera CVC;
    private void Awake()
    {
        CVC = this.GetComponent<CinemachineVirtualCamera>();
    }
    public void SetFollowPlayer()
    {
        if (Player == null)
            return;

        SetFollow(Player.gameObject);
    }

    public void SetPosZ(float posZ)
    {
        if (CVC == null)
        {
            CVC = this.GetComponent<CinemachineVirtualCamera>();
            if (CVC == null)
                return;
        }

        CVC.transform.position = new Vector3(CVC.transform.position.x, CVC.transform.position.y, posZ);
    }
    public void SetFollow(GameObject gameObject)
    {
        if (gameObject == null)
            return;
        if (gameObject.GetComponent<Unit>() == null)
            return;
        if (CVC == null)
        {
            CVC = this.GetComponent<CinemachineVirtualCamera>();
            if (CVC == null)
                return;
        }
        CVC.Follow = gameObject.transform;
    }
}
