using System.Collections.Generic;
using UnityEngine;


public class ArmorItem : EquipItem
{
    public ArmorStyle Style;
    public override EquipItemData GetData(out EquipItemData data)
    {
        data = unit?.ItemManager?.AllItemList[(int)Style];
        return data;
    }

    #region override
    protected override void Start()
    {
        base.Start();
        SetItemData(Data);
    }
    public override void DecreaseDurability()
    {
        unit?.ItemManager.AllItemList[(int)Style].DecreaseDurability();
        unit.ItemManager?.ExeItemEvent(ItemEvent, ItemEvent_Type.OnDamaged);
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
        if (!base.SetItemData(_data))
        {
            DestroyItem();
            Data.dataSO = null;
            SetSprite(null);
            return false;
        }

        //아이템 장착 배열에 위치
        unit.ItemManager.AllItemList[(int)(Data.dataSO as ArmorItemDataSO).Style].SetEquipItemData(_data);

        List<UnityEngine.Sprite> spriteList = new List<UnityEngine.Sprite>();
        foreach (var sprite in Data.CalculateSprite())
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
