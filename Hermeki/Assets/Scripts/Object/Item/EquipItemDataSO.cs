using SCOM;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EquipItemData
{
    public EquipItemDataSO dataSO;
    public int CurrentDurability;
    public EquipItemData(EquipItemDataSO dataSO, int currentDurability)
    {
        this.dataSO = dataSO;
        CurrentDurability = currentDurability;
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
    [Header("Durability")]
    public ItemSpriteData[] Sprite;
    public int MaxDurability;

    [Header("ItemEvent")]
    [field: SerializeField] public List<ItemEventSO> ItemEvents = new List<ItemEventSO>();

    [Header("Sounds")]
    [Tooltip("아이템 내구도 파괴 시 호출될 사운드")]
    public AudioData BrokenAudioData;
    public virtual ItemEventSet ExeEvent(ITEM_TPYE type, Unit unit, Unit enemy, ItemEventSO _itemEvent, ItemEventSet itemEventSet)
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
