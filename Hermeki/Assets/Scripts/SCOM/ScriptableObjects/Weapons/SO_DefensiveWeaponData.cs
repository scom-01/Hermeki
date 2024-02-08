using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newDefensiveWeaponData", menuName = "Data/Weapon Data/Defensive Weapon")]
public class SO_DefensiveWeaponData : SO_WeaponData
{
    [SerializeField] private WeaponAttackDetails[] attackDetails;
    public WeaponAttackDetails[] AttackDetails { get => attackDetails; set => attackDetails = value; }
    private void OnEnable()
    {
        amountOfAttacks = attackDetails.Length;

        movementSpeed = new float[amountOfAttacks];

        for (int i = 0; i < amountOfAttacks; i++)
        {
            movementSpeed[i] = attackDetails[i].movementSpeed;
        }
    }
}
