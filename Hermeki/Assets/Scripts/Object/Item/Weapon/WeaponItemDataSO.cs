using SCOM;
using UnityEngine;

[CreateAssetMenu(fileName = "newWeaponItemDataSO", menuName = "Data/Equip Data/Weapon Item Data")]
public class WeaponItemDataSO : EquipItemDataSO
{
    public WeaponStyle Style;
    /// <summary>
    /// 지형 충돌 여부
    /// </summary>
    public bool isPhysicsCheck_Ground = true;
    public bool isPhysicsCheck_Unit = true;

    [Header("Physics")]
    public PhysicsMaterial2D PM2D;
    [Header("Effect")]
    [Header("VFX")]
    public EffectPrefab Grounded_effectData;
    public EffectPrefab Unit_effectData;
    [Header("SFX")]
    public AudioData Grounded_audioData;
    public AudioData Unit_audioData;
}