using SCOM;
using System;
using UnityEngine;

[Serializable]
public class ItemEventSet
{
    /// <summary>
    /// 아이템 획득 시 초기 1회
    /// </summary>
    public bool init = false;
    /// <summary>
    /// 호출 시 시간
    /// </summary>
    public float startTime = 0;
    /// <summary>
    /// 횟수 계산용 변수(OnAction or OnHit etc.)
    /// </summary>
    public int Count = 0;
    public ItemEventSet(bool init = false, float startTime = 0, int count = 0)
    {
        this.init = init;
        this.startTime = startTime;
        Count = count;
    }
    public bool Set(ItemEventSet item)
    {
        if (item == null)
            return false;
        this.init = item.init;
        this.startTime = item.startTime;
        this.Count = item.Count;
        return true;
    }
}

public abstract class ItemEventSO : ScriptableObject, IExecuteEvent
{
    public ItemEvent_Type Item_Type;
    public ItemEventData itemEventData;

    /// <summary>
    /// Effect 효과 시 생성될 VFX
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public GameObject SpawnVFX(Unit unit)
    {
        if (unit == null)
            return null;

        if (itemEventData.VFX.Object == null)
            return null;
        GameObject go = itemEventData.VFX.SpawnObject(unit);
        return go;
    }

    /// <summary>
    /// Effect 효과 시 생성될 SFX
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public bool SpawnSFX(Unit unit)
    {
        if (unit == null)
            return false;

        if (itemEventData.SFX.Clip == null)
            return false;

        unit.Core.CoreSoundEffect.AudioSpawn(itemEventData.SFX);

        return true;
    }

    public virtual ItemEventSet ExcuteEvent(ItemEvent_Type type, StatsItemSO parentItem, Unit unit, Unit enemy, ItemEventSet itemEventSet)
    {
        if (Item_Type != type || Item_Type == ItemEvent_Type.None || itemEventSet == null)
            return itemEventSet;

        return itemEventSet;
    }
    public virtual ItemEventSet ExcuteEvent(ItemEvent_Type type, EquipItemDataSO parentItem, Unit unit, Unit enemy, ItemEventSet itemEventSet)
    {
        if (Item_Type != type || Item_Type == ItemEvent_Type.None || itemEventSet == null)
            return itemEventSet;

        return itemEventSet;
    }
}

[Serializable]
public struct ItemEventData
{
    /// <summary>
    /// 필요 호출 회수
    /// </summary>
    [Range(1, 99)]
    public int MaxCount;
    /// <summary>
    /// 재사용 대기시간
    /// </summary>
    public float CooldownTime;

    /// <summary>
    /// 발동 확률
    /// </summary>
    [Range(0f, 100f)]
    public float Percent;

    /// <summary>
    /// Effect VFX
    /// </summary>
    public EffectPrefab VFX;
    /// <summary>
    /// Effect SFX
    /// </summary>
    public AudioData SFX;
}