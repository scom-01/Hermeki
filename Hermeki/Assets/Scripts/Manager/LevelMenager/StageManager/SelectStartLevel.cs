using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class SelectStartLevel : MonoBehaviour
{
    [Header("Component")]
    public Image StartingCharacterImg;
    public Image SelectBorderImg;
    public TMP_Text NickName;
    public Button SelectBtn;

    [Header("Setting")]
    public string CharacterName;
    public List<EquipItemData> EquipStartingItem;
    public SpawnStartEquipItem StartAction;
    public int SelectedLevel
    {
        get => _currLevel;
        set
        {
            if (value > MaxLevel)
            {
                value = 0;
            }
            _currLevel = value;
        }
    }
    [SerializeField]
    private int _currLevel;
    private int currIdx;
    /// <summary>
    /// 난이도를 표현할 UI Image
    /// </summary>
    public List<Image> LevelImgList = new List<Image>();
    /// <summary>
    /// 선택여부
    /// </summary>
    public bool isSelect
    {
        get => _isSelect;
        set
        {
            if (SelectBorderImg != null)
            {
                SelectBorderImg.enabled = value;
            }
            _isSelect = value;
        }
    }
    private bool _isSelect;
    private int MaxLevel = 5;
    private void Awake()
    {
        var list = this.transform.parent.GetComponentsInChildren<SelectStartLevel>();
        for (int i = 0; i < list.Length; i++)
        {
            if (list[i] == this)
            {
                currIdx = i;
                return;
            }
        }
    }

    private void Start()
    {
        for (int i = 0; i < LevelImgList.Count; i++)
        {
            if (LevelImgList[i] == null)
                continue;

            if (SelectedLevel > i)
            {
                LevelImgList[i].enabled = true;
                continue;
            }
            LevelImgList[i].enabled = false;
        }
        if (PlayFabManager.Inst.UserDataDictionary.ContainsKey(CharacterName)) 
        {
            _currLevel = int.Parse(PlayFabManager.Inst.UserDataDictionary[CharacterName]);
            Debug.Log($"_currLevel = {_currLevel}");
        }
        else
        {
            PlayFabManager.Inst?.CS_SetUserData(CharacterName, 0.ToString());
            _currLevel = 0;
        }
    }
    public void SetStartingItem()
    {
        if (StartAction != null)
        {
            StartAction?.SetSpawnObjList(EquipStartingItem);
        }

        GameManager.Inst?.LevelManager?.ChangeLevel(SelectedLevel);
    }
    public void SetStageLevel(int idx)
    {
        if (GameManager.Inst?.LevelManager != null)
        {
            GameManager.Inst?.LevelManager?.SetCharacterLevel(CharacterName, idx);
        }

        if (idx > MaxLevel)
        {
            idx = 0;
        }
        SelectedLevel = idx;
        SetStartingItem();

        for (int i = 0; i < LevelImgList.Count; i++)
        {
            if (LevelImgList[i] == null)
                continue;

            if (SelectedLevel > i)
            {
                LevelImgList[i].enabled = true;
                continue;
            }
            LevelImgList[i].enabled = false;
        }
    }
    /// <summary>
    /// Event 호출
    /// </summary>
    public void IncreaseStageLevel()
    {
        SelectedLevel++;
        SetStageLevel(SelectedLevel);
        GameManager.Inst?.LevelManager?.SelectStart(currIdx);
    }
}
