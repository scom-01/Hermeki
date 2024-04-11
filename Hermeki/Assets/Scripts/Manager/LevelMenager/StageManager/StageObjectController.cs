using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class StageObjectController : MonoBehaviour
{
    public AssetReferenceGameObject Base_ArmorEquipObject, Base_WeaponEquipObject, Base_RuneEquipObject;
    public List<GameObject> ObjectList;
    private StageController _stageController;
    private int _stageLevel;

    private void Awake()
    {
        _stageController = GetComponentInParent<StageController>();
    }
    public GameObject GetSpawnItem(EquipItemData _data, Vector3 pos)
    {
        AsyncOperationHandle<GameObject> opHandle = new AsyncOperationHandle<GameObject>();
        
        EquipItemData temp = new EquipItemData(_data.dataSO, _data.CurrentDurability);
        switch (temp.dataSO.ItemType)
        {
            case Item_Type.Armor:
                opHandle = Addressables.LoadAssetAsync<GameObject>(Base_ArmorEquipObject);//.LoadAssetAsync();
                break;
            case Item_Type.Weapon:
                opHandle = Addressables.LoadAssetAsync<GameObject>(Base_WeaponEquipObject);//.LoadAssetAsync();
                break;
            case Item_Type.Rune:
                opHandle = Addressables.LoadAssetAsync<GameObject>(Base_RuneEquipObject);//.LoadAssetAsync();
                break;
            default:
                break;
        }

        opHandle.WaitForCompletion(); // Returns when operation is complete

        if (opHandle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject result = Instantiate(opHandle.Result, transform);
            result.GetComponentInChildren<EquipObject>().SetData(temp);
            result.transform.position = pos;
            ObjectList.Add(result);
            return result;
        }
        else
        {
            Addressables.Release(opHandle);
            return null;
        }
    }

    public bool SpawnItem(EquipItemData _data, Vector3 pos)
    {
        switch (_data.dataSO.ItemType)
        {
            case Item_Type.Armor:
                Base_ArmorEquipObject.InstantiateAsync(this.transform).Completed +=
            (AsyncOperationHandle<GameObject> _obj) =>
            {
                _obj.Result.GetComponentInChildren<EquipObject>().SetData(_data);
                _obj.Result.transform.position = pos;
                ObjectList.Add(_obj.Result);
            };
                break;
            case Item_Type.Weapon:
                Base_WeaponEquipObject.InstantiateAsync(this.transform).Completed +=
            (AsyncOperationHandle<GameObject> _obj) =>
            {
                _obj.Result.GetComponentInChildren<EquipObject>().SetData(_data);
                _obj.Result.transform.position = pos;
                ObjectList.Add(_obj.Result);
            };
                break;
            case Item_Type.Rune:
                Base_RuneEquipObject.InstantiateAsync(this.transform).Completed +=
            (AsyncOperationHandle<GameObject> _obj) =>
            {
                _obj.Result.GetComponentInChildren<EquipObject>().SetData(_data);
                _obj.Result.transform.position = pos;
                ObjectList.Add(_obj.Result);
            };
                break;
            default:
                break;
        }

        return true;
    }

    public bool SpawnUnit(AssetReferenceGameObject obj, Vector3 pos)
    {
        if (obj == null)
            return false;
        obj.InstantiateAsync(this.transform).Completed +=
            (AsyncOperationHandle<GameObject> _obj) =>
            {
                _obj.Result.transform.position = pos;
                ObjectList.Add(_obj.Result);
            };

        return true;
    }

    public GameObject GetObjectContain(EquipItemData _data)
    {
        for (int i = 0; i < ObjectList.Count; i++)
        {
            if (ObjectList[i].GetComponentInChildren<EquipObject>().Data == _data)
            {
                return ObjectList[i];
            }
        }
        return null;
    }
    public bool ClearObject()
    {
        foreach (var item in ObjectList)
        {
            Addressables.ReleaseInstance(item);
        }
        ObjectList.Clear();
        return true;
    }
}
