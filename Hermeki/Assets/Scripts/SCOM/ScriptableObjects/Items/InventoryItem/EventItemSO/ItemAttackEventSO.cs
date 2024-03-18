using UnityEngine;

[CreateAssetMenu(fileName = "newAttackEventData", menuName = "Data/Item Data/ItemAttackEventSO Data")]
public class ItemAttackEventSO : ItemEventSO
{
    [Header("Attack Event")]
    public int AdditionalDamage;

    /// <summary>
    /// 고정 데미지량 계산여부(true 시 AddtionalDamage, false 시 Critical 및 원소속성 계산하여 데미지)
    /// </summary>
    public bool isCalFixedDamage;

    /// <summary>
    /// 공격 속성
    /// </summary>
    public DAMAGE_ATT DAMAGE_ATT;

    /// <summary>
    /// 온힛 여부
    /// </summary>
    public bool isOnHit;

    /// <summary>
    /// 크리티컬 가능 여부(크리티컬 확률 100%일 경우 무한히 공격하는 버그 방지)
    /// </summary>
    public bool CanCritical;

    /// <summary>
    /// 스스로에게 피해
    /// </summary>
    public bool isSelf_harm;

    /// <summary>
    /// 피해량 흡혈
    /// </summary>
    public bool isBloodsucking;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parentItem"></param>
    /// <param name="unit">피해자</param>
    /// <param name="enemy">가해자</param>
    private void AttackAction(StatsItemSO parentItem, Unit unit, Unit enemy = null)
    {

        //스스로에게 피해
        if (isSelf_harm)
        {
            SpawnVFX(unit);
            SpawnSFX(unit);

            if(DAMAGE_ATT == DAMAGE_ATT.Fixed)
            {
                //고정데미지
                unit.Core.CoreDamageReceiver.FixedDamage(AdditionalDamage, true);
            }
            else
            {
                unit.Core.CoreDamageReceiver.TrueDamage(
                    unit,
                    parentItem.StatsData.Elemental,
                    DAMAGE_ATT,
                    unit.Core.CoreUnitStats.CalculStatsData.DefaultPower + AdditionalDamage
                    );
            }
        }
        else
        {
            if (enemy == null)
                return;

            SpawnVFX(enemy);
            SpawnSFX(enemy);

            //온힛
            if (isOnHit)
                unit.Inventory.ItemOnHitExecute(unit, enemy);

            if (DAMAGE_ATT == DAMAGE_ATT.Fixed)
            {
                float temp = 0f;
                //치명타 확률 및 원소 속성 계산
                if(isCalFixedDamage)
                {
                    temp = enemy.Core.CoreDamageReceiver.TrueDamage(
                    unit,
                    parentItem.StatsData.Elemental,
                    DAMAGE_ATT,
                    unit.Core.CoreUnitStats.CalculStatsData.DefaultPower + AdditionalDamage,
                    CanCritical
                    );
                }
                else
                {
                    //고정데미지
                    temp = enemy.Core.CoreDamageReceiver.FixedDamage(unit, AdditionalDamage, true);
                }
                if (isBloodsucking)
                    unit.Core.CoreUnitStats.IncreaseHealth(temp);
            }
            else
            {
                float temp = 
                enemy.Core.CoreDamageReceiver.TrueDamage(
                    unit,
                    parentItem.StatsData.Elemental,
                    DAMAGE_ATT,
                    unit.Core.CoreUnitStats.CalculStatsData.DefaultPower + AdditionalDamage,
                    CanCritical
                    );
                if (isBloodsucking)
                    unit.Core.CoreUnitStats.IncreaseHealth(temp);
            }
        }
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
            AttackAction(parentItem, unit, enemy);
            itemEventSet.Count = 0;
            itemEventSet.startTime = GameManager.Inst.PlayTime;
        }

        return itemEventSet;
    }
}
