using System.Collections.Generic;
using UnityEngine;

public enum ArmorStyle
{
    Helmet = 0,
    Armor = 1,
    Boots = 2,
}
public class ArmorItem : EquipItem
{
    public ArmorStyle Style;

    #region override
    protected override void Start()
    {
        base.Start();
        SetItemData(Data);
    }
    public override void DecreaseDurability()
    {
        base.DecreaseDurability();
        unit.ItemManager?.ExeItemEvent(ItemEvent, ITEM_TPYE.OnDamaged);
        if (Data.CurrentDurability > 0)
        {
            SetItemData(Data);
            return;
        }
        else
        {
            Data.CurrentDurability = 0;
            SetItemData(null);
            Debug.Log($"Destroy Weapon {this.name}");
        }
    }

    public override bool SetItemData(EquipItemData _data)
    {
        if(!base.SetItemData(_data))
        {
            DestroyItem();
            Data.dataSO = null;
            SetSprite(null);
            return false;
        }
        int idx = Data.dataSO.CalculateDurability(Data.CurrentDurability);
        List<UnityEngine.Sprite> spriteList = new List<UnityEngine.Sprite>();
        foreach (var sprite in Data.dataSO.Sprite[idx].sprites)
        {
            spriteList.Add(sprite);
        }
        SetSprite(spriteList);
        return true;
    }
    public override bool DestroyItem()
    {
        if (base.DestroyItem())
        {
            return false;
        }

        return true;
    }
    #endregion

    public void SetSprite(List<UnityEngine.Sprite> sprites)
    {
        switch (Style)
        {
            case ArmorStyle.Helmet:
                if (sprites == null)
                {
                    unit.ItemManager?.SPUM_SpriteList?.SyncPath(unit.ItemManager.SPUM_SpriteList._hairList[1], null);
                    break;
                }
                unit.ItemManager?.SPUM_SpriteList?.SyncPath(unit.ItemManager.SPUM_SpriteList._hairList[1], sprites[0]);
                break;
            case ArmorStyle.Armor:
                unit.ItemManager?.SPUM_SpriteList?.SyncPath(unit.ItemManager.SPUM_SpriteList._armorList, sprites);
                break;
            case ArmorStyle.Boots:
                unit.ItemManager?.SPUM_SpriteList?.SyncPath(unit.ItemManager.SPUM_SpriteList._pantList, sprites);
                break;
            default:
                break;
        }
    }
}
