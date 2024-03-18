using UnityEngine;

public class RuneEquipObject : EquipObject
{
    public RuneEquipObject(EquipItemData data) : base(data)
    {
        Type = Item_Type.Rune;
    }

    public ItemObject itemObject => this.transform.parent.GetComponentInChildren<ItemObject>();
    public override void Interactive(Unit unit)
    {
    }
    public override void UnInteractive(Unit unit)
    {
    }
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
