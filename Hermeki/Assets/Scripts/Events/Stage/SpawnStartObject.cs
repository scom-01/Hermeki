using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
[Serializable]
public class AddressabelSpawnPos
{
    public AssetReferenceGameObject AddressObj;
    public Transform Pos;

    public AddressabelSpawnPos(AssetReferenceGameObject obj, Transform pos)
    {
        AddressObj = obj;
        Pos = pos;
    }
}

public class SpawnStartObject : ActionEvent
{
    public List<AddressabelSpawnPos> AddressableSpawnObjList = new List<AddressabelSpawnPos>();
    public List<GameObject> SpawnedObject = new List<GameObject>();
    private bool isSpawned = false;
    private List<Transform> ChildTransformList = new List<Transform>();

    private void Awake()
    {
        ChildTransformList = this.GetComponentsInChildren<Transform>().ToList();
        ChildTransformList.Remove(this.transform);
    }

    protected override void Start()
    {
        base.Start();
        this.gameObject.layer = LayerMask.NameToLayer("Area");
    }

    protected override void UnAction()
    {
        ClearObj();
        isSpawned = false;
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
    public void SetSpawnObjList(List<AssetReferenceGameObject> objList)
    {
        AddressableSpawnObjList = new List<AddressabelSpawnPos>();

        if (objList == null || objList.Count == 0)
            return;

        for (int i = 0; i < objList.Count; i++)
        {
            if (i >= objList.Count || i >= ChildTransformList.Count)
                return;

            if (objList[i] == null)
                return;

            AddressableSpawnObjList.Add(new AddressabelSpawnPos(objList[i], ChildTransformList[i]));
        }
    }

    private void SpawnObj()
    {
        for (int i = 0; i < AddressableSpawnObjList.Count; i++)
        {
            AddressableSpawnObjList[i].AddressObj.InstantiateAsync(AddressableSpawnObjList[i].Pos).Completed +=
                (AsyncOperationHandle<GameObject> _obj) =>
                {
                    if (_obj.Result.CompareTag("Item") && _obj.Result.GetComponentInChildren<EquipObject>() != null)
                    {
                        _obj.Result.GetComponentInChildren<EquipObject>().isEquipable = true;
                    }
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
