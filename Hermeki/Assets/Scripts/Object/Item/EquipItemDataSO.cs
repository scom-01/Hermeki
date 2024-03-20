using SCOM;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EquipItemData
{
    public string StringName;
    public EquipItemDataSO dataSO;
    public int CurrentDurability
    {
        get
        {
            if (dataSO != null && _currDurability >= dataSO.MaxDurability)
            {
                _currDurability = dataSO.MaxDurability;
            }
            if (_currDurability < 0)
            {
                _currDurability = 0;
            }
            return _currDurability;
        }
        set
        {
            if (dataSO != null && value >= dataSO.MaxDurability)
            {
                value = dataSO.MaxDurability;
            }
            if (value < 0)
            {
                value = 0;
            }
            if (dataSO != null) 
                StringName = this.dataSO.name;
            _currDurability = value;
        }
    }
    [SerializeField]
    [field: Range(0, 99)]
    private int _currDurability;
    public EquipItemData(EquipItemDataSO dataSO = null, int currentDurability = 0)
    {
        this.dataSO = dataSO;
        StringName = (this.dataSO != null) ? this.dataSO.name : "";
        CurrentDurability = currentDurability;
    }

    public Sprite[] CalculateSprite()
    {
        return dataSO.Sprite[dataSO.CalculateDurability(CurrentDurability)].sprites;
    }
    public void DecreaseDurability(int amount = 1)
    {
        CurrentDurability -= amount;
        if (CurrentDurability < 0)
        {
            CurrentDurability = 0;
        }
    }

    public void SetEquipItemData(EquipItemData _data)
    {
        if (_data?.dataSO == null)
        {
            StringName = "";
            dataSO = null;
            CurrentDurability = 0;
            return;
        }

        StringName = _data.StringName;
        dataSO = _data.dataSO;
        CurrentDurability = _data.CurrentDurability;
    }
}
[Serializable]
public struct ItemSpriteData
{
    public Sprite[] sprites;
    public int durability;
}
[CreateAssetMenu(fileName = "newEquipItemDataSO", menuName = "Data/Equip Data/Equip Item Data")]
public class EquipItemDataSO : ScriptableObject
{
    public Item_Type ItemType;
    [Header("Durability")]
    public ItemSpriteData[] Sprite;
    public int MaxDurability;

    [Header("ItemEvent")]
    [field: SerializeField] public List<ItemEventSO> ItemEvents = new List<ItemEventSO>();

    [Header("Sounds")]
    [Tooltip("아이템 내구도 파괴 시 호출될 사운드")]
    public AudioData BrokenAudioData;


    [Header("Physics")]
    public PhysicsMaterial2D PM2D;

    public EquipItemDataSO(Item_Type _type)
    {
        ItemType = _type;
    }
    public virtual ItemEventSet ExeEvent(ItemEvent_Type type, Unit unit, Unit enemy, ItemEventSO _itemEvent, ItemEventSet itemEventSet)
    {
        return _itemEvent.ExcuteEvent(type, this, unit, enemy, itemEventSet);
    }

    /// <summary>
    /// 현재 내구도에 맞는 Sprite idx를 계산해주는 함수
    /// </summary>
    /// <param name="_Curr">아이템의 현재 내구도</param>
    /// <returns></returns>
    public int CalculateDurability(int _Curr)
    {
        int idx = 0;
        for (int i = 0; i < Sprite.Length; i++)
        {
            if (_Curr <= Sprite[i].durability)
            {
                idx = i;
            }
            else
            {
                break;
            }
        }
        return idx;
    }
}
