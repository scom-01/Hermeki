using Cinemachine;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviourPunCallbacks
{
    [Header("Unit")]
    [Tooltip("플레이어")]
    public Unit player;
    public GameObject UserObject;
    public Transform StartPos;

    [Header("Cam")]
    public CinemachineVirtualCamera VirtualCamera;
    private void Awake()
    {
        Application.targetFrameRate = 120;
        if (GameManager.Inst != null)
        {
            GameManager.Inst.LevelManager = this;
        }
    }
    public virtual void Start()
    {
        //GameStart();
    }
    public void GameStart()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        Vector3 _Pos = Vector3.zero;
        if (StartPos != null)
        {
            _Pos = StartPos.position;
        }
        
        GameObject obj = Instantiate(UserObject, StartPos);
        player = obj.GetComponent<Unit>();
        
        if (VirtualCamera != null && player != null)
            VirtualCamera.Follow = player.transform;
    }
    //private void SpawnPlayer()
    //{
    //    Vector3 _Pos = StartPos.position;
    //    GameObject obj = PhotonNetwork.Instantiate(PhotonUserObjectPath, _Pos, Quaternion.identity);
    //    obj.name = PhotonNetwork.NickName;
    //    player = obj.GetComponent<Unit>();
    //}

    public void GoLobby()
    {
        SceneManager.LoadSceneAsync(0);
        PhotonNetwork.Disconnect();
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("LeveleManager Room Join");
    }
}
