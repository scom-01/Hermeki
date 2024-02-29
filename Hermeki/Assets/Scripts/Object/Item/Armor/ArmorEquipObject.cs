using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorEquipObject : EquipObject
{
    public ArmorStyle style;
    public override void Interactive(Unit unit)
    {
        Debug.Log($"Armor Equip Interactive {unit.name}");
        unit.ItemManager?.AddArmorItem(this.gameObject);
        this.transform.root.gameObject.SetActive(false);
    }

    public override void UnInteractive(Unit unit)
    {
        Debug.Log($"Armor Equip UnInteractive {unit.name}");
    }
}
