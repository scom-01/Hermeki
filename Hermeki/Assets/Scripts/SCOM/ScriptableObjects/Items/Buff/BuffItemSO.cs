using System;
using UnityEngine;

[CreateAssetMenu(fileName = "newItemData", menuName = "Data/Item Data/Buff Data")]
public class BuffItemSO : ItemDataSO
{
    [Header("--StatsData--")]
    [Tooltip("아이템이 갖는 스탯")]
    public StatsData StatsData = new StatsData();
    [Tooltip("최대체력이 아닌 현재 체력 증가값")]
    public int Health;

    [Header("--Effects--")]
    public EffectData InitEffectData;

    public BuffItem_Data BuffData;
}

[Serializable]
public struct BuffItem_Data
{
    [Tooltip("버프 지속시간, 999f = Infinity")]
    public float DurationTime;
    [Tooltip("버프 타입, Active = 지속 시간 유한, Passive = 지속 시간 무한")]
    public EVENT_BUFF_TYPE BuffType;
    [Tooltip("중복여부")]
    public bool isOverlap;
    [Tooltip("중복 카운트")]
    [Range(1,99)] public int BuffCountMax;
    [Tooltip("중복 시 지속시간 초기화")]
    public bool isBuffInit;
}