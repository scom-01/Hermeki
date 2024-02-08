using System;
using System.Collections.Generic;
using System.Linq;
using SCOM.Weapons.Components;
using UnityEngine;

[CreateAssetMenu(fileName = "newWeaponData", menuName = "Data/Weapon Data/Weapon Data")]
public class WeaponDataSO : ScriptableObject
{
    [Tooltip("UI 표시될 Sprite")]
    [field: SerializeField] public Sprite WeaponSprite;
    [field: SerializeField] public int NumberOfActions { get; private set; }
    [field: SerializeField] public bool CanJump { get; private set; }
    [field: SerializeField] public bool CanAirAttack { get; private set; }
    [field: SerializeReference] public List<ComponentData> ComponentData { get; private set; }

    public T GetData<T>()
    {
        return ComponentData.OfType<T>().FirstOrDefault();
    }

    public List<Type> GetAllDependencies()
    {
        return ComponentData.Select(component => component.ComponentDependency).ToList();
    }
    public void AddData(ComponentData data)
    {
        if (ComponentData.FirstOrDefault(t => t.GetType() == data.GetType()) != null)
            return;

        ComponentData.Add(data);
    }
}
