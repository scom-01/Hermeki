using UnityEngine;

public class WeaponEquipObject : EquipObject
{
    public Unit targetUnit;
    public WeaponEquipObject(EquipItemData data) : base(data)
    {
    }

    public ItemObject itemObject => this.transform.GetComponentInParent<ItemObject>();
    public override void Interactive(Unit unit)
    {
        Debug.Log($"Weapon Equip Interactive {unit.name}");
        unit.ItemManager?.AddWeaponItem(Data);
        this.GetComponentInParent<Rigidbody2D>().gameObject.SetActive(false);
    }

    public override void UnInteractive(Unit unit)
    {
        Debug.Log($"Weapon Equip UnInteractive {unit.name}");  
    }
    [ContextMenu("SetSpriteRenderer")]
    public override void SetSpriteRenderer()
    {
        if (itemObject == null || Data.dataSO == null || Data.CurrentDurability == 0)
        {
            this.GetComponentInParent<Rigidbody2D>().gameObject.SetActive(false);
            return;
        }

        if (!this.GetComponentInParent<Rigidbody2D>().gameObject.activeSelf)
            this.GetComponentInParent<Rigidbody2D>().gameObject.SetActive(true);

        itemObject.SetSpriteRenderer(Data);
    }
}
