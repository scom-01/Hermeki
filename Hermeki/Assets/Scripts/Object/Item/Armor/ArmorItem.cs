using System.Collections.Generic;
using UnityEngine;

public enum ArmorStyle
{
    Helmet = 0,
    Armor = 1,
    Boots = 2,
}
public class ArmorItem : MonoBehaviour
{
    private Unit unit;
    protected AnimationEventHandler eventHandler;
    public ArmorStyle Style;
    public EquipItemData Data;
    private void Awake()
    {
        unit = GetComponentInParent<Unit>();
        if (unit != null)
        {
            this.tag = unit.transform.tag;
        }
        eventHandler = unit.GetComponentInChildren<AnimationEventHandler>();
    }

    private void Start()
    {
        SetArmorData(Data);
    }
    public void DecreaseDurability()
    {
        Data.CurrentDurability--;
        if (Data.CurrentDurability > 0)
        {
            SetArmorData(Data);
            return;
        }
        else
        {
            Data.CurrentDurability = 0;
            SetArmorData(null);
            Debug.Log($"Destroy Weapon {this.name}");
        }
    }

    public void SetArmorData(EquipItemData data)
    {
        if (data == null || data.dataSO == null || data.CurrentDurability == 0)
        {
            Data.dataSO = null;
            SetSprite(null);
            return;
        }
        Data = data;
        int idx = Data.dataSO.CalculateDurability(Data.CurrentDurability);
        List<UnityEngine.Sprite> spriteList = new List<UnityEngine.Sprite>();
        foreach (var sprite in Data.dataSO.Sprite[idx].sprites)
        {
            spriteList.Add(sprite);
        }
        SetSprite(spriteList);
    }

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
