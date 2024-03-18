using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour
{
    public EquipItemData EquipData;
    private Image Item_Img;
    private void Awake()
    {
        Item_Img = this.GetComponentInChildren<Image>();
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
        return true;
    }

    /// <summary>
    /// Set Inventory UI Sprite 
    /// </summary>
    /// <param name="_data"></param>
    /// <returns></returns>
    public bool SetSpriteUI(EquipItemData _data)
    {
        if (Item_Img == null || _data?.dataSO == null) 
            return false;

        Item_Img.sprite = _data.CalculateSprite()[0];
        
        return true;
    }
}
