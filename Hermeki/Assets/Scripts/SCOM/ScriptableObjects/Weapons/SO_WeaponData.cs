using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SCOM.Weapons.Components;
[CreateAssetMenu(fileName ="newWeaponData",menuName ="Data/Weapon Data/Basic Weapon Data",order = 0)]
public class SO_WeaponData : ScriptableObject
{
    [field: SerializeField]
    [Tooltip("공격 모션 수")]
    public int amountOfAttacks;

    [Tooltip("콤보 공격 초기화 시간")]
    public float actionCounterResetCooldown;
    [Tooltip("공격 시 VelocityX movement 값")]
    public float[] movementSpeed { get; protected set; }

    [Header("Skill polymorphism")]
    [Tooltip("공격 중 점프")]
    public bool CanJump;
    [Tooltip("공중 공격")]
    public bool CanAirAttack;

    /*[field: SerializeReference] public List<ComponentData> ComponentData { get; private set; }

    public T GetData<T>()
    {
        return ComponentData.OfType<T>().FirstOrDefault();
    }

    public void AddData(ComponentData data)
    {
        if (ComponentData.FirstOrDefault(t => t.GetType() == data.GetType()) != null)
        {
            return;
        }

        ComponentData.Add(data);
    }*/
}
