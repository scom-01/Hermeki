using System.Collections.Generic;
using UnityEngine;

public class EffectContainer : MonoBehaviour
{
    public List<ProjectilePooling> ProjectilePoolingList = new List<ProjectilePooling>();
    public List<EffectPooling> EffectPoolingList = new List<EffectPooling>();
    public List<DmgTxtPooling> DmgTxtPoolingList = new List<DmgTxtPooling>();

    public ProjectilePooling CheckProjectile(GameObject _projectile)
    {
        if (ProjectilePoolingList.Count == 0)
        {
            var projectilePooling = AddProjectile(_projectile, 5);
            return projectilePooling.GetComponent<ProjectilePooling>();
        }

        for (int i = 0; i < ProjectilePoolingList.Count; i++)
        {
            if (ProjectilePoolingList[i].Object == _projectile)
            {
                return ProjectilePoolingList[i];
            }
        }

        var newProjectile = AddProjectile(_projectile, 5).GetComponent<ProjectilePooling>();
        return newProjectile;
    }

    public GameObject AddProjectile(GameObject _projectile, int amount = 5)
    {
        var projectilePool = Instantiate(GlobalValue.Base_ProjectilePooling, this.transform);
        projectilePool.GetComponent<ProjectilePooling>().Init(_projectile, amount);
        ProjectilePoolingList.Add(projectilePool.GetComponent<ProjectilePooling>());
        return projectilePool;
    }

    public ObjectPooling CheckObject(ObjectPooling_TYPE objectPooling_TYPE, GameObject _obj, Vector3 size, Transform transform = null)
    {
        switch (objectPooling_TYPE)
        {
            case ObjectPooling_TYPE.Effect:
                if (EffectPoolingList.Count == 0)
                {
                    var obj = AddObject(objectPooling_TYPE, _obj, 5, size, transform).GetComponent<EffectPooling>();
                    return obj;
                }

                for (int i = 0; i < EffectPoolingList.Count; i++)
                {
                    if (EffectPoolingList[i].Object == _obj)
                    {
                        return EffectPoolingList[i];
                    }
                }

                var newObj = AddObject(objectPooling_TYPE, _obj, 5, size, transform).GetComponent<EffectPooling>();
                return newObj;
            case ObjectPooling_TYPE.Projectile:
                if (ProjectilePoolingList.Count == 0)
                {
                    var projectilePooling = AddProjectile(_obj, 5);
                    return projectilePooling.GetComponent<ProjectilePooling>();
                }

                for (int i = 0; i < ProjectilePoolingList.Count; i++)
                {
                    if (ProjectilePoolingList[i].Object == _obj)
                    {
                        return ProjectilePoolingList[i];
                    }
                }

                var newProjectile = AddProjectile(_obj, 5).GetComponent<ProjectilePooling>();
                return newProjectile;
            case ObjectPooling_TYPE.DmgText:
                if (DmgTxtPoolingList.Count == 0)
                {
                    var obj = AddObject(objectPooling_TYPE, _obj, 5, size, transform).GetComponent<DmgTxtPooling>();
                    return obj;
                }

                for (int i = 0; i < DmgTxtPoolingList.Count; i++)
                {
                    if (DmgTxtPoolingList[i].Object == _obj)
                    {
                        return DmgTxtPoolingList[i];
                    }
                }

                var DmgObj = AddObject(objectPooling_TYPE, _obj, 5, size, transform).GetComponent<DmgTxtPooling>();
                return DmgObj;
            default:
                return null;
        }
    }

    public GameObject AddObject(ObjectPooling_TYPE objectPooling_TYPE, GameObject _Obj, int amount, Vector3 size, Transform transform = null)
    {
        switch (objectPooling_TYPE)
        {
            case ObjectPooling_TYPE.Effect:
                var effectPool = Instantiate(GlobalValue.Base_EffectPooling, transform ? transform : this.transform);
                effectPool.GetComponent<EffectPooling>().Init(_Obj, amount, size);
                EffectPoolingList.Add(effectPool.GetComponent<EffectPooling>());
                return effectPool;
            case ObjectPooling_TYPE.Projectile:
                var projectilePool = Instantiate(GlobalValue.Base_ProjectilePooling, transform ? transform : this.transform);
                projectilePool.GetComponent<EffectPooling>().Init(_Obj, amount, size);
                ProjectilePoolingList.Add(projectilePool.GetComponent<ProjectilePooling>());
                return projectilePool;
            case ObjectPooling_TYPE.DmgText:
                var DmgTxtPool = Instantiate(GlobalValue.Base_DmgTxtPooling, transform ? transform : this.transform);
                DmgTxtPool.GetComponent<DmgTxtPooling>().Init(_Obj, amount, size);
                DmgTxtPoolingList.Add(DmgTxtPool.GetComponent<DmgTxtPooling>());
                return DmgTxtPool;
            default:
                return null;
        }
    }
}