using System;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour
{
    [SerializeField]
    private EquipItemData EquipData;
    [SerializeField]
    private Image Item_Img;
    [SerializeField]
    private TMP_Text Durability_Txt;
    private UI_Canvas _ParentCanvas;
    private InventoryTable _ParentInventoryTable;
    private EquipItemData _currEquipItemData;
    private void Start()
    {
        _ParentInventoryTable = this.GetComponentInParent<InventoryTable>();
        _ParentCanvas = this.GetComponentInParent<UI_Canvas>();
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
        SetSpriteUI(EquipData);
        SetDurabilityTxt(EquipData);
        return true;
    }

    /// <summary>
    /// Set Inventory UI Sprite 
    /// </summary>
    /// <param name="_data"></param>
    /// <returns></returns>
    private bool SetSpriteUI(EquipItemData _data)
    {
        if (Item_Img == null || _data?.dataSO == null)
            return false;

        Item_Img.sprite = _data.CalculateSprite()[0];

        return true;
    }

    private bool SetDurabilityTxt(EquipItemData _data)
    {
        if (Durability_Txt == null || _data?.dataSO == null)
            return false;
        Durability_Txt.text = String.Format($"{_data.CurrentDurability}/{_data.dataSO.MaxDurability}");
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
                    _ParentInventoryTable.unit?.ItemManager?.ChangeArmor(EquipData);
                    _ParentInventoryTable.SetState(InventoryState.Close);
                    break;
                case Item_Type.Weapon:
                    _ParentInventoryTable.SetState(InventoryState.ChoiceHand);
                    return;
                case Item_Type.Rune:
                    _ParentInventoryTable.SetState(InventoryState.Enchant);
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
                                    //_ParentInventoryTable.unit?.ItemManager?.Remove_InventoryWeaponItem(EquipData);
                                    if (ResearchItem(EquipData) != -1)
                                    {

                                    }
                                    _ParentInventoryTable.unit.ItemManager.ResearchItem(EquipData).SetEquipItemData(temp);
                                    //_ParentInventoryTable.unit.ItemManager.EnchantWeapon(temp);// SetState(InventoryState.ChoiceHand);
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
