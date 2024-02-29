using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

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
    public int CurrentDurability;

    private void Awake()
    {
        unit = GetComponentInParent<Unit>();
        if (unit != null)
        {
            this.tag = unit.transform.tag;
        }
        eventHandler = unit.GetComponentInChildren<AnimationEventHandler>();
    }

    private void DecreaseDurability()
    {
        CurrentDurability--;
        if (CurrentDurability > 0)
        {
            //CalculateWeaponSprite();
            return;
        }
        else
        {
            //SetWeaponData(null);
            Debug.Log($"Destroy Weapon {this.name}");
        }
    }

    public void SetSprite(List<UnityEngine.Sprite> sprites)
    {
        switch (Style)
        {
            case ArmorStyle.Helmet:
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
