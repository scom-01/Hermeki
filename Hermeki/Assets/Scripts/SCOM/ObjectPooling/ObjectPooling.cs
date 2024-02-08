using System.Collections.Generic;
using UnityEngine;


public class ObjectPooling : MonoBehaviour
{
    public GameObject Object;
    public int MaxPoolAmount;
    protected Queue<GameObject> ObjectQueue = new Queue<GameObject>();
    public virtual GameObject CreateObject(Vector3 size, bool active = false)
    {
        var newobj = Instantiate(Object, transform);
        newobj.transform.localScale = new Vector3(size.x, size.y, size.z);
        newobj.gameObject.SetActive(active);
        return newobj;
    }

    public virtual void Init(GameObject _obj, int count, Vector3 size)
    {
        if (_obj == null)
            return;
        if (size == Vector3.zero)
            size = Vector3.one;

        Object = _obj;
        MaxPoolAmount = count;

        for (int i = 0; i < MaxPoolAmount; i++)
        {
            ObjectQueue.Enqueue(CreateObject(size));
        }
    }

    public virtual GameObject GetObejct(Vector3 pos, Quaternion quaternion, Vector3 size)
    {
        if (ObjectQueue.Count > 0)
        {
            var obj = ObjectQueue.Dequeue();
            if (obj != null)
            {
                obj.transform.SetPositionAndRotation(pos, quaternion);
                obj.gameObject.SetActive(true);
            }
            return obj;
        }
        else
        {
            var newobj = CreateObject(size);
            newobj.transform.SetPositionAndRotation(pos, quaternion);
            newobj.gameObject.SetActive(true);
            return newobj;
        }
    }

    public virtual void ReturnObject(GameObject obj)
    {
        if (ObjectQueue.Count >= MaxPoolAmount)
        {
            Destroy(obj.gameObject);
        }
        else
        {
            obj.gameObject.SetActive(false);
            ObjectQueue.Enqueue(obj);
        }
    }
}
