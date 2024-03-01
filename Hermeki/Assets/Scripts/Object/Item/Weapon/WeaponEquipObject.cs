using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEquipObject : EquipObject
{
    public WeaponEquipObject(EquipItemData data) : base(data)
    {
    }

    public ItemObject itemObject => this.transform.root.GetComponent<ItemObject>();
    public override void Interactive(Unit unit)
    {
        Debug.Log($"Weapon Equip Interactive {unit.name}");
        unit.ItemManager?.AddWeaponItem(Data);
        this.transform.root.gameObject.SetActive(false);
    }

    public override void UnInteractive(Unit unit)
    {
        Debug.Log($"Weapon Equip UnInteractive {unit.name}");
    }
    [ContextMenu("SetSpriteRenderer")]
    public override void SetSpriteRenderer()
    {
        if (itemObject == null || Data.dataSO == null || Data.CurrentDurability == 0)
        {
            this.transform.root.gameObject.SetActive(false);
            return;
        }

        if (!this.transform.root.gameObject.activeSelf)
            this.transform.root.gameObject.SetActive(true);

        itemObject.SetSpriteRenderer(Data);
    }
}
