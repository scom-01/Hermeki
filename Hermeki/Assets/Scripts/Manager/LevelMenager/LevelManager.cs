using Cinemachine;
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
    /// <summary>
    /// 현재 진행 중인 캐릭터 UI 클래스
    /// </summary>
    public SelectStartLevel CurrSelectUI;
    public bool isPlaying = false;
    [Tooltip("현재 진행중인 스테이지")]
    public int CurrStageIdx = 0;
    private int MaxIdx = 0;
    [Tooltip("현재 진행중인 캐릭터 이름")]
    public string CurrCharacterName;
    [Tooltip("스테이지 난이도")]
    public int StageLevel = 0;
    public int MaxLevel = 5;

    //-------Observer
    public List<ILevelManagerObserver> Observers = new List<ILevelManagerObserver>();

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
        Application.targetFrameRate = 250;
        if (GameManager.Inst != null)
        {
            GameManager.Inst.LevelManager = this;
        }
    }
    public virtual void Start()
    {
        StageList = GetComponentsInChildren<StageController>().ToList();
        MaxIdx = StageList.Count;
        if (LevelCanvas != null)
        {
            LevelCanvas.enabled = true;
            SelectUIList = LevelCanvas.GetComponentsInChildren<SelectStartLevel>().ToList();
            if (SelectUIList != null && SelectUIList.Count > 0)
            {
                SelectStart(0);
            }
        }
    }

    private void OnEnable()
    {
		if (PlayFabManager.Inst == null)
		{
			SceneManager.LoadSceneAsync("Title");
			return;
		}
	}

    private void OnDisable()
    {        
        if (PlayFabManager.Inst == null)
        {
            return;
        }


        //캐릭터 레벨 PlayFab 저장 
        for (int i = 0; i < CharacterLevel.Count; i++)
        {
            PlayFabManager.Inst?.CS_SetUserData(CharacterLevel.ElementAt(i).Key, CharacterLevel.ElementAt(i).Value);
        }
    }

    private void Update()
    {
        if (player == null)
            return;

        if (!player.IsAlive && isPlaying)
        {
            isPlaying = false;
            PostProcessingManager.Inst?.SetVignette(0, 1f);
            Invoke(nameof(GameOver), 2f);
        }
    }


    /// <summary>
    /// Event Play버튼으로 호출
    /// </summary>
    public void GameStart()
    {
        isPlaying = true;
        Vector3 _Pos = Vector3.zero;
        _Pos = CurrStage().StartPos.position;
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
                StartStage();
                if (InventoryTable != null)
                {
                    InventoryTable.unit = player;
                }
            };
        }

        PostProcessingManager.Inst?.SetVignette(1f, 0);
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
        SceneManager.LoadSceneAsync("Title");
    }


    #region Stage Func
    public bool StartStage()
    {
        if (player == null)
            return false;

        player.transform.position = CurrStage().StartPos.position;
        if (VirtualCamera != null && player != null)
        {
            VirtualCamera.Follow = player.transform;
            VirtualCamera.transform.position = player.transform.position;
        }

        if (StageList.Count == 0)
            return false;

        if (!StageList[CurrStageIdx].StartStage())
        {
            return false;
        }

        return true;
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
        ChangeStageIdx(CurrStageIdx);
        //스테이지 모두 클리어 시
        if (CurrStageIdx >= StageList.Count)
        {
            CurrStageIdx = StageList.Count - 1;
            GameOver();

            //현재 최대 레벨 클리어 시
            if (GetCharacterLevel(CurrCharacterName) < StageLevel + 1)
            {
                SetCharacterLevel(CurrCharacterName, StageLevel + 1);
                CurrSelectUI.SetMaxLevel(StageLevel + 1);
            }
            return false;
        }
        if (!StartStage())
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

        notifyStageLevelObservers(StageLevel);
        return true;
    }

    public bool ChangeStageIdx(int idx)
    {
        if (idx >= MaxIdx)
        {
            idx = MaxIdx;
        }

        CurrStageIdx = idx;

        notifyStageIdxObservers(CurrStageIdx);
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
                CurrSelectUI = SelectUIList[idx];
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

    #region Observer
    /// <summary>
    /// 옵저버 리스트에 추가
    /// </summary>
    /// <param name="_observer"></param>
    /// <returns></returns>
    public bool registerObserver(ILevelManagerObserver _observer)
    {
        if (Observers == null)
            return false;
        Observers.Add(_observer);
        return true;
    }

    /// <summary>
    /// 옵저버 리스트에서 삭제
    /// </summary>
    /// <param name="_observer"></param>
    /// <returns></returns>
    public bool removeObserver(ILevelManagerObserver _observer)
    {
        if (Observers == null)
            return false;
        Observers.Remove(_observer);
        return true;
    }

    /// <summary>
    /// 옵저버들에게 스테이지 레벨 변경 사항 알림
    /// </summary>
    /// <param name="_value"></param>
    public void notifyStageLevelObservers(int _value)
    {
        foreach (var _observer in Observers)
        {
            _observer.UpdateStageLevel(_value);
        }
    }
    /// <summary>
    /// 옵저버들에게 스테이지 Idx 변경 사항 알림
    /// </summary>
    /// <param name="_value"></param>
    public void notifyStageIdxObservers(int _value)
    {
        foreach (var _observer in Observers)
        {
            _observer.UpdateStageIdx(_value);
        }
    }
    #endregion
}
