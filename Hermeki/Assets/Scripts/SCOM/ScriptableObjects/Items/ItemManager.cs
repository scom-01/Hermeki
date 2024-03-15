using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class EquipItemEventSet
{
    public EquipItemDataSO EquipItemData;
    public List<ItemEventSet> itemEffectSets = new List<ItemEventSet>();
    /// <summary>
    /// OnAction이면 OnAction의 Count를 OnHit면 OnHit의 Count를 계산
    /// </summary>
    public List<int> EffectCount = new List<int>();
    public EquipItemEventSet(EquipItemDataSO itemSO, ItemEventSet _itemEffectSets = null)//  float _startTime = 0, int _Count = 0)
    {
        this.EquipItemData = itemSO;

        if (_itemEffectSets == null)
        {
            itemEffectSets = new List<ItemEventSet>();
            for (int i = 0; i < EquipItemData.ItemEvents.Count; i++)
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
    public SPUM_SpriteList SPUM_SpriteList;

    //현재 장착 
    public List<WeaponItem> WeaponItemList = new List<WeaponItem>();
    public List<ArmorItem> ArmorItemList = new List<ArmorItem>();

    //대기 인벤토리
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
                _armoritem.SetItemData(data);
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
            _weaponitem.SetItemData(data);
            return true;
        }
        Debug.Log($"무기 DB에 저장");
        WeaponDataList.Add(data);
        return true;
    }
    private int CurrentArmorPower()
    {
        int result = 0;
        foreach (var _armoritem in ArmorItemList)
        {
            result += _armoritem.Data.CurrentDurability;
        }
        return result;
    }

    public bool DamagedArmor(int _damage = 1)
    {
        if (CurrentArmorPower() == 0)
        {            
            return false;
        }

        for (int i = 0; i < _damage; i++)
        {
            foreach (var _armorItem in ArmorItemList)
            {
                if (_armorItem.Data.CurrentDurability > 0)
                {
                    _armorItem.DecreaseDurability();
                    break;
                }
            }
        }
        return true;
    }

    #region Event
    public bool ExeItemEvent(EquipItemEventSet _ItemEvent = null, ITEM_TPYE type = ITEM_TPYE.None, Unit enemy = null)
    {
        if (_ItemEvent == null)
        {
            return false;
        }
        for (int j = 0; j < _ItemEvent.itemEffectSets.Count; j++)
        {
            _ItemEvent.itemEffectSets[j] = _ItemEvent.EquipItemData.ExeEvent(type, Unit, enemy, _ItemEvent.EquipItemData.ItemEvents[j], _ItemEvent.itemEffectSets[j]);
        }
        return true;
    }
    public bool ExeItemEvent(List<EquipItemEventSet> _ItemEventList = null, ITEM_TPYE type = ITEM_TPYE.None, Unit enemy = null)
    {
        if (_ItemEventList == null)
        {
            return false;
        }
        for (int i = 0; i < _ItemEventList.Count; i++)
        {
            for (int j = 0; j < _ItemEventList[i].itemEffectSets.Count; j++)
            {
                _ItemEventList[i].itemEffectSets[j] = _ItemEventList[i].EquipItemData.ExeEvent(type, Unit, enemy, _ItemEventList[i].EquipItemData.ItemEvents[j], _ItemEventList[i].itemEffectSets[j]);
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
    public bool ItemOnHitExecute(List<EquipItemEventSet> _ItemEventList = null, Unit enemy = null)
    {
        if (Unit == null)
            return false;

        ExeItemEvent(_ItemEventList, ITEM_TPYE.OnHitEnemy, enemy);
        return true;
    }

    public bool ItemOnHitGround(List<EquipItemEventSet> _ItemEventList = null, Unit enemy = null)
    {
        if (Unit == null)
            return false;

        ExeItemEvent(_ItemEventList, ITEM_TPYE.OnHitGround, enemy);
        return true;
    }

    /// <summary>
    /// 액션 시 효과
    /// </summary>
    /// <param name="Unit"></param>
    /// <returns></returns>
    public bool ItemActionExecute(List<EquipItemEventSet> _ItemEventList = null)
    {
        if (Unit == null)
            return false;

        ExeItemEvent(_ItemEventList, ITEM_TPYE.OnAction, Unit?.GetTarget());
        return true;
    }

    /// <summary>
    /// 업데이트 시 호출
    /// </summary>
    /// <param name="Unit"></param>
    /// <returns></returns>
    public bool ItemExeUpdate(List<EquipItemEventSet> _ItemEventList = null)
    {
        if (Unit == null)
            return false;

        ExeItemEvent(_ItemEventList, ITEM_TPYE.OnUpdate, Unit?.GetTarget());
        return true;
    }

    /// <summary>
    /// 씬 변경 시 호출
    /// </summary>
    /// <param name="Unit"></param>
    public void ItemExeOnMoveMap(List<EquipItemEventSet> _ItemEventList = null)
    {
        if (Unit == null)
            return;

        ExeItemEvent(_ItemEventList, ITEM_TPYE.OnMoveMap, Unit?.GetTarget());
    }

    /// <summary>
    /// 데미지 입을 시 호출
    /// </summary>
    /// <param name="Unit"></param>
    /// <param name="enemy"></param>
    public void ItemExeOnDamaged(List<EquipItemEventSet> _ItemEventList = null, Unit enemy = null)
    {
        if (Unit == null)
            return;

        ExeItemEvent(_ItemEventList, ITEM_TPYE.OnDamaged, enemy);
    }


    /// <summary>
    /// 점프 시 호출
    /// </summary>
    /// <param name="Unit"></param>
    /// <param name="enemy"></param>
    public void ItemExeOnJump(List<EquipItemEventSet> _ItemEventList = null, Unit enemy = null)
    {
        if (Unit == null)
            return;

        ExeItemEvent(_ItemEventList, ITEM_TPYE.OnJump, enemy);
    }

    /// <summary>
    /// 착지 시 호출
    /// </summary>
    /// <param name="Unit"></param>
    /// <param name="enemy"></param>
    public void ItemExeOnLand(List<EquipItemEventSet> _ItemEventList = null, Unit enemy = null)
    {
        if (Unit == null)
            return;

        ExeItemEvent(_ItemEventList, ITEM_TPYE.OnLand, enemy);
    }

    /// <summary>
    /// 적 처치 시 호출
    /// </summary>
    /// <param name="Unit"></param>
    /// <param name="enemy"></param>
    public void ItemExeOnKilled(List<EquipItemEventSet> _ItemEventList = null, Unit enemy = null)
    {
        if (Unit == null)
            return;

        ExeItemEvent(_ItemEventList, ITEM_TPYE.OnKilled, enemy);
    }
    #endregion
}