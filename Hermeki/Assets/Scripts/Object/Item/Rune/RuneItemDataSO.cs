using System;
using UnityEngine;

[Serializable]
public struct EnchantMethod
{
    public AllItemStyle ItemStyle;
    /// <summary>
    /// 결과물
    /// </summary>
    public EquipItemDataSO Result;
}

[CreateAssetMenu(fileName = "new_RuneItemDataSO", menuName = "Data/Equip Data/Rune Item Data")]
public class RuneItemDataSO : EquipItemDataSO
{
    public EnchantMethod[] enchantMethods;
    public RuneItemDataSO(Item_Type _type) : base(_type)
    {
        ItemType = Item_Type.Rune;
    }
}
