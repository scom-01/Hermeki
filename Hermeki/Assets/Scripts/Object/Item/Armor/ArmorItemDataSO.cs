using UnityEngine;

[CreateAssetMenu(fileName = "newArmorItemDataSO", menuName = "Data/Equip Data/Armor Item Data")]
public class ArmorItemDataSO : EquipItemDataSO
{
    public ArmorStyle Style;
    [Header("Physics")]
    public PhysicsMaterial2D PM2D;
}