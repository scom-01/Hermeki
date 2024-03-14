using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SpawnPos
{
    public GameObject Obj;
    public Transform Pos;
}

public class SpawnStartObject : EndActionEvent
{
    public List<SpawnPos> SpawnObjList = new List<SpawnPos>();
    public List<GameObject> SpawnedObject = new List<GameObject>();
    private bool isSpawned = false;

    protected override void Start()
    {
        base.Start();
        this.gameObject.layer = LayerMask.NameToLayer("Area");
    }

    protected override void Action()
    {
        base.Action();
        ClearObj();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag(this.tag) || isSpawned)
        {
            return;
        }

        SpawnObj();
        isSpawned = true;
    }

    private void SpawnObj()
    {
        for (int i = 0; i < SpawnObjList.Count; i++)
        {
            if (SpawnObjList[i].Obj == null)
                continue;
            GameObject _obj = Instantiate(SpawnObjList[i].Obj);
            _obj.transform.position = SpawnObjList[i].Pos.position;
            SpawnedObject.Add(_obj);
        }
    }

    private void ClearObj()
    {
        for (int i = 0; i < SpawnedObject.Count; i++)
        {
            Destroy(SpawnedObject[i].gameObject);
        }
        SpawnedObject = new List<GameObject>();
    }
}
