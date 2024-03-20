using UnityEngine;

public class WeaponEquipObject : EquipObject
{
    public Unit targetUnit;
    public WeaponEquipObject(EquipItemData data) : base(data)
    {
        Type = Item_Type.Weapon;
    }

    public ItemObject itemObject => this.transform.parent.GetComponentInChildren<ItemObject>();
    public override void Interactive(Unit unit)
    {
        Debug.Log($"Weapon Equip Interactive {unit.name}");
        unit?.ItemManager?.AddEquipItem(Data);
        this.transform.parent.GetComponentInChildren<Rigidbody2D>().gameObject.SetActive(false);
    }

    public override void UnInteractive(Unit unit)
    {
        Debug.Log($"Weapon Equip UnInteractive {unit.name}");  
    }
    [ContextMenu("SetSpriteRenderer")]
    public override void SetSpriteRenderer()
    {
        if (Data.dataSO == null || Data.CurrentDurability == 0)
        {
            this.transform.parent.GetComponentInChildren<Rigidbody2D>().gameObject.SetActive(false);
            return;
        }

        if (!this.transform.parent.GetComponentInChildren<Rigidbody2D>().gameObject.activeSelf)
            this.transform.parent.GetComponentInChildren<Rigidbody2D>().gameObject.SetActive(true);

        if (itemObject != null)
            itemObject.SetSpriteRenderer(Data);
    }
}
