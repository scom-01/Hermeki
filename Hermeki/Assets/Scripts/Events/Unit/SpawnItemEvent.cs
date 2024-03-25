using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SpawnItemEvent : UnitEvent
{
    public List<EquipItemData> equipItems;
    public List<AssetReferenceGameObject> UnitAssets;
    public override void Action()
    {
        if (unit == null)
            return;

        foreach (var item in equipItems)
        {
            GameManager.Inst.LevelManager.CurrStage()?.SO_Controller?.SpawnItem(item, unit.Core.CoreCollisionSenses.UnitCenterPos);
        }

        foreach (var item in UnitAssets)
        {
            GameManager.Inst.LevelManager.CurrStage()?.SO_Controller?.SpawnUnit(item, unit.Core.CoreCollisionSenses.UnitCenterPos);
        }
    }
}
