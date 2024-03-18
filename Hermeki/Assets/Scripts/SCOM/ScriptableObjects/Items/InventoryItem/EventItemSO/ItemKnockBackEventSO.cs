using UnityEngine;

[CreateAssetMenu(fileName = "newKnockBackEvent", menuName = "Data/Item Data/ItemKnockBackEventSO Data")]
public class ItemKnockBackEventSO : ItemEventSO
{
    [Header("KnockBack Event")]
    public Vector2 angle;
    public float strength;
    
    private void KnockBackAction(Unit unit, Unit enemy = null)
    {
        if (enemy == null)
            return;

        SpawnVFX(unit);
        SpawnSFX(unit);

        enemy.Core.CoreKnockBackReceiver.KnockBack(angle, strength, unit.Core.CoreMovement.FancingDirection);
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
            KnockBackAction(unit, enemy);
            itemEventSet.Count = 0;
            itemEventSet.startTime = GameManager.Inst.PlayTime;
        }

        return itemEventSet;
    }    
}
