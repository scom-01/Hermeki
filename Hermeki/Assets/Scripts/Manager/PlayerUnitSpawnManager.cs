using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitSpawnManager : MonoBehaviour
{
    public GameObject SinglePlayerPrefab;
    public GameObject MultiPlayerPrefab;

    public CinemachineVirtualCamera VirtualCamera;

    private void Start()
    {
        if(SinglePlayerPrefab!=null)
        {
            GameObject obj = Instantiate(SinglePlayerPrefab);
            if (VirtualCamera != null)
            {
                VirtualCamera.Follow = obj.transform;
            }
        }
    }
}
