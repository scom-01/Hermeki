using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestManager : Singleton<TestManager>
{
    public GameObject playerObj;
    public Player player;
    public CinemachineVirtualCamera VirtualCamera;

    protected override void Awake()
    {
        base.Awake();
        VirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
    }
    // Update is called once per frame
    void Update()
    {        
        if (Keyboard.current[Key.R].wasPressedThisFrame)
        {
            respawn();
        }
    }

    void respawn()
    {
        if (player.IsAlive)
        {
            player.Core.CoreDeath.Die();
            var obj = player.gameObject;
            Destroy(obj, 1f);
            player = null;
        }

        if (playerObj != null)
        {
            player = Instantiate(playerObj).GetComponent<Player>();
            VirtualCamera.Follow = player.gameObject.transform;
        }
    }
}