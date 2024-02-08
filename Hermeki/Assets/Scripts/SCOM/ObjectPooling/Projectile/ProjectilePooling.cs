using SCOM;
using UnityEngine;

public class ProjectilePooling : ObjectPooling
{
    public ProjectileData m_ProjectileData;

    public override GameObject CreateObject(Vector3 size, bool active = false)
    {
        Projectile obj = Instantiate(Object, transform).GetComponent<Projectile>();
        var projectile_Data = m_ProjectileData;
        obj.Init(projectile_Data);
        obj.gameObject.SetActive(active);
        return obj.gameObject;
    }

    public void Init(GameObject _obj, int count)
    {
        Object = _obj;
        MaxPoolAmount = count;

        for (int i = 0; i < MaxPoolAmount; i++)
        {
            ObjectQueue.Enqueue(CreateObject(m_ProjectileData.EffectScale));
        }
    }

    public GameObject GetObejct(Unit unit, ProjectileData _projectilData)
    {
        if (ObjectQueue.Count > 0)
        {
            var obj = ObjectQueue.Dequeue();
            if(obj != null)
            {
                obj.GetComponent<Projectile>().SetUp(unit, _projectilData);
                obj.gameObject.SetActive(true);
                obj.GetComponent<Projectile>().Shoot();
            }            
            return obj;
        }
        else
        {
            var newobj = CreateObject(Vector3.one);
            newobj.GetComponent<Projectile>().SetUp(unit, _projectilData);
            newobj.gameObject.SetActive(true);
            newobj.GetComponent<Projectile>().Shoot();
            return newobj;
        }
    }
}