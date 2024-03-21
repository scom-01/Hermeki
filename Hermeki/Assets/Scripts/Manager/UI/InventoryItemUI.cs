using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct InventoryContent
{
    public GameObject ContentParent;
    public Image Item_Img;
    public TMP_Text Durability_Txt;
    public TMP_Text Equip_Txt;
}

public class InventoryItemUI : MonoBehaviour
{
    [SerializeField]
    private EquipItemData EquipData;

    private InventoryTable _ParentInventoryTable;

    public List<InventoryContent> inventoryContents = new List<InventoryContent>();
    private void Awake()
    {
        _ParentInventoryTable = this.GetComponentInParent<InventoryTable>();
    }

    /// <summary>
    /// Set EquipItemData
    /// </summary>
    /// <param name="_data"></param>
    /// <returns></returns>
    public bool SetData(EquipItemData _data)
    {
        if (_data?.dataSO == null)
            return false;

        EquipData = _data;
        SetActiveContent();
        return true;
    }

    private bool SetActiveContent()
    {
        if (inventoryContents?.Count == 0)
            return false;

        foreach (var Content in inventoryContents)
        {
            Content.ContentParent.SetActive(false);
        }
        if (inventoryContents.Count == (int)Item_Type.Count)
        {
            inventoryContents[(int)EquipData.dataSO.ItemType].ContentParent.SetActive(true);
            SetSpriteUI(EquipData, inventoryContents[(int)EquipData.dataSO.ItemType].Item_Img);
            SetDurabilityTxt(EquipData, inventoryContents[(int)EquipData.dataSO.ItemType].Durability_Txt, inventoryContents[(int)EquipData.dataSO.ItemType].Equip_Txt);
        }

        return true;
    }

    /// <summary>
    /// Set Inventory UI Sprite 
    /// </summary>
    /// <param name="_data"></param>
    /// <returns></returns>
    private bool SetSpriteUI(EquipItemData _data, Image _img)
    {
        if (_img == null || _data?.dataSO == null)
            return false;

        _img.sprite = _data.CalculateSprite()[0];

        return true;
    }

    private bool SetDurabilityTxt(EquipItemData _data, TMP_Text _durability_Txt, TMP_Text _equip_Txt)
    {
        if (_data?.dataSO == null)
            return false;

        if (_durability_Txt != null)
            _durability_Txt.text = String.Format($"{_data.CurrentDurability}/{_data.dataSO.MaxDurability}");

        if (checkEquip(_data))
        {
            if (_equip_Txt != null)
                _equip_Txt.text = String.Format($"E");
        }
        else
        {
            if (_equip_Txt != null)
                _equip_Txt.text = String.Format($"");
        }
        return true;
    }

    /// <summary>
    /// true = 장착 중
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private bool checkEquip(EquipItemData data)
    {
        if (data?.dataSO == null)
        {
            return false;
        }
        switch (data.dataSO.ItemType)
        {
            case Item_Type.Armor:
                if (_ParentInventoryTable?.unit?.ItemManager?.CheckEquipArmor(data) == null)
                {
                    return false;
                }
                break;
            case Item_Type.Weapon:
                if (_ParentInventoryTable?.unit?.ItemManager?.CheckEquipWeapon(data) == null)
                {
                    return false;
                }
                break;
            case Item_Type.Rune:
                return false;
            case Item_Type.Count:
                return false;
            default:
                return false;
        }

        return true;
    }

    //Event
    public void OnClick()
    {
        if (_ParentInventoryTable == null || EquipData?.dataSO == null)
            return;

        if (_ParentInventoryTable.CurrItem?.dataSO == null)
        {
            //현재 클릭한 아이템을 선택 아이템으로 설정
            _ParentInventoryTable.CurrItem = EquipData;

            switch (EquipData.dataSO.ItemType)
            {
                case Item_Type.Armor:
                    _ParentInventoryTable.SetState(InventoryState.ChoiceEquip);
                    break;
                case Item_Type.Weapon:
                    _ParentInventoryTable.SetState(InventoryState.ChoiceEquip);
                    return;
                case Item_Type.Rune:
                    _ParentInventoryTable.SetState(InventoryState.ChoiceEnchant);
                    return;
                default:
                    break;
            }
            return;
        }
        else
        {
            //선택된 아이템이 있을 때
            switch (_ParentInventoryTable.CurrItem.dataSO.ItemType)
            {
                case Item_Type.Armor:
                    break;

                case Item_Type.Weapon:
                    break;
                case Item_Type.Rune:
                    EquipItemData temp;
                    RuneItemDataSO runeData = (_ParentInventoryTable.CurrItem.dataSO as RuneItemDataSO);
                    //인챈트 가능한 아이템 없으면 Close
                    if (runeData.enchantMethods?.Length == 0)
                    {
                        _ParentInventoryTable.SetState(InventoryState.Close);
                        break;
                    }

                    switch (EquipData.dataSO.ItemType)
                    {
                        //방어구를 선택 했었을 때
                        case Item_Type.Armor:
                            for (int i = 0; i < runeData.enchantMethods.Length; i++)
                            {
                                //인챈트 가능한 방어구일 때
                                if (runeData.enchantMethods[i].ItemStyle == (AllItemStyle)(EquipData.dataSO as ArmorItemDataSO).Style)
                                {
                                    //인챈트된 새로운 아이템
                                    temp = new EquipItemData(runeData.enchantMethods[i].Result, runeData.enchantMethods[i].Result.MaxDurability);

                                    _ParentInventoryTable.unit?.ItemManager?.EnchantArmor(temp);
                                    _ParentInventoryTable.CurrItem.DecreaseDurability();
                                    break;
                                }
                            }
                            break;
                        //무기를 선택 했었을 때
                        case Item_Type.Weapon:
                            for (int i = 0; i < runeData.enchantMethods.Length; i++)
                            {
                                if (runeData.enchantMethods[i].ItemStyle == (AllItemStyle)(EquipData.dataSO as WeaponItemDataSO).Style)
                                {
                                    //인챈트된 새로운 아이템
                                    temp = new EquipItemData(runeData.enchantMethods[i].Result, runeData.enchantMethods[i].Result.MaxDurability);
                                    if (ResearchItem(EquipData) != -1)
                                    {

                                    }
                                    _ParentInventoryTable.unit.ItemManager.ResearchItem(EquipData).SetEquipItemData(temp);
                                    _ParentInventoryTable.CurrItem.DecreaseDurability();
                                    break;
                                }
                            }
                            break;
                        case Item_Type.Rune:
                            break;
                        default:
                            break;
                    }

                    if (_ParentInventoryTable.CurrItem.CurrentDurability == 0)
                    {
                        _ParentInventoryTable.unit?.ItemManager?.Remove_InventoryRuneItem(_ParentInventoryTable.CurrItem);
                    }
                    _ParentInventoryTable.SetState(InventoryState.Close);
                    break;
                default:
                    break;
            }
        }
    }

    private int ResearchItem(EquipItemData data)
    {
        if (data?.dataSO == null || _ParentInventoryTable?.unit == null)
            return -1;

        for (int i = 0; i < _ParentInventoryTable.unit.ItemManager.AllItemList.Length; i++)
        {
            if (_ParentInventoryTable.unit.ItemManager.AllItemList[i].dataSO == null)
                continue;
            if (data == _ParentInventoryTable.unit.ItemManager.AllItemList[i])
            {
                return i;
            }
        }
        return -1;
    }
}
