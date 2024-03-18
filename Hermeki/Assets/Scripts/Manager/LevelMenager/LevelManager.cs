using Cinemachine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using TMPro;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    [Header("Unit")]
    [Tooltip("플레이어")]
    public Unit player;
    public Transform StartPos;
    public AssetReferenceGameObject SpawnUnit;

    [Header("Stage")]
    public List<StageController> StageList = new List<StageController>();
    public List<SelectStartLevel> SelectUIList = new List<SelectStartLevel>();
    public bool isPlaying = false;
    [Tooltip("현재 진행중인 스테이지")]
    public int CurrStageIdx = 0;
    [Tooltip("스테이지 난이도")]
    public int StageLevel = 0;
    public int MaxLevel = 5;

    [Header("UI")]
    public Canvas RootCanvas;
    public Canvas SelectLevelCanvas;
    public SPUM_SpriteList SettingSpriteList;
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
            if (SelectUIList != null && SelectUIList.Count > 0)
            {
                SelectStart(0);
            }
        }
    }
    private void Update()
    {
        if (player == null)
            return;

        if(!player.IsAlive && isPlaying)
        {
            isPlaying = false;
            Invoke("GameOver", 2f);
        }
    }
    public void GameStart()
    {
        isPlaying = true;
        Vector3 _Pos = Vector3.zero;
        if (StartPos != null)
        {
            _Pos = StartPos.position;
        }

        if (SpawnUnit != null)
        {
            SpawnUnit.InstantiateAsync().Completed += (AsyncOperationHandle<GameObject> _obj) =>
            {
                player = _obj.Result.GetComponent<Unit>();
                SetUnitSprite();
                if (VirtualCamera != null && player != null)
                {
                    VirtualCamera.Follow = player.transform;
                    VirtualCamera.transform.position = player.transform.position;
                }
            };
        }
    }
    public void GameOver()
    {
        isPlaying = false;
        if (player == null || RootCanvas == null)
            return;

        CurrStageIdx = 0;
        RootCanvas.gameObject.SetActive(true);
        Addressables.ReleaseInstance(player.gameObject);
        player = null;
        if (VirtualCamera != null && player != null)
            VirtualCamera.Follow = null;
        for (int i = 0; i < StageList.Count; i++)
        {
            StageList[i].ResetStage();
        }
    }

    public bool SetUnitSprite()
    {
        if (player == null || SettingSpriteList == null)
            return false;
        player.GetComponentInChildren<SPUM_SpriteList>()?.SetSpriteList(SettingSpriteList);
        player.GetComponentInChildren<SPUM_SpriteList>()?.ResyncData();
        return true;
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

    public StageController CurrStage()
    {
        if (StageList?.Count == 0)
            return null;
        return StageList[CurrStageIdx];
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
            VirtualCamera.Follow = null;
            //VirtualCamera.PreviousStateIsValid = false;
            VirtualCamera.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = null;
            VirtualCamera.transform.position = player.transform.position;
            StartCoroutine(UpdateCameraFrameLater());
        }
        if (!StageList[CurrStageIdx].StartStage())
        {
            return false;
        }
        return true;
    }
    private IEnumerator UpdateCameraFrameLater()
    {
        yield return null;

        if (player != null)
            VirtualCamera.Follow = player.transform;
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
            if (i == idx)
            {
                SelectUIList[idx].isSelect = true;
                SelectUIList[idx].SetStageLevel(SelectUIList[idx].SelectedLevel);
                continue;
            }
            SelectUIList[i].isSelect = false;
        }
    }
    #endregion
}
