using UnityEngine;


[CreateAssetMenu(fileName = "newItemEffectData", menuName = "Data/Item Data/ItemSpawnEvent Data")]
public class ItemSpawnEventSO : ItemEventSO
{
    [Tooltip("스폰될 오브젝트")]
    [SerializeField] private GameObject Object;
    private bool SpawnObject(Unit unit)
    {
        if (unit == null)
            return false;

        if (Object == null)
            return false;

        SpawnVFX(unit);
        SpawnSFX(unit);

        var obj = Instantiate(Object);
        obj.transform.position = unit.Core.CoreCollisionSenses.UnitCenterPos;
        if (obj.TryGetComponent(out ISpawn _SpawnObj))
        {
            _SpawnObj.Spawn();
        }
        return true;
    }
    public override ItemEventSet ExcuteEvent(ItemEvent_Type type, StatsItemSO parentItem, Unit unit, Unit enemy, ItemEventSet itemEventSet)
    {
        if (Item_Type != type || Item_Type == ItemEvent_Type.None || itemEventSet == null)
            return itemEventSet;

        if (!itemEventSet.init)
        {
            itemEventSet.init = true;
        }

        if (GameManager.Inst.PlayTime < itemEventSet.startTime + itemEventData.CooldownTime)
        {
            Debug.Log($"itemEffectSet.CoolTime = {GameManager.Inst.PlayTime - itemEventSet.startTime}");
            return itemEventSet;
        }

        itemEventSet.Count++;
        if (itemEventSet.Count >= itemEventData.MaxCount && itemEventData.Percent >= Random.Range(0f, 100f))
        {
            SpawnObject(unit);
            itemEventSet.Count = 0;
            itemEventSet.startTime = GameManager.Inst.PlayTime;
        }

        return itemEventSet;
    }
}
