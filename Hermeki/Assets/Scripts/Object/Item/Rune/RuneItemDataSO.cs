using UnityEngine;

[CreateAssetMenu(fileName = "new_RuneItemDataSO", menuName = "Data/Equip Data/Rune Item Data")]
public class RuneItemDataSO : EquipItemDataSO
{
    public RuneItemDataSO(Item_Type _type) : base(_type)
    {
        ItemType = Item_Type.Rune;
    }
}
