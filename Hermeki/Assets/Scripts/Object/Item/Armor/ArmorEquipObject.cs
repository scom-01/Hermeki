using UnityEngine;

public class ArmorEquipObject : EquipObject
{
    public Unit targetUnit;
    public Rect rect;

    public ArmorEquipObject(EquipItemData data) : base(data)
    {
        Type = Item_Type.Armor;
    }

    public SpriteRenderer[] SpriteRenderers => GetComponentsInChildren<SpriteRenderer>();
    public override void Interactive(Unit unit)
    {
        Debug.Log($"Armor Equip Interactive {unit.name}");
        unit.ItemManager?.AddArmorItem(Data);
        this.GetComponentInParent<Rigidbody2D>().gameObject.SetActive(false);
    }
    public override void UnInteractive(Unit unit)
    {
        Debug.Log($"Armor Equip UnInteractive {unit.name}");
    }

    [ContextMenu("SetSpriteRenderer")]
    public override void SetSpriteRenderer()
    {
        if (SpriteRenderers == null || Data.dataSO == null || Data.CurrentDurability == 0)
        {
            this.GetComponentInParent<Rigidbody2D>().gameObject.SetActive(false);
            return;
        }

        if (!this.GetComponentInParent<Rigidbody2D>().gameObject.activeSelf)
            this.GetComponentInParent<Rigidbody2D>().gameObject.SetActive(true);

        if (GetComponentInParent<BoxCollider2D>() != null && (Data?.dataSO as ArmorItemDataSO) != null)
        {
            GetComponentInParent<BoxCollider2D>().offset = (Data?.dataSO as ArmorItemDataSO).Collider_Rect.position;
            GetComponentInParent<BoxCollider2D>().size = (Data?.dataSO as ArmorItemDataSO).Collider_Rect.size;
        }

        int idx = Data.dataSO.CalculateDurability(Data.CurrentDurability);

        for (int i = 0; i < SpriteRenderers.Length; i++)
        {
            SpriteRenderers[i].sprite = Data.dataSO.Sprite[idx].sprites[i];
        }
    }
}
