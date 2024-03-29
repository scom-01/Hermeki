﻿using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

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
    [Tooltip("현재 진행중인 캐릭터 이름")]
    public string CurrCharacterName;
    [Tooltip("스테이지 난이도")]
    public int StageLevel = 0;
    public int MaxLevel = 5;

    /// <summary>
    /// 캐릭터들의 해금 레벨값, string : 캐릭터이름, int : 캐릭터 해금 레벨
    /// </summary>
    private Dictionary<string, int> CharacterLevel = new Dictionary<string, int>();

    [Header("UI")]
    public Canvas LevelCanvas;
    public InventoryTable InventoryTable;

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
        if (LevelCanvas != null)
        {
            SelectUIList = LevelCanvas.GetComponentsInChildren<SelectStartLevel>().ToList();
            if (SelectUIList != null && SelectUIList.Count > 0)
            {
                SelectStart(0);
            }
        }
    }

    private void OnDisable()
    {
        if (PlayFabManager.Inst == null)
            return;

        //캐릭터 레벨 PlayFab 저장 
        for (int i = 0; i < CharacterLevel.Count; i++)
        {
            PlayFabManager.Inst.CS_SetUserData(CharacterLevel.ElementAt(i).Key, CharacterLevel.ElementAt(i).Value);
        }
    }

    private void Update()
    {
        if (player == null)
            return;

        if (!player.IsAlive && isPlaying)
        {
            isPlaying = false;
            Invoke("GameOver", 2f);
        }
    }


    /// <summary>
    /// Event Play버튼으로 호출
    /// </summary>
    public void GameStart()
    {
        isPlaying = true;
        Vector3 _Pos = Vector3.zero;
        if (StartPos != null)
        {
            _Pos = StartPos.position;
        }

        if (LevelCanvas != null)
            LevelCanvas.enabled = false;

        if (InventoryTable != null)
        {
            InventoryTable.SetState(InventoryState.Close);
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
                if (InventoryTable != null)
                {
                    InventoryTable.unit = player;
                }
            };
        }

        StartStage();
    }
    public void GameOver()
    {
        isPlaying = false;
        if (player == null || LevelCanvas == null)
            return;

        CurrStageIdx = 0;
        if (LevelCanvas != null)
            LevelCanvas.enabled = true;

        if (InventoryTable != null)
            InventoryTable.SetState(InventoryState.Close);

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

    /// <summary>
    /// 현재 스테이지 컨트롤러 return
    /// </summary>
    /// <returns></returns>
    public StageController CurrStage()
    {
        if (StageList?.Count == 0)
            return null;
        return StageList[CurrStageIdx];
    }

    /// <summary>
    /// 다음 스테이지로 이동
    /// </summary>
    /// <param name="player">이동할 플레이어</param>
    /// <returns></returns>
    public bool GoNextStage(Player player)
    {
        if (player == null)
            return false;

        CurrStageIdx++;

        //스테이지 모두 클리어 시
        if (CurrStageIdx >= StageList.Count)
        {
            CurrStageIdx = StageList.Count - 1;
            GameOver();

            SetCharacterLevel(CurrCharacterName, StageLevel + 1);
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

    /// <summary>
    /// 현재 진행 중인 캐릭터 이름 설정
    /// </summary>
    /// <param name="Name"></param>
    /// <returns></returns>
    public bool ChangeName(string Name)
    {
        if (Name == "")
            return false;
        CurrCharacterName = Name;
        return true;
    }

    /// <summary>
    /// 현재 진행 중인 레벨 설정
    /// </summary>
    /// <param name="idx"></param>
    /// <returns></returns>
    public bool ChangeLevel(int idx)
    {
        if (idx >= MaxLevel)
        {
            idx = MaxLevel;
        }
        StageLevel = idx;
        return true;
    }


    /// <summary>
    /// 시작 캐릭터 설정
    /// </summary>
    /// <param name="idx">캐릭터 인덱스</param>
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

    /// <summary>
    /// 캐릭터 레벨 가져오기
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public int GetCharacterLevel(string key)
    {
        if (!CharacterLevel.ContainsKey(key))
        {
            return -1;
        }
        return CharacterLevel[key];
    }

    /// <summary>
    /// 캐릭터 레벨 설정
    /// </summary>
    /// <param name="key">캐릭터 이름</param>
    /// <param name="value">레벨</param>
    /// <returns></returns>
    public bool SetCharacterLevel(string key, int value)
    {
        if (CharacterLevel == null)
            return false;

        if (CharacterLevel.ContainsKey(key))
        {
            CharacterLevel[key] = value;
            foreach (var _char in CharacterLevel)
            {
                print($"{_char.Key} = {_char.Value}");
            }
            return true;
        }
        CharacterLevel.Add(key, value);
        foreach (var _char in CharacterLevel)
        {
            print($"{_char.Key} = {_char.Value}");
        }
        return true;
    }
    #endregion
}
