using System.Collections.Generic;
using UnityEngine;

public class ArmorEquipObject : EquipObject
{
    public Unit targetUnit;
    public Rect rect;
    public ArmorStyle armorStyle;
    public List<Transform> SpriteTransformList = new List<Transform>();
    public SpriteRenderer[] SpriteRenderers;
    public ArmorEquipObject(EquipItemData data) : base(data)
    {
        Type = Item_Type.Armor;
        
    }

    public override void Interactive(Unit unit)
    {
        Debug.Log($"Armor Equip Interactive {unit.name}");
        unit?.ItemManager?.AddEquipItem(Data);
        this.GetComponentInParent<Rigidbody2D>().gameObject.SetActive(false);
    }
    public override void UnInteractive(Unit unit)
    {
        Debug.Log($"Armor Equip UnInteractive {unit.name}");
    }

    public override bool SetData(EquipItemData _data, bool _isEquipable = false)
    {
        if (_data?.dataSO == null)
            return false;
        armorStyle = (_data.dataSO as ArmorItemDataSO).Style;
        return base.SetData(_data, _isEquipable);
    }
    [ContextMenu("SetSpriteRenderer")]
    public override void SetSpriteRenderer()
    {
        foreach (var _transform in SpriteTransformList)
        {
            _transform.gameObject.SetActive(false);
        }
        SpriteTransformList[(int)armorStyle].gameObject.SetActive(true);
        SpriteRenderers = SpriteTransformList[(int)armorStyle].gameObject.GetComponentsInChildren<SpriteRenderer>();

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

        for (int i = 0; i < SpriteRenderers.Length; i++)
        {
            if (Data.CalculateSprite() != null && i < Data.CalculateSprite().Length)
                SpriteRenderers[i].sprite = Data.CalculateSprite()[i];
        }
    }
}
