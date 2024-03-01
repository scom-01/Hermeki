using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [HideInInspector]
    public SpriteRenderer SR => this.GetComponent<SpriteRenderer>();
    private Rigidbody2D rb2d => this.GetComponent<Rigidbody2D>();
    private PolygonCollider2D pc2d => GetComponent<PolygonCollider2D>();
    private EquipItemData data;
    private void Awake()
    {
        //SR = this.GetComponent<SpriteRenderer>();
        //rb2d = this.GetComponent<Rigidbody2D>();
        SetPolygon();
    }

    public void SetSpriteRenderer(EquipItemData _data)
    {
        if (_data == null)
            return;
        this.data = _data;
        int idx = data.dataSO.CalculateDurability(data.CurrentDurability);
        SR.sprite = data.dataSO.Sprite[idx].sprites[0];
        SetPolygon();
    }

    private void SetPolygon()
    {
        if (SR == null && pc2d == null)
            return;

        if (pc2d == null)
            this.AddComponent<PolygonCollider2D>().isTrigger = false;
        PolygonCollider2D polygon = pc2d;

        int shapeCount = SR.sprite.GetPhysicsShapeCount();
        polygon.pathCount = shapeCount;
        var points = new List<Vector2>(64);
        for (int i = 0; i < shapeCount; i++)
        {
            SR.sprite.GetPhysicsShape(i, points);
            polygon.SetPath(i, points);
        }
        polygon.isTrigger = false;
        if (data != null)
        {
            polygon.sharedMaterial = (data.dataSO as WeaponItemDataSO).PM2D;
        }
    }

    private void FixedUpdate()
    {
        if (rb2d == null)
            return;
        Mathf.Clamp(rb2d.velocity.x, -15, 15);
        Mathf.Clamp(rb2d.velocity.y, -15, 15);
    }
}
