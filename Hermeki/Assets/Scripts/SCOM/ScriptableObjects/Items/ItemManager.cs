using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using static UnityEditor.Progress;


[Serializable]
public class WeaponItemEventSet
{
    public WeaponItemDataSO weaponItemData;
    public List<ItemEventSet> itemEffectSets = new List<ItemEventSet>();
    /// <summary>
    /// OnAction이면 OnAction의 Count를 OnHit면 OnHit의 Count를 계산
    /// </summary>
    public List<int> EffectCount = new List<int>();
    public WeaponItemEventSet(WeaponItemDataSO itemSO, ItemEventSet _itemEffectSets = null)//  float _startTime = 0, int _Count = 0)
    {
        this.weaponItemData = itemSO;

        if (_itemEffectSets == null)
        {
            itemEffectSets = new List<ItemEventSet>();
            for (int i = 0; i < weaponItemData.ItemEvents.Count; i++)
            {
                itemEffectSets.Add(new ItemEventSet());
            }
        }
    }
}

public class ItemManager : MonoBehaviour
{
    public Unit Unit
    {
        get
        {
            if (unit == null)
            {
                unit = this.GetComponent<Unit>();
            }
            return unit;
        }
    }
    private Unit unit;
    /// <summary>
    /// 현재 아이템
    /// </summary>
    public List<WeaponItemEventSet> ItemEventList = new List<WeaponItemEventSet>();
    public SPUM_SpriteList SPUM_SpriteList;

    public List<WeaponItem> WeaponItemList = new List<WeaponItem>();
    public List<ArmorItem> ArmorItemList = new List<ArmorItem>();

    public List<EquipItemData> WeaponDataList = new List<EquipItemData>();
    public List<EquipItemData> ArmorDataList = new List<EquipItemData>();

    private void Start()
    {
        ArmorItemList = this.GetComponentsInChildren<ArmorItem>().ToList();
        WeaponItemList = this.GetComponentsInChildren<WeaponItem>().ToList();
    }
    private void Update()
    {
        ItemExeUpdate();
    }
    public bool AddArmorItem(EquipItemData data)
    {
        if (data == null || data.dataSO == null || data.CurrentDurability == 0)
            return false;

        List<Sprite> sprites = new List<Sprite>();
        int idx = data.dataSO.CalculateDurability(data.CurrentDurability);
        foreach (var SR in data.dataSO.Sprite[idx].sprites)
        {
            sprites.Add(SR);
        }

        foreach (var _armoritem in ArmorItemList)
        {
            if (_armoritem.Style == (data.dataSO as ArmorItemDataSO).Style)
            {
                _armoritem.SetArmorData(data);
                return true;
            }
        }
        Debug.Log($"방어구 DB에 저장");
        ArmorDataList.Add(data);
        return true;
    }

    public bool AddWeaponItem(EquipItemData data)
    {
        if (data == null || data.dataSO == null || data.CurrentDurability == 0)
            return false;

        foreach (var _weaponitem in WeaponItemList)
        {
            if(_weaponitem.Data.dataSO != null)
            {
                continue;
            }
            _weaponitem.SetWeaponData(data);
            return true;
        }
        Debug.Log($"무기 DB에 저장");
        WeaponDataList.Add(data);
        return true;
    }
    #region Add, Remove
    public bool AddItemEvent(WeaponItemEventSet item)
    {
        if (item == null)
            return false;

        if (ItemEventList.Contains(item))
        {
            Debug.Log($"Contains {item.weaponItemData.name}");
            return false;
        }
        ItemEventList.Add(item);

        return true;
    }
    public bool RemoveItemEvent(WeaponItemEventSet item)
    {
        if (item == null)
            return false;

        ItemEventList.Remove(item);
        return true;
    }
    #endregion

    #region Event
    public bool ExeItemEvent(ITEM_TPYE type, Unit enemy = null)
    {
        for (int i = 0; i < ItemEventList.Count; i++)
        {
            for (int j = 0; j < ItemEventList[i].itemEffectSets.Count; j++)
            {
                ItemEventList[i].itemEffectSets[j] = ItemEventList[i].weaponItemData.ExeEvent(type, Unit, enemy, ItemEventList[i].weaponItemData.ItemEvents[j], ItemEventList[i].itemEffectSets[j]);
            }
        }
        return true;
    }

    /// <summary>
    /// 아이템의 Init Event 호출
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public bool ItemOnInit(ItemSet itemSet)
    {
        for (int i = 0; i < itemSet.itemEffectSets.Count; i++)
        {
            itemSet.itemEffectSets[i] = itemSet.item.ExeEvent(ITEM_TPYE.OnInit, Unit, Unit?.GetTarget(), itemSet.item.ItemEvents[i], itemSet.itemEffectSets[i]);
        }
        return true;

    }


    /// <summary>
    /// 공격 적중 시 효과
    /// </summary>
    /// <param name="Unit">공격 주체</param>
    /// <param name="Enemy">적중 당한 적</param>
    /// <returns></returns>
    public bool ItemOnHitExecute(Unit enemy = null)
    {
        if (Unit == null)
            return false;

        ExeItemEvent(ITEM_TPYE.OnHitEnemy, enemy);
        return true;
    }

    public bool ItemOnHitGround(Unit enemy = null)
    {
        if (Unit == null)
            return false;

        ExeItemEvent(ITEM_TPYE.OnHitGround, enemy);
        return true;
    }

    /// <summary>
    /// 액션 시 효과
    /// </summary>
    /// <param name="Unit"></param>
    /// <returns></returns>
    public bool ItemActionExecute()
    {
        if (Unit == null)
            return false;

        ExeItemEvent(ITEM_TPYE.OnAction, Unit?.GetTarget());
        return true;
    }

    /// <summary>
    /// 업데이트 시 호출
    /// </summary>
    /// <param name="Unit"></param>
    /// <returns></returns>
    public bool ItemExeUpdate()
    {
        if (Unit == null)
            return false;

        ExeItemEvent(ITEM_TPYE.OnUpdate, Unit?.GetTarget());
        return true;
    }

    /// <summary>
    /// 씬 변경 시 호출
    /// </summary>
    /// <param name="Unit"></param>
    public void ItemExeOnMoveMap()
    {
        if (Unit == null)
            return;

        ExeItemEvent(ITEM_TPYE.OnMoveMap, Unit?.GetTarget());
    }

    /// <summary>
    /// 데미지 입을 시 호출
    /// </summary>
    /// <param name="Unit"></param>
    /// <param name="enemy"></param>
    public void ItemExeOnDamaged(Unit enemy = null)
    {
        if (Unit == null)
            return;

        ExeItemEvent(ITEM_TPYE.OnDamaged, enemy);
    }


    /// <summary>
    /// 점프 시 호출
    /// </summary>
    /// <param name="Unit"></param>
    /// <param name="enemy"></param>
    public void ItemExeOnJump(Unit enemy = null)
    {
        if (Unit == null)
            return;

        ExeItemEvent(ITEM_TPYE.OnJump, enemy);
    }

    /// <summary>
    /// 착지 시 호출
    /// </summary>
    /// <param name="Unit"></param>
    /// <param name="enemy"></param>
    public void ItemExeOnLand(Unit enemy = null)
    {
        if (Unit == null)
            return;

        ExeItemEvent(ITEM_TPYE.OnLand, enemy);
    }

    /// <summary>
    /// 적 처치 시 호출
    /// </summary>
    /// <param name="Unit"></param>
    /// <param name="enemy"></param>
    public void ItemExeOnKilled(Unit enemy = null)
    {
        if (Unit == null)
            return;

        ExeItemEvent(ITEM_TPYE.OnKilled, enemy);
    }
    #endregion
}