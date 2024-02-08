using System;
using System.Collections.Generic;
using System.Linq;
using SCOM.Manager;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Settings;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst
    {
        get
        {
            if (_Inst == null)
            {
                _Inst = FindObjectOfType(typeof(GameManager)) as GameManager;
                if (_Inst == null)
                {
                    Debug.Log("no Singleton obj");
                }
                else
                {
                    DontDestroyOnLoad(_Inst.gameObject);
                }
            }
            return _Inst;
        }
    }
    private static GameManager _Inst = null;

    /// <summary>
    /// true = pause
    /// </summary>
    [HideInInspector] public bool isPause = false;
    public PlayerInputHandler InputHandler
    {
        get
        {
            if (_inputHandler == null)
            {
                _inputHandler = this.GetComponent<PlayerInputHandler>();
            }

            return _inputHandler;
        }
    }
    private PlayerInputHandler _inputHandler;

    public StageManager StageManager
    {
        get
        {
            if (Inst == null)
                return null;

            if (_stageManager == null)
                return null;

            return _stageManager;
        }
        set
        {
            _stageManager = value;
        }
    }
    private StageManager _stageManager = null;

    [Header("----UI----")]
    //public MainUIManager MainUI;
    //public SubUIManager SubUI;
    //public CfgUIManager CfgUI;
    //public ReforgingUIManager ReforgingUI;
    //public ResultUIManager ResultUI;
    public Transform DamageUI;
    //public CutSceneManagerUI CutSceneUI;
    //public EffectTextUI EffectTextUI;
    //public PlayTimeManagerUI PlayTimeUI;
    public PlayableDirector PlayerDieCutScene;

    public UI_State Old_UIState, Curr_UIState = UI_State.GamePlay;
    /// <summary>
    /// 플레이타임
    /// </summary>
    public float PlayTime;

    public GameObject LastSelectedObject;
    /// <summary>
    /// Scene 이름으로 Scene Number를 알아내기 위한 Scene List
    /// </summary>
    public List<string> SceneNameList
    {
        get
        {
            if (m_sceneNameList.Count < SceneManager.sceneCountInBuildSettings)
            {
                for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
                {
                    m_sceneNameList.Add(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)));
                }
            }
            return m_sceneNameList;
        }
    }
    private List<string> m_sceneNameList = new List<string>();
    public event Action SaveAction;

    private void Awake()
    {
        if (_Inst)
        {
            var managers = Resources.FindObjectsOfTypeAll(typeof(GameManager));
            for (int i = 0; i < managers.Length; i++)
            {
                Debug.Log($"{managers[i]} = {i}");
                if (i > 0)
                {
                    Destroy(managers[i].GameObject());
                }
            }
            return;
        }

        LocalizationSettings.StringDatabase.MissingTranslationState = MissingTranslationBehavior.PrintWarning;

        _Inst = this;
        DontDestroyOnLoad(this.gameObject);
        GraphicsSettings.useScriptableRenderPipelineBatching = true;
        //if (MainUI == null)
        //    MainUI = this.GetComponentInChildren<MainUIManager>();

        //if (SubUI == null)
        //    SubUI = this.GetComponentInChildren<SubUIManager>();

        //if (CfgUI == null)
        //    CfgUI = this.GetComponentInChildren<CfgUIManager>();

        //if (ReforgingUI == null)
        //    ReforgingUI = this.GetComponentInChildren<ReforgingUIManager>();

        //if (DamageUI == null)
        //    DamageUI = this.GetComponentInChildren<DamageUIManager>();

        //if (ResultUI == null)
        //    ResultUI = this.GetComponentInChildren<ResultUIManager>();

        //if (CutSceneUI == null)
        //    CutSceneUI = this.GetComponentInChildren<CutSceneManagerUI>();

        //if (EffectTextUI == null)
        //    EffectTextUI = this.GetComponentInChildren<EffectTextUI>();

        //if (PlayTimeUI == null)
        //    PlayTimeUI = this.GetComponentInChildren<PlayTimeManagerUI>();

        if (PlayerDieCutScene == null)
            PlayerDieCutScene = this.GetComponentInChildren<PlayableDirector>();
    }

    private void Update()
    {
        if (StageManager != null && InputHandler.playerInput.currentActionMap == InputHandler.playerInput.actions.FindActionMap("GamePlay"))
        {
            PlayTime += Time.deltaTime;
        }

#if UNITY_EDITOR
        if (InputHandler.ESCInput)
        {
            //사망 시엔 ResultUI의 Title누르는 것 외엔 작동하지않도록
        }
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            LastSelectedObject = EventSystem.current.currentSelectedGameObject;
            Debug.Log("currentSelectedGameObject = " + EventSystem.current.currentSelectedGameObject.name);
        }
        else
        {
            Debug.Log("CurrentSelectedGameObject = null");
        }
#endif
    }
    private void Start()
    {
        //if (DataManager.Inst == null)
        //    return;

        //SetSaveData();

        ////해당 함수들은 GameManager에서 참조할 변수들이 있어 GameManager에서 선언
        //DataManager.Inst.UserKeySettingLoad();
        //DataManager.Inst.PlayerCfgBGMLoad();
        //DataManager.Inst.PlayerCfgSFXLoad();
        //DataManager.Inst.PlayerCfgQualityLoad();
        //DataManager.Inst.PlayerCfgLanguageLoad();

        //DeafultData는 GameManager Start()시에 호출되어야한다.(ex.SkipCutSceneList)
        //if (DataManager.Inst.JSON_DataParsing.Json_DefaultDataParsing())
        //{
        //    DataManager.Inst?.PlayerUnlockItem();
        //}
    }

    public void CheckPause(InputEnum inputEnum, bool Init = false)
    {
        if (!isPause)
            Pause(inputEnum, Init);
        else
            Continue(inputEnum);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        isPause = true;
    }
    private void Pause(InputEnum inputEnum, bool Init = false)
    {
        Time.timeScale = 0f;
        //switch (inputEnum)
        //{
        //    //InventoryOpen
        //    case InputEnum.UI:
        //        ChangeUI(UI_State.Inventory, Init);
        //        break;
        //    case InputEnum.Cfg:
        //        ChangeUI(UI_State.Cfg, Init);
        //        break;
        //    case InputEnum.CutScene:
        //        ChangeUI(UI_State.CutScene, Init);
        //        break;
        //}
        isPause = true;
    }
    private void Continue(InputEnum inputEnum)
    {
        if (!isPause)
            return;

        Time.timeScale = 1f;
        //ChangeUI(UI_State.GamePlay);
        //var _TitleManager = FindObjectOfType(typeof(TitleManager)) as TitleManager;
        //if (_TitleManager != null)
        //{
        //    if (_TitleManager.buttons.Count > 0)
        //        GameManager.Inst.SetSelectedObject(_TitleManager.buttons[0].gameObject);
        //}
        isPause = false;
    }

    //UI Button On Click
    public void Continue()
    {
        Time.timeScale = 1f;

        //ChangeUI(UI_State.GamePlay);

        //var _TitleManager = FindObjectOfType(typeof(TitleManager)) as TitleManager;
        //if (_TitleManager != null)
        //{
        //    if (_TitleManager.buttons.Count > 0)
        //        GameManager.Inst.SetSelectedObject(_TitleManager.buttons[0].gameObject);
        //}
        if (StageManager != null)
            InputHandler.playerInput.currentActionMap = InputHandler.playerInput.actions.FindActionMap(InputEnum.GamePlay.ToString());

        isPause = false;
    }

    #region UI

    //public void ChangeUI(UI_State ui, bool Init = false)
    //{
    //    var _ui = ui;
    //    if (Curr_UIState == UI_State.Cfg && Curr_UIState != Old_UIState)
    //    {
    //        _ui = Old_UIState;
    //    }
    //    Old_UIState = Curr_UIState;
    //    Debug.Log($"Switch UI {_ui}");
    //    UI_Init();

    //    if (Init)
    //        return;

    //    Curr_UIState = ui;
    //    Application.targetFrameRate = 144;
    //    CursorUnLock();
    //    switch (_ui)
    //    {
    //        case UI_State.GamePlay:
    //            CursorLock();
    //            InputHandler.ChangeCurrentActionMap(InputEnum.GamePlay, false);
    //            if (StageManager != null)
    //            {
    //                PlayTimeUI.Canvas.enabled = true;
    //            }
    //            SubUI.InventorySubUI.SetInventoryState(InventoryUI_State.Put);
    //            EventSystemSetSelectedNull();
    //            if (StageManager != null)
    //                MainUI.Canvas.enabled = true;
    //            else
    //                MainUI.Canvas.enabled = false;
    //            break;
    //        case UI_State.Inventory:
    //            PlayTimeUI.Canvas.enabled = true;
    //            SubUI.InventorySubUI.animator?.GetCurrentAnimatorClipInfo(0)[0].clip.SampleAnimation(ResultUI.animator.gameObject, 0);
    //            SubUI.InventorySubUI.animator?.Play("Action", -1, 0f);
    //            SubUI.InventorySubUI.Canvas.enabled = true;

    //            if (SubUI.InventorySubUI.InventoryItems.CurrentSelectItem == null)
    //            {
    //                GameManager.Inst.SetSelectedObject(SubUI.InventorySubUI.InventoryItems.Items[0].gameObject);
    //            }
    //            else
    //            {
    //                GameManager.Inst.SetSelectedObject(SubUI.InventorySubUI.InventoryItems.CurrentSelectItem.gameObject);
    //            }
    //            if (StageManager != null)
    //            {
    //                SubUI.InventorySubUI.EquipWeapon?.SetWeaponCommandData(StageManager.player.Inventory.weaponData.weaponCommandDataSO);
    //            }

    //            break;
    //        case UI_State.Reforging:
    //            SubUI.InventorySubUI.SetInventoryState(InventoryUI_State.Put);
    //            ReforgingUI.EnabledChildrensCanvas(true);
    //            ReforgingUI.Canvas.enabled = true;
    //            ReforgingUI.animator?.Play("Action", -1, 0f);
    //            GameManager.Inst.SetSelectedObject(ReforgingUI.equipWeapon.gameObject);
    //            break;
    //        case UI_State.Cfg:
    //            if (StageManager != null)
    //            {
    //                PlayTimeUI.Canvas.enabled = true;
    //            }

    //            CfgUI.Canvas.enabled = true;
    //            CfgUI.animator?.Play("Action", -1, 0f);
    //            GameManager.Inst.SetSelectedObject(CfgUI.ConfigPanelUI.cfgBtns[0].gameObject);
    //            break;
    //        case UI_State.CutScene:
    //            CursorLock();
    //            InputHandler.ChangeCurrentActionMap(InputEnum.CutScene, false);
    //            SubUI.InventorySubUI.SetInventoryState(InventoryUI_State.Put);
    //            CutSceneUI.Canvas.enabled = true;
    //            break;
    //        case UI_State.Result:
    //            ResultUI.animator?.GetCurrentAnimatorClipInfo(0)[0].clip.SampleAnimation(ResultUI.animator.gameObject, 0);
    //            ResultUI.animator?.Play("Action", -1, 0f);
    //            ResultUI.Canvas.enabled = true;
    //            GameManager.Inst.SetSelectedObject(ResultUI.GoTitleBtn.gameObject);
    //            break;
    //        case UI_State.Loading:
    //            CursorLock();
    //            SubUI.InventorySubUI.SetInventoryState(InventoryUI_State.Put);
    //            break;
    //    }
    //}

    //private void UI_Init()
    //{
    //    MainUI.Canvas.enabled = false;
    //    if (CfgUI.ConfigPanelUI.cfgBtns.Length > 0)
    //    {
    //        for (int i = 0; i < CfgUI.ConfigPanelUI.cfgBtns.Length; i++)
    //        {
    //            if (CfgUI.ConfigPanelUI.cfgBtns[i].ActiveUI != null)
    //                CfgUI.ConfigPanelUI.cfgBtns[i].ActiveUI.GetComponent<SettingUI>().Canvas.enabled = false;
    //        }
    //    }
    //    CfgUI.Canvas.enabled = false;
    //    SubUI.InventorySubUI.Canvas.enabled = false;
    //    SubUI.DetailSubUI.Canvas.enabled = false;
    //    ResultUI.Canvas.enabled = false;
    //    ReforgingUI.EnabledChildrensCanvas(false);
    //    ReforgingUI.Canvas.enabled = false;
    //    CutSceneUI.Canvas.enabled = false;
    //    EffectTextUI.Canvas.enabled = false;
    //    PlayTimeUI.Canvas.enabled = false;
    //}

    /// <summary>
    /// Inspector EventTrigger 에서 ChangeUI를 사용하기 위함
    /// 1. EventTrigger를 사용할 Object에 ChangeUIPanel.cs를 AddComponent한다
    /// 2. ChangeUIPanel의 UI_State를 사용하고자 하는 UI_State로 변경한다.
    /// 3. EventTrigger에 GameManager의 ChangeUI(ChangeUIPanel panel)를 넣고 1.에 Add한 ChangeUIPanel을 할당한다.
    /// </summary>
    /// <param name="panel"></param>
    //public void ChangeUI(ChangeUIPanel panel)
    //{
    //    ChangeUI(panel.UI_State);
    //}

    public void EventSystemLastSelected()
    {
        if (LastSelectedObject != null)
        {
            Debug.Log($"Set LastSelectedObject = {LastSelectedObject?.name}");
        }
        EventSystem.current.SetSelectedGameObject(LastSelectedObject);
    }
    public void EventSystemSetSelectedNull()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
    public void SetSelectedObject(GameObject obj)
    {
        if (obj != null)
        {
            LastSelectedObject = EventSystem.current.currentSelectedGameObject;
            EventSystem.current.SetSelectedGameObject(obj);
        }
        else
        {
            EventSystemLastSelected();
        }
    }
    #endregion


    //public void LoadData()
    //{
    //    if (DataManager.Inst == null)
    //        return;

    //    if (GameManager.Inst.StageManager == null)
    //    {
    //        return;
    //    }

    //    if (DataManager.Inst.JSON_DataParsing.Json_Parsing())
    //    {
    //        DataManager.Inst?.SetLockItemList();
    //        DataManager.Inst?.LoadPlayTime();
    //        DataManager.Inst?.LoadBuffs(StageManager.player.GetComponent<BuffSystem>());
    //        DataManager.Inst?.PlayerInventoryDataLoad(StageManager.player.Inventory);
    //        DataManager.Inst?.PlayerCurrHealthLoad(StageManager.player.Core.CoreUnitStats);
    //    }
    //}
    //public void SaveData()
    //{
    //    SaveAction?.Invoke();
    //    DataManager.Inst?.UnlockItemList.Clear();
    //}

    //public void Data_Save()
    //{
    //    if (DataManager.Inst == null)
    //        return;

    //    if (StageManager == null)
    //    {
    //        return;
    //    }
    //    DataManager.Inst.PlayerInventoryDataSave(
    //        GameManager.Inst.StageManager.player.Inventory.Weapon,
    //        GameManager.Inst.StageManager.player.Inventory.Items);
    //    DataManager.Inst?.SaveBuffs(GameManager.Inst.StageManager.player.GetComponent<BuffSystem>());
    //    DataManager.Inst?.PlayerCurrHealthSave(
    //        (int)GameManager.Inst.StageManager.player.Core.CoreUnitStats.CurrentHealth);

    //    DataManager.Inst?.JSON_DataParsing.JSON_SceneDataSave();
    //    DataManager.Inst?.JSON_DataParsing.JSON_DefaultDataSave();
    //}

    private void CursorLock()
    {
        //Debug.Log("Cursor Lock");
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    private void CursorUnLock()
    {
        //Debug.Log("Cursor UnLock");
        //Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = true;
    }

    //public void SetSaveData()
    //{
    //    SaveAction += Data_Save;
    //    SaveAction += DataManager.Inst.PlayerUnlockItem;
    //}

    //public void ResetData()
    //{
    //    if (DataManager.Inst == null)
    //        return;
    //    SaveAction = null;
    //    DataManager.Inst.DeleteJSONFile();

    //    DataManager.Inst?.NextStage(2);
    //}
    //public void NewGame()
    //{
    //    if (DataManager.Inst == null)
    //        return;
    //    ResetData();

    //    MainUI.MainPanel.BuffPanelSystem.Reset();
    //    MainUI.MainPanel.SkillPanelSystem.PrimarySkillPanel.Reset();
    //    MainUI.MainPanel.SkillPanelSystem.SecondarySkillPanel.Reset();

    //    var skipCutScene = DataManager.Inst.JSON_DataParsing.m_JSON_DefaultData.SkipCutSceneList.ToList();
    //    if (skipCutScene.Contains(4))
    //    {
    //        DataManager.Inst?.NextStage(3);
    //    }
    //    else
    //    {
    //        DataManager.Inst?.NextStage(4);
    //    }
    //    if (isPause)
    //    {
    //        InputHandler.ChangeCurrentActionMap(InputEnum.GamePlay, true);
    //    }
    //    ClearScene();
    //}
    public void ClearScene()
    {
        var FadeOut = Resources.Load<GameObject>(GlobalValue.FadeOutCutScene);
        if (FadeOut != null)
        {
            Instantiate(FadeOut);
        }
    }
    public void GoTitleScene()
    {
        //ChangeUI(UI_State.Loading);
        Time.timeScale = 1f;
        isPause = false;
        var FadeOut = Resources.Load<GameObject>(GlobalValue.TitleFadeOutCutScene);
        if (FadeOut != null)
        {
            Instantiate(FadeOut);
        }
    }
}
