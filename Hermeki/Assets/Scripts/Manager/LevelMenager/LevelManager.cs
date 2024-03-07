using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviourPunCallbacks
{
    [Header("Unit")]
    [Tooltip("플레이어")]
    public Unit player;
    public GameObject UserObject;
    public Transform StartPos;


    [Header("Photon")]
    public bool isMulti = false;
    public string PhotonUserObjectPath;
    public PhotonView PV;

    [Header("Cam")]
    public CinemachineVirtualCamera VirtualCamera;
    public virtual void Start()
    {
        Vector3 _Pos = StartPos.position;
        if (PlayerPrefs.GetInt("IsMulti") == 1)
            isMulti = true;
        if (isMulti)
        {
            GameObject obj = PhotonNetwork.Instantiate(PhotonUserObjectPath, _Pos, Quaternion.identity);
            obj.name = PhotonNetwork.NickName;
            player = obj.GetComponent<Unit>();
        }
        else
        {
            GameObject obj = Instantiate(UserObject, StartPos);
            player = obj.GetComponent<Unit>();
        }

        if (VirtualCamera != null && player != null)
            VirtualCamera.Follow = player.transform;
    }
}
