using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipItemData
{
    public WeaponItemDataSO dataSO;
    public int CurrentDurability;
    public EquipItemData(WeaponItemDataSO dataSO, int currentDurability)
    {
        this.dataSO = dataSO;
        CurrentDurability = currentDurability;
    }
}


[Serializable]
public struct ItemSpriteData
{
    public Sprite sprite;
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
    public virtual ItemEventSet ExeEvent(ITEM_TPYE type, Unit unit, Unit enemy, ItemEventSO _itemEvent, ItemEventSet itemEventSet)
    {
        return _itemEvent.ExcuteEvent(type, this, unit, enemy, itemEventSet);
    }
}
