using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItemEvent : UnitEvent
{
    public List<EquipItemData> equipItems;
    public override void Action()
    {
        if (unit == null)
            return;

        foreach (var item in equipItems)
        {
            GameManager.Inst.LevelManager.CurrStage()?.SO_Controller?.SpawnItem(item, unit.Core.CoreCollisionSenses.UnitCenterPos);
        }
    }
}
