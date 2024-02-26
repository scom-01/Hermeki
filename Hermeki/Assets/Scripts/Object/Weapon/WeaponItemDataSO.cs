using SCOM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItemData
{
    public WeaponItemDataSO dataSO;
    public int CurrentDurability;
    public WeaponItemData(WeaponItemDataSO dataSO, int currentDurability)
    {
        this.dataSO = dataSO;
        CurrentDurability = currentDurability;
    }
}
[Serializable]
public struct WeaponItemSpriteData
{
    public Sprite sprite;
    public int durability;
}

[CreateAssetMenu(fileName = "newWeaponItemDataSO", menuName = "Data/Weapon Data/Weapon Item Data")]
public class WeaponItemDataSO : ScriptableObject
{
    [Header("Weapon")]
    public WeaponItemSpriteData[] WeaponSprite;
    public int MaxDurability;
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
