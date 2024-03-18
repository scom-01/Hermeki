using UnityEngine;

[CreateAssetMenu(fileName = "newProjectileAttackEventData", menuName = "Data/Item Data/ItemProjectileAttackEventSO Data")]
public class ItemProjectileEventSO : ItemEventSO
{
    [Header("Projectile Event")]
    public ProjectileActionData ProjectileActionData;

    private void ProjectileShoot(Unit unit, Unit enemy)
    {
        SpawnVFX(unit);
        SpawnSFX(unit);

        unit.Core.CoreEffectManager.StartProjectileCheck(unit, ProjectileActionData);
    }

    public override ItemEventSet ExcuteEvent(ItemEvent_Type type, EquipItemDataSO parentItem, Unit unit, Unit enemy, ItemEventSet itemEventSet)
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
            ProjectileShoot(unit, enemy);
            itemEventSet.Count = 0;
            itemEventSet.startTime = GameManager.Inst.PlayTime;
        }

        return itemEventSet;
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
            ProjectileShoot(unit, enemy);
            itemEventSet.Count = 0;
            itemEventSet.startTime = GameManager.Inst.PlayTime;
        }

        return itemEventSet;
    }
}
