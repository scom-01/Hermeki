using UnityEngine;


public class EffectPooling : ObjectPooling
{
    public override GameObject CreateObject(Vector3 size, bool active = false)
    {
        var newobj = Instantiate(Object, transform);
        newobj.GetComponent<EffectController>().size = size;
        newobj.gameObject.SetActive(active);
        return newobj;
    }

    public override GameObject GetObejct(Vector3 pos, Quaternion quaternion, Vector3 size)
    {
        var _size = size;
        if(size == Vector3.zero)
        {
            _size = Vector3.one;
        }
        if(ObjectQueue.Count > 0)
        {
            var obj = ObjectQueue.Dequeue();
            if(obj != null)
            {
                obj.transform.SetPositionAndRotation(pos, quaternion);
                obj.GetComponent<EffectController>().size = size;
                obj.GetComponent<EffectController>().isInit = true;
                obj.gameObject.SetActive(true);
            }
            return obj;
        }
        else
        {
            var newobj = CreateObject(_size, true);
            newobj.transform.SetPositionAndRotation(pos, quaternion);
            newobj.GetComponent<EffectController>().size = size;
            newobj.GetComponent<EffectController>().isInit = true;
            newobj.gameObject.SetActive(true);
            return newobj;
        }
    }

    public override void ReturnObject(GameObject obj)
    {
        if (ObjectQueue.Count >= MaxPoolAmount)
        {
            Destroy(obj.gameObject);
        }
        else
        {
            obj.GetComponent<EffectController>().isInit = true;
            obj.gameObject.SetActive(false);
            ObjectQueue.Enqueue(obj);
        }
    }
}
