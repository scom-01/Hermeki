using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{
    private Unit unit;
    private SpriteRenderer sr => GetComponent<SpriteRenderer>();
    private PolygonCollider2D pc2D => GetComponent<PolygonCollider2D>();
    private void Awake()
    {
        unit = GetComponentInParent<Unit>();
        if (unit != null)
        {
            this.tag = unit.transform.tag;
        }
    }
    private void Start()
    {
        if (sr == null || sr.sprite == null)
            return;

        unit.Core.CoreUnitStats.OnHealthZero -= SpawnChunk;        
        unit.Core.CoreUnitStats.OnHealthZero += SpawnChunk;        
    }

    [ContextMenu("Spawn Chunk")]
    public void SpawnChunk()
    {
        if (sr == null || sr.sprite == null)
            return;

        GameObject obj = new GameObject();
        obj.layer = LayerMask.NameToLayer("Object");
        obj.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        obj.AddComponent<SpriteRenderer>().sprite = sr.sprite;
        obj.AddComponent<PolygonCollider2D>().isTrigger = false;

        obj.transform.position = this.transform.position;
        Destroy(obj, 2);
    }
}
