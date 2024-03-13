using Cinemachine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Unit")]
    [Tooltip("플레이어")]
    public Unit player;
    public GameObject UserObject;
    public Transform StartPos;

    public List<StageController> StageList = new List<StageController>();
    public int CurrStageIdx = 0;

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
        StageList = GetComponentsInChildren<StageController>().ToList();
    }
    public void GameStart()
    {
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

    public void GoLobby()
    {
        SceneManager.LoadSceneAsync(0);
        
    }

    public void StartStage()
    {
        if (StageList.Count == 0)
            return;

        if (!StageList[CurrStageIdx].StartStage())
        {
            return;
        }

        return;
    }
    public bool GoNextStage(Player player)
    {
        if (player == null)
            return false;

        CurrStageIdx++;
        if (CurrStageIdx >= StageList.Count)
        {
            CurrStageIdx = StageList.Count - 1;
            return false;
        }
        player.transform.position = StageList[CurrStageIdx].StartPos.position;
        if (!StageList[CurrStageIdx].StartStage())
        {
            return false;
        }
        return true;
    }
}
