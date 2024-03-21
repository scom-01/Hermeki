using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

    //전체 아이템 배열
    /// <summary>
    /// 0~19 장착 아이템, 20~29 방어구, 무기 아이템 배열, 30~39 룬 아이템 배열
    /// </summary>
    public EquipItemData[] AllItemList = new EquipItemData[39];

    //현재 장착 
    public List<WeaponItem> EquipWeaponItemList = new List<WeaponItem>();
    public List<ArmorItem> EquipArmorItemList = new List<ArmorItem>();

    private void Start()
    {
        EquipArmorItemList = this.GetComponentsInChildren<ArmorItem>().ToList();
        EquipWeaponItemList = this.GetComponentsInChildren<WeaponItem>().ToList();
    }
    private void Update()
    {
        ItemExeUpdate();
    }

    #region Add, Remove

    public EquipItemData ResearchItem(EquipItemData data)
    {
        if (data?.dataSO == null)
            return null;
        for (int i = 0; i < AllItemList.Length; i++)
        {
            if (AllItemList[i].dataSO == null)
                continue;

            if (AllItemList[i]==data)
            {
                return AllItemList[i];
            }
        }
        return null;
    }
    public bool AddEquipItem(EquipItemData data)
    {
        if (data?.dataSO == null)
            return false;

        switch (data.dataSO.ItemType)
        {
            case Item_Type.Armor:
                AddArmorItem(data);
                break;
            case Item_Type.Weapon:
                AddWeaponItem(data);
                break;
            case Item_Type.Rune:
                AddRuneItem(data);
                break;
            default:
                break;
        }
        return true;
    }
    public bool AddArmorItem(EquipItemData data)
    {
        if (data?.dataSO == null || data.CurrentDurability == 0)
            return false;

        //현재 장착 가능한 부위에 장착
        if(!Equip_ArmorItem(data))
        {
            Debug.Log($"{data.dataSO.name} DB에 저장");
            Add_InventoryItem(data);
        }        
        return true;
    }

    public bool AddWeaponItem(EquipItemData data)
    {
        if (data == null || data.dataSO == null || data.CurrentDurability == 0)
            return false;

        //현재 장착 가능한 손에 장착
        if (!Equip_WeaponItem(data)) 
        {
            Debug.Log($"{data.dataSO.name} DB에 저장");
            Add_InventoryItem(data);
        }        
        return true;
    }

    public bool AddRuneItem(EquipItemData data)
    {
        if (data == null || data.dataSO == null || data.CurrentDurability == 0)
            return false;

        Debug.Log($"룬 DB에 저장");
        Add_InventoryRuneItem(data);
        return true;
    }

    /// <summary>
    /// 아이템을 장착한다면 true를 반환
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool Equip_ArmorItem(EquipItemData data)
    {
        if (data?.dataSO == null || data.CurrentDurability == 0)
            return false;

        //현재 장착 가능한 부위에 장착
        foreach (var _armoritem in EquipArmorItemList)
        {
            if (_armoritem.Data.dataSO != null)
            {
                continue;
            }

            //부위가 같고 장착 중이 아니라면
            if (_armoritem.Style == (data.dataSO as ArmorItemDataSO).Style)
            {
                _armoritem.SetItemData(data);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 아이템을 장착한다면 true를 반환
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool Equip_WeaponItem(EquipItemData data)
    {
        if (data?.dataSO == null || data.CurrentDurability == 0)
            return false;

        //현재 장착 가능한 부위에 장착
        foreach (var _weaponitem in EquipWeaponItemList)
        {
            if (_weaponitem.Data.dataSO != null)
            {
                continue;
            }
            _weaponitem.SetItemData(data);
            return true;
        }
        return false;
    }

    public EquipItemData CheckEquipArmor(EquipItemData data)
    {
        if (data?.dataSO == null)
            return null;
        for (int i = 0; i < EquipArmorItemList.Count; i++)
        {            
            if (EquipArmorItemList[i].Data == data) 
            {
                return EquipArmorItemList[i].Data;
            }
        }
        return null;
    }
    public EquipItemData CheckEquipWeapon(EquipItemData data)
    {
        if (data?.dataSO == null)
            return null;
        for (int i = 0; i < EquipWeaponItemList.Count; i++)
        {            
            if (EquipWeaponItemList[i].Data == data) 
            {
                return EquipWeaponItemList[i].Data;
            }
        }
        return null;
    }

    public bool Add_InventoryItem(EquipItemData data)
    {
        if (data?.dataSO == null)
            return false;

        for (int i = 20; i < 30; i++)
        {
            if (AllItemList[i].dataSO == null)
            {
                AllItemList[i].SetEquipItemData(data);
                break;
            }
        }

        return true;
    }
    public bool Add_InventoryRuneItem(EquipItemData data)
    {
        if (data?.dataSO == null)
            return false;

        for (int i = 30; i < AllItemList.Length; i++)
        {
            if (AllItemList[i].dataSO == null)
            {
                AllItemList[i].SetEquipItemData(data);
                break;
            }
        }

        return true;
    }

    public bool Remove_InventoryArmorItem(EquipItemData data)
    {
        if (data?.dataSO == null)
            return false;
        for (int i = 0; i < AllItemList.Length; i++)
        {
            if (AllItemList[i].dataSO == null)
                continue;
            if (AllItemList[i] == data)
            {
                AllItemList[i].SetEquipItemData(null);
                break;
            }
        }
        return true;
    }
    public bool Remove_InventoryWeaponItem(EquipItemData data)
    {
        if (data?.dataSO == null)
            return false;
        for (int i = 0; i < AllItemList.Length; i++)
        {
            if (AllItemList[i].dataSO == null)
                continue;
            if (AllItemList[i] == data)
            {
                AllItemList[i].SetEquipItemData(null);
                break;
            }
        }
        return true;
    }
    public bool Remove_InventoryRuneItem(EquipItemData data)
    {
        if (data?.dataSO == null)
            return false;
        for (int i = 0; i < AllItemList.Length; i++)
        {
            if (AllItemList[i].dataSO == null)
                continue;
            if (AllItemList[i] == data)
            {
                AllItemList[i].SetEquipItemData(null);
                break;
            }
        }
        return true;
    }
    #endregion

    #region Change

    public bool ChangeArmor(EquipItemData data)
    {
        if (data?.dataSO == null)
            return false;
        for (int i = 0; i < EquipArmorItemList.Count; i++)
        {
            if (EquipArmorItemList[i]?.Style == (data.dataSO as ArmorItemDataSO)?.Style)
            {
                //장착 중이던 아이템 인벤토리로 이동
                EquipItemData temp = new EquipItemData(EquipArmorItemList[i].Data.dataSO, EquipArmorItemList[i].Data.CurrentDurability);

                //장착 아이템 교체
                EquipArmorItemList[i].SetItemData(data);

                ResearchItem(data)?.SetEquipItemData(temp);
            }
        }
        return true;
    }

    public bool ChangeWeapon(EquipItemData data, bool isLeft = true)
    {
        if (data?.dataSO == null)
            return false;
        for (int i = 0; i < EquipWeaponItemList.Count; i++)
        {
            if (EquipWeaponItemList[i].isLeft == isLeft)
            {
                //장착 중이던 아이템 인벤토리로 이동
                EquipItemData temp = new EquipItemData(EquipWeaponItemList[i].Data.dataSO, EquipWeaponItemList[i].Data.CurrentDurability);
                
                //장착 아이템 교체
                EquipWeaponItemList[i]?.SetItemData(data);

                ResearchItem(data)?.SetEquipItemData(temp);
                break;
            }
        }
        return true;
    }

    /// <summary>
    /// 룬 아이템과 합성
    /// </summary>
    /// <param name="data">Rune 타입 아이템</param>
    /// <returns></returns>
    public bool EnchantArmor(EquipItemData data)
    {
        if (data?.dataSO == null)
            return false;
        EquipItemData temp = new EquipItemData(data.dataSO, data.dataSO.MaxDurability);
        for (int i = 0; i < EquipArmorItemList.Count; i++)
        {
            if (EquipArmorItemList[i]?.Style == (temp.dataSO as ArmorItemDataSO)?.Style)
            {
                //장착 아이템 교체
                EquipArmorItemList[i].SetItemData(temp);
            }
        }
        return true;
    }
    /// <summary>
    /// 룬 아이템과 합성
    /// </summary>
    /// <param name="data">Rune 타입 아이템</param>
    /// <param name="isLeft"></param>
    /// <returns></returns>
    public bool EnchantWeapon(EquipItemData data)
    {
        if (data?.dataSO == null)
            return false;

        EquipItemData temp = ResearchItem(data);

        for (int i = 0; i < EquipWeaponItemList.Count; i++)
        {
            if (EquipWeaponItemList[i].Data == data)
            {
                //장착 아이템 교체
                EquipWeaponItemList[i]?.SetItemData(data);
                break;
            }
        }
        return true;
    }
    public bool EnchantWeapon(EquipItemData data, bool isLeft)
    {
        if (data?.dataSO == null)
            return false;

        EquipItemData temp = new EquipItemData(data.dataSO, data.dataSO.MaxDurability);
        for (int i = 0; i < EquipWeaponItemList.Count; i++)
        {
            if (EquipWeaponItemList[i].isLeft == isLeft)
            {
                //장착 아이템 교체
                EquipWeaponItemList[i]?.SetItemData(temp);
                break;
            }
        }
        return true;
    }
    #endregion
    private int CurrentArmorPower()
    {
        int result = 0;
        foreach (var _armoritem in EquipArmorItemList)
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
            foreach (var _armorItem in EquipArmorItemList)
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
    public bool ExeItemEvent(EquipItemEventSet _ItemEvent = null, ItemEvent_Type type = ItemEvent_Type.None, Unit enemy = null)
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
    public bool ExeItemEvent(List<EquipItemEventSet> _ItemEventList = null, ItemEvent_Type type = ItemEvent_Type.None, Unit enemy = null)
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
            itemSet.itemEffectSets[i] = itemSet.item.ExeEvent(ItemEvent_Type.OnInit, Unit, Unit?.GetTarget(), itemSet.item.ItemEvents[i], itemSet.itemEffectSets[i]);
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

        ExeItemEvent(_ItemEventList, ItemEvent_Type.OnHitEnemy, enemy);
        return true;
    }

    public bool ItemOnHitGround(List<EquipItemEventSet> _ItemEventList = null, Unit enemy = null)
    {
        if (Unit == null)
            return false;

        ExeItemEvent(_ItemEventList, ItemEvent_Type.OnHitGround, enemy);
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

        ExeItemEvent(_ItemEventList, ItemEvent_Type.OnAction, Unit?.GetTarget());
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

        ExeItemEvent(_ItemEventList, ItemEvent_Type.OnUpdate, Unit?.GetTarget());
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

        ExeItemEvent(_ItemEventList, ItemEvent_Type.OnMoveMap, Unit?.GetTarget());
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

        ExeItemEvent(_ItemEventList, ItemEvent_Type.OnDamaged, enemy);
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

        ExeItemEvent(_ItemEventList, ItemEvent_Type.OnJump, enemy);
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

        ExeItemEvent(_ItemEventList, ItemEvent_Type.OnLand, enemy);
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

        ExeItemEvent(_ItemEventList, ItemEvent_Type.OnKilled, enemy);
    }
    #endregion
}