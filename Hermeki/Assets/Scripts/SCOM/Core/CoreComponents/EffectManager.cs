using Cinemachine;
using SCOM.Weapons.Components;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using System.Drawing;

namespace SCOM.CoreSystem
{
    public class EffectManager : CoreComponent
    {
        public List<ObjectPooling> ObjectPoolList = new List<ObjectPooling>();

        protected override void Awake()
        {
            base.Awake();
        }

        #region Effects
        public GameObject StartEffects(GameObject effectPrefab, Vector2 pos, Quaternion rot, Vector3 size, bool _isFollow = false)
        {
            if (effectPrefab == null)
                return null;
            if (effectPrefab.GetComponent<EffectController>() == null)
            {
                effectPrefab.AddComponent<EffectController>();
            }

            if (size == Vector3.zero)
                size = effectPrefab.gameObject.transform.localScale;

            if (effectPrefab.GetComponent<EffectController>().isDestroy)
            {
                GameObject go = Instantiate(effectPrefab, pos, rot, _isFollow ? this.transform : GameManager.Inst.StageManager.EffectContainer.transform);
                go.GetComponent<EffectController>().size = size;
                return go;
            }
            else
            {
                if (_isFollow)
                    return (CheckObject(ObjectPooling_TYPE.Effect, effectPrefab, size) as EffectPooling).GetObejct(pos, rot, size);
                else
                    return (FindObjectOfType<EffectContainer>()?.CheckObject(ObjectPooling_TYPE.Effect, effectPrefab, size, FindObjectOfType<EffectContainer>().transform) as EffectPooling).GetObejct(pos, rot, size);
                    //return (GameManager.Inst.StageManager.EffectContainer?.CheckObject(ObjectPooling_TYPE.Effect, effectPrefab, size, GameManager.Inst.StageManager.EffectContainer.transform) as EffectPooling).GetObejct(pos, rot, size);
            }
        }
        public GameObject StartEffectsPos(GameObject effectPrefab, Vector2 pos, bool _isFollow = false)
        {
            if(effectPrefab == null)
            {
                return null;
            }
            return StartEffects(effectPrefab, pos, effectPrefab.transform.eulerAngles, effectPrefab.transform.localScale, _isFollow);            
        }
        public GameObject StartEffectsPos(GameObject effectPrefab, Vector2 pos, Vector3 size, bool _isFollow = false)
        {
            return StartEffects(effectPrefab, pos, effectPrefab.transform.eulerAngles, size, _isFollow);            
        }
        public GameObject StartEffects(GameObject effectPrefab, Vector2 pos, Vector3 euler, Vector3 size, bool _isFollow = false)
        {
            var temp = euler;
            if(core.CoreMovement.FancingDirection < 0)
            {
                temp = new Vector3(euler.x, euler.y + 180f, euler.z);
            }
            return StartEffects(effectPrefab, pos, Quaternion.Euler(temp), size, _isFollow);            
        }

        public GameObject StartEffects(GameObject effectPrefab, Vector3 size)
        {
            return StartEffects(effectPrefab, transform.position, effectPrefab.transform.eulerAngles, size, GameManager.Inst.StageManager.EffectContainer.transform);            
        }

        public GameObject StartEffectsWithRandomRot(GameObject effectPrefab)
        {
            var randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0, 360f));
            return StartEffects(effectPrefab, transform.position, randomRotation, Vector3.zero);
        }

        public GameObject StartEffectsWithRandomPos(GameObject effectPrefab, float Range, Vector3 size, Vector2 pos)
        {
            if (core.CoreMovement.FancingDirection > 0)
            {
                return StartEffects(effectPrefab, new Vector2(
                                                    pos.x + Random.Range(-Range, Range),
                                                    pos.y + Random.Range(-Range, Range)),
                                                    Quaternion.Euler(effectPrefab.transform.eulerAngles),
                                                    size);
            }

            return StartEffects(effectPrefab, new Vector2(
                                                    pos.x + Random.Range(-Range, Range),
                                                    pos.y + Random.Range(-Range, Range)),
                                                    Quaternion.Euler(effectPrefab.transform.eulerAngles.x, effectPrefab.transform.eulerAngles.y + 180.0f, effectPrefab.transform.eulerAngles.z),
                                                    size);            
        }
        public GameObject StartEffectsWithRandomPos(GameObject effectPrefab, float Range, int FancingDirection, Vector3 size, bool _isFollow = false)
        {
            if (FancingDirection > 0)
            {
                return StartEffects(effectPrefab, new Vector2(
                                                    transform.position.x + Random.Range(-Range, Range),
                                                    transform.position.y + Random.Range(-Range, Range)),
                                                    Quaternion.Euler(effectPrefab.transform.eulerAngles),
                                                    size,
                                                    _isFollow);
            }

            return StartEffects(effectPrefab, new Vector2(
                                                    transform.position.x + Random.Range(-Range, Range),
                                                    transform.position.y + Random.Range(-Range, Range)),
                                                    Quaternion.Euler(effectPrefab.transform.eulerAngles.x, effectPrefab.transform.eulerAngles.y + 180.0f, effectPrefab.transform.eulerAngles.z),
                                                    size,
                                                    _isFollow);
        }

        public GameObject StartEffectsWithRandomPosRot(GameObject effectPrefab, float Range, Vector3 size, bool _isFollow = false)
        {
            var randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0, 360f));
            return StartEffects(effectPrefab, new Vector2(
                                                    transform.position.x + Random.Range(-Range, Range),
                                                    transform.position.y + Random.Range(-Range, Range)),
                                                    randomRotation,
                                                    size, _isFollow);
        }

        public GameObject StartProjectileCheck(Unit _unit, ProjectileActionData _projectileActionData)
        {
            return FindObjectOfType<EffectContainer>().CheckProjectile(_projectileActionData.Projectile).GetObejct(_unit, _projectileActionData.ProjectileData);
            //return GameManager.Inst.StageManager.EffectContainer?.CheckProjectile(_projectileActionData.Projectile).GetObejct(_unit, _projectileActionData.ProjectileData);
        }
        #endregion


        private ObjectPooling CheckObject(ObjectPooling_TYPE objectPooling_TYPE, GameObject _obj, Vector3 size)
        {
            if (ObjectPoolList.Count == 0)
            {
                var obj = AddObject(objectPooling_TYPE, _obj, 5, size).GetComponent<EffectPooling>();
                return obj;
            }

            for (int i = 0; i < ObjectPoolList.Count; i++)
            {
                if (ObjectPoolList[i].Object == _obj)
                {
                    return ObjectPoolList[i];
                }
            }

            var newObj = AddObject(objectPooling_TYPE, _obj, 5, size).GetComponent<EffectPooling>();
            return newObj;
        }
        private GameObject AddObject(ObjectPooling_TYPE objectPooling_TYPE, GameObject _Obj, int amount, Vector3 size)
        {
            var effectPool = Instantiate(GlobalValue.Base_EffectPooling, this.transform);
            effectPool.GetComponent<EffectPooling>().Init(_Obj, amount, size);
            ObjectPoolList.Add(effectPool.GetComponent<EffectPooling>());
            return effectPool;
        }
    }
}