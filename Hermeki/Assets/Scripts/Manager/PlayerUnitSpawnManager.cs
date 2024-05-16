using Cinemachine;
using UnityEngine;

public class PlayerUnitSpawnManager : MonoBehaviour
{
    public GameObject SinglePlayerPrefab;
    public GameObject MultiPlayerPrefab;

    public CinemachineVirtualCamera VirtualCamera;

    private void Start()
    {
        if (SinglePlayerPrefab != null)
        {
            GameObject obj = Instantiate(SinglePlayerPrefab);
            
            if (TestManager.Inst != null)
                TestManager.Inst.player = obj.GetComponent<Player>();

            if (VirtualCamera != null)
            {
                VirtualCamera.Follow = obj.transform;
            }
        }
    }
}
