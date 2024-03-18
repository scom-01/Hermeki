using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class StageItemController : MonoBehaviour
{
    public AssetReferenceGameObject Base_ArmorEquipObject, Base_WeaponEquipObject, Base_RuneEquipObject;
    public List<GameObject> ObjectList;
    private StageController _stageController;
    private int _stageLevel;

    private void Awake()
    {
        _stageController = GetComponentInParent<StageController>();
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
