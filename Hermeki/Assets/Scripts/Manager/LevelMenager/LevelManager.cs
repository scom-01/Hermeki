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

    [Header("Stage")]
    public List<StageController> StageList = new List<StageController>();
    public List<SelectStartLevel> SelectUIList = new List<SelectStartLevel>();
    
    [Tooltip("현재 진행중인 스테이지")]
    public int CurrStageIdx = 0;
    [Tooltip("스테이지 난이도")]
    public int StageLevel = 0;
    public int MaxLevel = 5;

    [Header("UI")]
    public Canvas SelectLevelCanvas;
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
        if (SelectLevelCanvas != null)
        {
            SelectUIList = SelectLevelCanvas.GetComponentsInChildren<SelectStartLevel>().ToList();
        }
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
    #region Stage Func
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
        if (VirtualCamera != null)
        {
            VirtualCamera.transform.position = player.transform.position;
        }
        if (!StageList[CurrStageIdx].StartStage())
        {
            return false;
        }
        return true;
    }
    public bool ChangeLevel(int idx)
    {
        if (idx >= MaxLevel) 
        {
            idx = MaxLevel;            
        }
        StageLevel = idx;
        return true;
    }

    public void SelectStart(int idx)
    {
        for (int i = 0; i < SelectUIList.Count; i++)
        {
            if(i == idx)
            {
                SelectUIList[idx].isSelect = true;
                continue;
            }
            SelectUIList[i].isSelect = false;
        }
    }
    #endregion
}
