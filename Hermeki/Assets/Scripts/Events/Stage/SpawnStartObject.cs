using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
[Serializable]
public struct AddressabelSpawnPos
{
    public AssetReferenceGameObject AddressObj;
    public Transform Pos;
}

public class SpawnStartObject : EndActionEvent
{
    public List<AddressabelSpawnPos> AddressableSpawnObjList = new List<AddressabelSpawnPos>();
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
        if (!collision.CompareTag(this.tag) || isSpawned)
        {
            return;
        }

        SpawnObj();
        isSpawned = true;
    }

    private void SpawnObj()
    {
        for (int i = 0; i < AddressableSpawnObjList.Count; i++)
        {
            //if (AddressableSpawnObjList[i].AddressObj == null)
            //    continue;

            AddressableSpawnObjList[i].AddressObj.InstantiateAsync(AddressableSpawnObjList[i].Pos).Completed +=
                (AsyncOperationHandle<GameObject> _obj) =>
                {
                    SpawnedObject.Add(_obj.Result);
                };
        }
    }

    private void ClearObj()
    {
        for (int i = 0; i < SpawnedObject.Count; i++)
        {
            if (!Addressables.ReleaseInstance(SpawnedObject[i]))
            {
                Destroy(SpawnedObject[i].gameObject);
            }
        }
        SpawnedObject = new List<GameObject>();
    }
}
