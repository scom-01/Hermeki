using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [HideInInspector]
    public SpriteRenderer SR;
    protected Rigidbody2D rb2d;
    protected PolygonCollider2D pc2d => GetComponent<PolygonCollider2D>();
    protected EquipItemData data;
    [SerializeField]
    protected bool isTrigger;
    protected virtual void Awake()
    {
        SR = this.GetComponent<SpriteRenderer>();
        rb2d = this.GetComponent<Rigidbody2D>();
        if (pc2d == null)
            this.AddComponent<PolygonCollider2D>();
        SetPolygon();
    }

    public void SetSpriteRenderer(EquipItemData _data)
    {
        if (_data == null)
            return;
        this.data = _data;
        SR.sprite = data.CalculateSprite()[0];
        SetPolygon();
    }

    /// <summary>
    /// Sprite Polygon Update
    /// </summary>
    protected virtual void SetPolygon()
    {
        if (SR?.sprite == null)
            return;

        if (pc2d == null)
            this.AddComponent<PolygonCollider2D>();
        PolygonCollider2D polygon = pc2d;

        int shapeCount = SR.sprite.GetPhysicsShapeCount();
        polygon.pathCount = shapeCount;
        var points = new List<Vector2>(64);
        for (int i = 0; i < shapeCount; i++)
        {
            SR.sprite.GetPhysicsShape(i, points);
            polygon.SetPath(i, points);
        }
        polygon.isTrigger = isTrigger;
        if (data?.dataSO != null)
        {
            polygon.sharedMaterial = data.dataSO.PM2D;
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
