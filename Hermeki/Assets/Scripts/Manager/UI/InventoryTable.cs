using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;


public class InventoryTable : MonoBehaviour
{
    public Unit unit;
    Canvas canvas;
    List<UI_Canvas> TableList = new List<UI_Canvas>();
    public InventoryState State;

    public EquipItemData CurrItem = new EquipItemData();

    //장착 중인 아이템 리스트
    public List<EquipItemData> EquipItemList = new List<EquipItemData>();
    //인벤토리 리스트
    public List<EquipItemData> InventoryItemList = new List<EquipItemData>();
    private void Awake()
    {
        canvas = this.GetComponent<Canvas>();
        TableList = this.GetComponentsInChildren<UI_Canvas>().ToList();
    }
    private void Start()
    {
        SetState(InventoryState.Close);
    }
    
    public void SetState(InventoryState _state)
    {
        State = _state;
        if (State == InventoryState.Table)
        {
            CurrItem = null;
            canvas.enabled = true;
        }
        else if (State == InventoryState.Close)
        {
            canvas.enabled = false;
            foreach (var _canvas in TableList)
            {
                _canvas.Canvas_Disable();
            }
            CurrItem = null;
            return;
        }
        else
        {
            canvas.enabled = true;
        }

        if (unit != null)
        {
            //현재 장착 중인 아이템 리스트 구하기
            {
                List<EquipItemData> templist = new List<EquipItemData>();
                for (int i = 0; i < 20; i++)
                {
                    if (unit.ItemManager.AllItemList[i].dataSO == null)
                        continue;
                    templist.Add(unit.ItemManager.AllItemList[i]);
                }
                EquipItemList = templist;
            }

            //인벤토리 아이템 리스트 구하기
            {
                List<EquipItemData> templist = new List<EquipItemData>();
                for (int i = 20; i < unit.ItemManager.AllItemList.Length; i++)
                {
                    if (unit.ItemManager.AllItemList[i].dataSO == null)
                        continue;
                    templist.Add(unit.ItemManager.AllItemList[i]);
                }
                InventoryItemList = templist;
            }
        }
        else
        {
            EquipItemList = new List<EquipItemData>();
            InventoryItemList = new List<EquipItemData>();
        }

        for (int i = 0; i < TableList.Count; i++)
        {
            if (TableList[i].State == State)
            {
                TableList[i].Canvas_Enable();
            }
            else
            {
                TableList[i].Canvas_Disable();
            }
        }


    }

    #region Research Item
    [ContextMenu("ResearchRune")]
    public List<EquipItemData> ResearchRune()
    {
        List<EquipItemData> tempList = InventoryItemList.ToList();
        for (int i = 0; i < tempList.Count; i++)
        {
            if (tempList[i].dataSO.ItemType != Item_Type.Rune)
            {
                tempList.RemoveAt(i);
                i--;
                continue;
            }
        }
        for (int i = 0; i < tempList.Count; i++)
        {
            print(tempList[i].dataSO.name);
            print(tempList[i].CurrentDurability);
        }
        return tempList;
    }

    /// <summary>
    /// 장착가능한 아이템
    /// </summary>
    /// <returns></returns>
    public List<EquipItemData> ResearchEquipable()
    {
        List<EquipItemData> tempList = InventoryItemList.ToList();
        for (int i = 0; i < tempList.Count; i++)
        {
            if (tempList[i].dataSO.ItemType == Item_Type.Rune)
            {
                tempList.RemoveAt(i);
                i--;
                continue;
            }

            //현재 캐릭터가 장착중인 아이템

        }
        return tempList;
    }
    /// <summary>
    /// 인챈트 가능한 아이템
    /// </summary>
    /// <returns></returns>
    public List<EquipItemData> ResearchEnchantable()
    {
        List<EquipItemData> tempList = InventoryItemList.ToList();
        for (int i = 0; i < tempList.Count; i++)
        {
            if (tempList[i].dataSO.ItemType == Item_Type.Rune)
            {
                tempList.RemoveAt(i);
                i--;
                continue;
            }
        }

        //현재 장착 중인 아이템 표시
        for (int i = 0; i < EquipItemList.Count; i++)
        {
            tempList.Add(EquipItemList[i]);
        }
        return tempList;
    }
    #endregion

    #region Event

    //Event
    public void SetState(int idx)
    {
        SetState((InventoryState)idx);
    }

    //Event
    public void SetEquipWeaponHand(bool isLeft)
    {
        if (CurrItem?.dataSO == null || GameManager.Inst?.LevelManager?.player?.ItemManager == null)
        {
            SetState(InventoryState.Close);
            return;
        }

        for (int i = 0; i < GameManager.Inst.LevelManager.player.ItemManager.EquipWeaponItemList.Count; i++)
        {
            if (GameManager.Inst.LevelManager.player.ItemManager.EquipWeaponItemList[i].Data.dataSO == null)
            {
                continue;
            }
            else if (GameManager.Inst.LevelManager.player.ItemManager.EquipWeaponItemList[i].isLeft == isLeft)
            {
                GameManager.Inst.LevelManager.player.ItemManager.ChangeWeapon(CurrItem, isLeft);
                SetState(InventoryState.Close);
                return;
            }
        }
        GameManager.Inst.LevelManager.player.ItemManager.EnchantWeapon(CurrItem, isLeft);
        SetState(InventoryState.Close);
    }

    //Event
    public void SetChoiceEquip(bool isEquip)
    {
        if (CurrItem?.dataSO == null || GameManager.Inst?.LevelManager?.player?.ItemManager == null)
        {
            SetState(InventoryState.Close);
            return;
        }
        if(isEquip)
        {
            switch (CurrItem.dataSO.ItemType)
            {
                case Item_Type.Armor:
                    unit?.ItemManager?.ChangeArmor(CurrItem);
                    break;
                case Item_Type.Weapon:
                    SetState(InventoryState.ChoiceHand);
                    return;
                case Item_Type.Rune:
                    break;
                default:
                    break;
            }
        }
        //드랍
        else
        {
            EquipItemData temp = new EquipItemData(CurrItem.dataSO, CurrItem.CurrentDurability);
            unit.ItemManager.ResearchItem(CurrItem).SetEquipItemData(null);
            GameManager.Inst.LevelManager.CurrStage()?.SO_Controller?.SpawnItem(temp, unit.Core.CoreCollisionSenses.UnitCenterPos);
        }
        
        SetState(InventoryState.Close);
    }

    //Event
    public void SetChoiceEnchant(bool isEnchant)
    {
        if (CurrItem?.dataSO == null || GameManager.Inst?.LevelManager?.player?.ItemManager == null)
        {
            SetState(InventoryState.Close);
            return;
        }

        if (isEnchant)
        {
            SetState(InventoryState.Enchant);
            return;
        }
        //드랍
        else
        {
            EquipItemData temp = new EquipItemData(CurrItem.dataSO, CurrItem.CurrentDurability);
            unit.ItemManager.ResearchItem(CurrItem).SetEquipItemData(null);
            GameManager.Inst.LevelManager.CurrStage()?.SO_Controller?.SpawnItem(temp, unit.Core.CoreCollisionSenses.UnitCenterPos);
        }

        SetState(InventoryState.Close);
    }
    #endregion
}
