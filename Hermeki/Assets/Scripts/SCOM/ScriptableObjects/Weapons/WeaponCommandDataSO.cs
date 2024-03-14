using SCOM;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newWeaponCommandData", menuName = "Data/Weapon Data/WeaponCommand Data")]
public class WeaponCommandDataSO : ScriptableObject
{
    [field: SerializeField] public int WeaponIdx;
    [field: SerializeField] public int NumberOfActions { get; private set; }
    [Tooltip("Weapon Image")]
    [field: SerializeField] public Sprite WeaponImg { get; private set; }
    [Tooltip("Weapon Name")]
    [field: SerializeField] public string WeaponName { get; private set; }
    //[field: SerializeField] public LocalizedString WeaponNameLocal { get; private set; }
    [Tooltip("Weapon Elemental Power")]
    [field: SerializeField] public E_Power Elelemental_Power { get; private set; }
    [Tooltip("Weapon ClassLevel")]
    [field: Min(1)]
    [field: Tooltip("Min = 1")]
    [field: SerializeField] public int WeaponClassLevel { get; private set; }

    [Tooltip("충돌방지 위한 기본 OverrideController")]
    [field: SerializeField] public AnimatorOverrideController DefaultAnimator;
    [Tooltip("충돌방지 위한 기본 WeaponDataSO")]
    [field: SerializeField] public WeaponDataSO DefaultWeaponDataSO;

    //[Tooltip("현재 이 무기로 재련하기 위해 필요한 재화")]
    //[field: SerializeField] public Goods_Data GoodsCost;
    [Tooltip("업그레이드 시 변경할 CommandDataSO  순서는 Elemental 순서 (1.Water > 2.Earth > 3.Wind > 4.Fire > Water)")]
    [field: SerializeField] public List<WeaponCommandDataSO> UpgradeWeaponCommandDataSO = new List<WeaponCommandDataSO>();

    [field: SerializeField] public List<CommandList> GroundedCommandList;
    [field: SerializeField] public List<CommandList> AirCommandList;

    [Header("Skill Data")]
    public WeaponSkillData PrimarySkillData;
    public WeaponSkillData SecondarySkillData;
}