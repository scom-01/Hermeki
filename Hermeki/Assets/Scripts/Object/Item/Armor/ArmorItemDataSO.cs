using UnityEngine;

[CreateAssetMenu(fileName = "newArmorItemDataSO", menuName = "Data/Equip Data/Armor Item Data")]
public class ArmorItemDataSO : EquipItemDataSO
{
    public ArmorStyle Style;
    public Rect Collider_Rect;
    public ArmorItemDataSO(Item_Type _type) : base(_type)
    {
        ItemType = Item_Type.Armor;
    }
}