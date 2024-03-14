using UnityEngine;

[CreateAssetMenu(fileName = "newUnitData",menuName ="Data/Unit Data")]
public class UnitData : ScriptableObject
{
    [Header("Status")]
    [Tooltip("기본 Status")]
    public StatsData statsStats;

    public string UnitName;
    //public LocalizedString UnitNameLocal;

    [Header("RigidBody2D")]
    public float UnitGravity;

    [Header("InvincibleTime")]
    [Tooltip("피격 쿨타임")]
    public float invincibleTime = 1f;
    [Tooltip("터치 피격 쿨타임")]
    public float touchDamageinvincibleTime = 1f;

    [Header("Collider")]
    [Tooltip("기본 캡슐 콜라이더 Offset")]
    public Vector2 standCC2DOffset;
    [Tooltip("기본 캡슐 콜라이더 Size")]
    public Vector2 standCC2DSize;
    [Tooltip("캡슐 콜라이더 PhysicsMtrl2D")]
    public PhysicsMaterial2D UnitCC2DMaterial;

    [Header("Check Variables")]
    [Tooltip("지면 감지 거리")]
    public float groundCheckDistance = 0.1f;
    [Tooltip("벽면 감지 거리")]
    public float wallCheckDistance = 0.5f;
    [Tooltip("유닛 감지 거리")]
    public float UnitDetectedDistance = 0.5f;

    public Unit_Size unit_size = Unit_Size.Small;

    public LayerMask GroundMask;

    [Header("Animator")]
    public RuntimeAnimatorController UnitAnimator;
}
