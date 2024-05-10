using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SpawnItemEvent : UnitEvent
{
    /// <summary>
    /// 드랍 아이템
    /// </summary>
    public List<EquipItemData> equipItems;
    /// <summary>
    /// 스폰 유닛
    /// </summary>
    public List<AssetReferenceGameObject> UnitAssets;
    public override void Action()
    {
        if (unit == null)
            return;

        foreach (var item in equipItems)
        {
            if (GameManager.Inst?.LevelManager?.CurrStage()?.SO_Controller?.SpawnItem(item, unit.Core.CoreCollisionSenses.UnitCenterPos) == null)
            {
                //Instantiate(item, unit.Core.CoreCollisionSenses.UnitCenterPos, Quaternion.identity);
            }
        }

        foreach (var item in UnitAssets)
        {
            if (GameManager.Inst?.LevelManager?.CurrStage()?.SO_Controller?.SpawnUnit(item, unit.Core.CoreCollisionSenses.UnitCenterPos) == null)
            {
                var pos = unit.Core.CoreCollisionSenses.UnitCenterPos;
                item.InstantiateAsync(this.transform.parent).Completed +=
                    (AsyncOperationHandle<GameObject> _obj) =>
                    {
                        _obj.Result.transform.position = pos;
                        _obj.Result.AddComponent<SelfCleanup>();
                    };
            }
        }
    }
}
