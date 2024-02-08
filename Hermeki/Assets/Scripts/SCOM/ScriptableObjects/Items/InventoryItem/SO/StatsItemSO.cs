using System;
using System.Collections.Generic;
using SCOM;
using SCOM.Weapons.Components;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

[Serializable]
public struct Write_StatsData_item
{
    public Stats_TYPE type;
    public LocalizedString StatsLocalizeString;
    [Tooltip("부가 조건")]
    public float variable;
    [Tooltip("아이템 효과의 값")]
    public float value;
    [Tooltip("퍼센트 표기 여부")]
    public bool ShowPercent;
    [Tooltip("부호 표기 여부(+,-)")]
    public bool HideMark;
}

[Serializable]
public struct ItemComposite
{
    public StatsItemSO MaterialItem;
    public StatsItemSO ResultItem;
    public AudioPrefab EditSFX;
    public GameObject EditVFX;
}

[CreateAssetMenu(fileName = "newItemData", menuName = "Data/Item Data/Stats Data")]
public class StatsItemSO : ItemDataSO
{
    public List<ItemComposite> CompositeItems;
    /// <summary>
    /// 필드 드랍
    /// </summary>
    public bool isFieldSpawn;

    [Header("--StatsData--")]
    [Tooltip("아이템이 갖는 스탯")]
    public StatsData StatsData = new StatsData();

    [Tooltip("표기되는 아이템 효과")]
    public List<Write_StatsData_item> StatsItems = new List<Write_StatsData_item>();
    
    /// <summary>
    /// 아이템의 스탯 설명
    /// </summary>
    public string StatsData_Descripts
    {
        get
        {
            string temp = "";
            for (int i = 0; i < StatsItems.Count; i++)
            {
                temp += (LocalizationSettings.StringDatabase.GetTableEntry("Stats_Table", (StatsItems[i].StatsLocalizeString.TableEntryReference.KeyId == 0) ?
                    StatsItems[i].type.ToString() :
                    StatsItems[i].StatsLocalizeString.TableEntryReference).Entry.Value)
                    + (StatsItems[i].HideMark ? " " : ((StatsItems[i].value >= 0) ? (" +") : " -")) + Mathf.Abs(StatsItems[i].value);
                string var = "{variable}";
                if (temp.Contains(var))
                {
                    temp = temp.Replace(var, StatsItems[i].variable.ToString());
                }   

                if (StatsItems[i].ShowPercent)
                {
                    temp += "%";
                }

                temp += "\n";
            }
            return temp;
        }
    }
    [Tooltip("최대체력이 아닌 현재 체력 증가값")]
    public int Health;

    [Header("--Buff--")]
    public List<BuffItemSO> buffItems = new List<BuffItemSO>();
    [Header("--Effects--")]
    public EffectData InitEffectData;
    public List<EffectPrefab> InfinityEffectObjects = new List<EffectPrefab>();
    [Header("--ItemEvent--")]
    [field: SerializeField] public List<ItemEventSO> ItemEvents = new List<ItemEventSO>();
    [field: SerializeField] public LocalizedString EventNameLocal { get; private set; }
    [field: SerializeField] public LocalizedString EventDescriptionLocal { get; private set; }

    public virtual ItemEventSet ExeEvent(ITEM_TPYE type, Unit unit,Unit enemy,ItemEventSO _itemEvent, ItemEventSet itemEventSet)
    {
        return _itemEvent.ExcuteEvent(type, this, unit, enemy, itemEventSet);
    }
}

[Serializable]
public struct EffectData
{
    [field: Header("Collider Use")]
    [field: Tooltip("획득 시 이펙트")]
    [field: SerializeField] public GameObject AcquiredEffectPrefab { get; private set; }
    [field: Tooltip("획득 시 사운드이펙트")]
    [field: SerializeField] public AudioPrefab AcquiredSFX { get; private set; }

    [field: Tooltip("아이템 소비 여부")]
    [field: SerializeField] public bool isEquipment { get; private set; }
}
