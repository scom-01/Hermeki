using SCOM.Weapons.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct ProjectileData
{

    [Header("- - - Projectile - - -")]
    [Tooltip("발사 위치")]
    /// <summary>
    /// 발사 위치
    /// </summary>
    public Vector3 Pos;
    [Tooltip("발사 각도")]
    /// <summary>
    /// 발사 각도
    /// </summary>
    public Vector3 Rot;
    /// <summary>
    /// Rot로 Speed값만큼 고정이동 여부
    /// </summary>
    public bool isFixedMovement;
    [Tooltip("투사체 속도")]
    /// <summary>
    /// 투사체 속도
    /// </summary>
    public float Speed;
    [Tooltip("투사체 지속시간")]
    /// <summary>
    /// 투사체 지속시간
    /// </summary>
    [Min(0.5f)]
    public float DurationTime;

    [Header("- - - Damage - - -")]
    [Tooltip("추가 데미지")]
    /// <summary>
    /// 추가 데미지
    /// </summary>
    public float AdditionalDmg;
    [Tooltip("고정 데미지(이때 데미지는 AdditionalDmg로 계산)")]
    /// <summary>
    /// 고정 데미지(이때 데미지는 AdditionalDmg로 계산)
    /// </summary>
    public bool isFixed;
    [Tooltip("넉백 각도")]
    /// <summary>
    /// 넉백
    /// </summary>
    public Vector2 KnockbackAngle;
    [Tooltip("OnHit 효과 발동 여부")]
    /// <summary>
    /// 온힛효과 발동여부
    /// </summary>
    public bool isOnHit;
    [Header("- - - Attribute - - -")]    
    public bool isBox;
    [Tooltip("투사체 피격 판정 크기")]
    /// <summary>
    /// 발사체 피격 판정 크기
    /// </summary>
    public float Radius;
    /// <summary>
    /// Collider가 BoxCollider일때 size
    /// </summary>
    public Vector2 BCsize;
    [Tooltip("투사체 Offset")]
    /// <summary>
    /// 피격 판정 CircleCollider의 Offset
    /// </summary>
    public Vector2 Offset;
    [Tooltip("투사체 중력")]
    /// <summary>
    /// RigidBody2D GravityScale
    /// </summary>
    public float GravityScale;
    [Tooltip("단일 피격 여부")]
    /// <summary>
    /// 단일 피격
    /// </summary>
    public bool isSingleShoot;

    [Header("- - - Collision - - -")]
    [Tooltip("Ground 충돌 여부")]
    /// <summary>
    /// Ground와 충돌 판별 여부
    /// </summary>
    public bool isCollisionGround;
    [Tooltip("Platform 충돌 여부")]
    /// <summary>
    /// Platform과 충돌 판별 여부
    /// </summary>
    public bool isCollisionPlatform;
    [Tooltip("Bound 여부")]
    /// <summary>
    /// Bound 여부
    /// </summary>
    public bool isBound;
    [Tooltip("bound 적용할 Materaial (반드시 isBound = true일 때 정상작동)")]
    /// <summary>
    /// bound 적용할 Materaial
    /// </summary>
    public PhysicsMaterial2D UnitCC2DMaterial;

    [Header("- - - Effect - - -")]
    [Tooltip("투사체 로컬 스케일")]
    /// <summary>
    /// 이펙트 로컬 스케일
    /// </summary>
    public Vector3 EffectScale;
    [Tooltip("투사체 임팩트 이펙트 로컬 스켈")]
    /// <summary>
    /// 임팩트 이펙트 로컬 스케일
    /// </summary>
    public Vector3 ImpactScale;
    public float ImpactRamdomPosRange;
    [Tooltip("투사체 임팩트 시 Camera Shake 여부")]
    public bool isShakeCam;
    [Tooltip("투사체 임팩트 시 Camera Shake Data(isShakeCam = true일 때 작동)")]
    /// <summary>
    /// 투사체 임팩트 시 ShakeCam
    /// </summary>
    public CamData camDatas;
    [Header("- - - Type - - -")]
    [Tooltip("투사체 발사 타입(추적 발사, 타겟 방향으로 직선 발사,공격 방향에 타겟이 있으면 타겟방향으로 직선 발사) ")]
    public HomingType homingType;
}

[Serializable]
public enum HomingType
{
    Done = 0,
    /// <summary>
    /// 추적 발사
    /// </summary>
    isHoming,
    /// <summary>
    /// 타겟 방향으로 직선 발사
    /// </summary>
    isToTarget,
    /// <summary>
    /// 공격 방향에 타겟이 있으면 타겟방향으로 직선 발사
    /// </summary>
    isToTarget_Direct,
}

[System.Serializable]
public struct ProjectileActionData
{
    public ProjectileData ProjectileData;
    /// <summary>
    /// 투사체
    /// </summary>
    public GameObject Projectile;
}

namespace SCOM.Weapons.Components
{
    [Serializable]
    public class WeaponProjectileActionData : ActionData
    {
        [field: SerializeField] public ProjectileActionData[] ProjectileActionData;
    }
}
