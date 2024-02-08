using System;
using SCOM.Weapons.Components;
using UnityEngine;

namespace SCOM
{
    [Serializable]
    public class GoodsData
    {
        [Tooltip("Goods Type")]
        [SerializeField] public GOODS_TPYE Goods;
        [Tooltip("개당 재화량, 총 재화 = Amount * DropCount")]
        /// <summary>
        /// 개당 재화량
        /// </summary>
        [SerializeField] public int Amount;
        [Tooltip("드랍될 재화아이템 갯수")]
        /// <summary>
        /// 드랍될 재화아이템 갯수
        /// </summary>
        [SerializeField] public int DropCount;
        //[HideInInspector] public GoodsSO SOdata;
        /// <summary>
        /// 재화의 삭제 딜레이 시간
        /// </summary>
        [SerializeField] public float DestroyTime;
        /// <summary>
        /// 재화의 삭제 딜레이 시간 랜덤 범위 (+- Random.Range)
        /// </summary>
        [SerializeField] public float DestroyTimeRange;
        public GoodsData(GOODS_TPYE goods, int amount, int dropCount, float destroyTime, float destroyTimeRange)
        {
            Goods = goods;
            Amount = amount;
            DropCount = dropCount;
            DestroyTime = destroyTime;
            DestroyTimeRange = destroyTimeRange;
        }
    }

    [Serializable]
    public struct WeaponSkillData
    {
        public WeaponAnimData GroundweaponData;
        public WeaponAnimData AirweaponData;
        [Min(0f)]
        public float SkillCoolTime;
        public Sprite SkillSprite;
    }
    [Serializable]
    public struct WeaponAnimData
    {
        public AnimatorOverrideController AnimOC;
        public WeaponDataSO WeaponDataSO;
    }
    
    [Serializable]
    public struct WeaponData
    {
        public WeaponCommandDataSO weaponCommandDataSO;
        public WeaponDataSO weaponDataSO;
    }

    [Serializable]
    public struct AudioPrefab
    {
        public AudioClip Clip;
        [Range(0,1)]
        public float Volume;
        public AudioPrefab(AudioClip clip, float volume)
        {
            Clip = clip;
            Volume = volume;
        }
    }

    [Serializable]
    public struct EffectPrefab
    {
        [Tooltip("유닛의 바닥에서 스폰")]
        /// <summary>
        /// Spawn Pos isGrounded
        /// </summary>
        public bool isGround;
        [Tooltip("유닛의 머리에서 스폰")]
        /// <summary>
        /// Spawn Pos isGrounded
        /// </summary>
        public bool isHeader;
        [Tooltip("스폰 위치를 유닛 기준이 아닌 오브젝트의 위치로 스폰")]
        public bool isTransformGlobal;
        [Tooltip("랜덤 위치 스폰")]
        /// <summary>
        /// Spawn Pos isRandom
        /// </summary>
        public bool isRandomPosRot;
        [Tooltip("랜덤 위치 범위")]
        /// <summary>
        /// Spawn Pos RandomRange
        /// </summary>
        public float isRandomRange;
        [Tooltip("이펙트의 Offset")]
        /// <summary>
        /// Effect Offset
        /// </summary>
        public Vector2 EffectOffset;
        /// <summary>
        /// Additional Effect Scale
        /// </summary>
        [field: Tooltip("This Additional float , ex.1) default 1 + additionalScale , ex.2) EffectScale(1) = 2")]
        public Vector3 EffectScale;
        [Tooltip("유닛의 하위 오브젝트로 스폰")]
        /// <summary>
        /// isFollow Unit
        /// </summary>
        public bool isFollowing;
        [Tooltip("스폰할 이펙트 오브젝트")]
        /// <summary>
        /// Spawn Object
        /// </summary>
        public GameObject Object;

        public GameObject SpawnObject(Vector3 pos)
        {
            if (Object == null)
                return null;

            var offset = new Vector3(EffectOffset.x, EffectOffset.y);
            var size = EffectScale;

            if (isRandomPosRot)
            {
                var randomRotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0, 360f));
                pos = new Vector2(pos.x + UnityEngine.Random.Range(-isRandomRange, isRandomRange), pos.y + UnityEngine.Random.Range(-isRandomRange, isRandomRange));
                return (GameManager.Inst.StageManager.EffectContainer?.CheckObject(ObjectPooling_TYPE.Effect, Object, EffectScale, GameManager.Inst.StageManager.EffectContainer.transform) as EffectPooling).GetObejct(pos, randomRotation, size);
            }
            else
            {
                return (GameManager.Inst.StageManager.EffectContainer?.CheckObject(ObjectPooling_TYPE.Effect, Object, EffectScale, GameManager.Inst.StageManager.EffectContainer.transform) as EffectPooling).GetObejct(pos + offset, Quaternion.Euler(Object.transform.eulerAngles), size);
            }
        }

        public GameObject SpawnObject(Unit unit)
        {
            if (Object == null)
                return null;

            if (unit == null)
                return null;

            var offset = new Vector3(EffectOffset.x * unit.Core.CoreMovement.FancingDirection, EffectOffset.y);
            var size = EffectScale;

            if (isTransformGlobal)
            {
                return unit.Core.CoreEffectManager.StartEffectsPos(Object, Vector2.zero, size, false);
            }

            if (isHeader)
            {
                return unit.Core.CoreEffectManager.StartEffectsPos(Object,
                    (isRandomPosRot ? unit.Core.CoreCollisionSenses.HeaderCenterPos : unit.Core.CoreCollisionSenses.HeaderCenterPos + offset), size, isFollowing);
            }
            else if (isGround)
            {
                return unit.Core.CoreEffectManager.StartEffectsPos(Object,
                    (isRandomPosRot ? unit.Core.CoreCollisionSenses.GroundCenterPos : unit.Core.CoreCollisionSenses.GroundCenterPos + offset), size, isFollowing);
            }
            else
            {
                if (isRandomPosRot)
                {
                    return unit.Core.CoreEffectManager.StartEffectsWithRandomPosRot(
                            Object,
                            isRandomRange, size, isFollowing);
                }
                else
                {
                    return unit.Core.CoreEffectManager.StartEffectsPos(Object, unit.Core.CoreCollisionSenses.UnitCenterPos + offset, size, isFollowing);
                }
            }
        }
    }

    [Serializable]
    public struct HitAction
    {
        public bool Debug;
        /// <summary>
        /// 공격 범위
        /// </summary>
        public Rect ActionRect;

        [field: Min(1)]
        /// <summary>
        /// 다단 히트 횟수
        /// </summary>
        public int RepeatAction;

        /// <summary>
        /// 추가 데미지
        /// </summary>
        [field: Tooltip("추가 데미지")]
        public float AdditionalDamage;
        [field: Tooltip("고정 데미지 | AdditionalDamage로 고정데미지 피해")]
        public bool isFixed;
        /// <summary>
        /// 온힛판정
        /// </summary>
        public bool isOnHit;
        /// <summary>
        /// Knockback Angle
        /// </summary>
        [field: Tooltip("넉백 Angle, 벡터 크기에 따라 넉백 증가")]
        public Vector2 KnockbackAngle;
        /// <summary>
        /// 공격 시 효과
        /// </summary>
        public EffectPrefab[] EffectPrefab;
        /// <summary>
        /// 공격 시 사운드
        /// </summary>
        public AudioPrefab[] audioClips;
        /// <summary>
        /// 공격 시 ShakeCam
        /// </summary>
        public CamData[] camDatas;
    }
}
