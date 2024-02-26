using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    private SpriteRenderer sr;
    private Rigidbody2D rb2d;
    private PolygonCollider2D pc2d => GetComponent<PolygonCollider2D>();
    public WeaponItemDataSO dataSO;
    private void Awake()
    {
        sr = this.GetComponent<SpriteRenderer>();
        rb2d = this.GetComponent<Rigidbody2D>();
        SetPolygon();
    }

    private void SetPolygon()
    {
        if (sr == null && pc2d == null)
            return;

        if (pc2d == null)
            this.AddComponent<PolygonCollider2D>().isTrigger = false;
        PolygonCollider2D polygon = pc2d;

        int shapeCount = sr.sprite.GetPhysicsShapeCount();
        polygon.pathCount = shapeCount;
        var points = new List<Vector2>(64);
        for (int i = 0; i < shapeCount; i++)
        {
            sr.sprite.GetPhysicsShape(i, points);
            polygon.SetPath(i, points);
        }
        polygon.isTrigger = false;
        if (dataSO != null)
        {
            polygon.sharedMaterial = dataSO.PM2D;
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
