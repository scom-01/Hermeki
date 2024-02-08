using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData",menuName ="Data/Player Data/Base Data")]
public class PlayerData : UnitData
{
    [Header("Jump State")]
    //[Tooltip("점프 Velocity")]
    //public float jumpVelocity = 15f;
    [Tooltip("점프 가능 횟수")]
    public int amountOfJumps = 1;

    [Header("Collider")]
    [Tooltip("대쉬 콜라이더 크기")]
    public float dashColliderHeight;

    [Header("Wall Jump State")]
    [Tooltip("벽 점프 Velocity")]
    public float wallJumpVelocity = 20f;
    [Tooltip("벽 점프 시간")]
    public float wallJumpTime = 0.4f;
    [Tooltip("벽 점프 시 각도 y = 2x")]
    public Vector2 wallJumpAngle = new Vector2(1, 2);

    [Header("In Air State")]
    [Tooltip("공중 점프 가능 시간 (점프하지않고 낙하 시 0.2s 안에 점프 가능)")]
    public float coyeteTime = 0.2f;
    [Tooltip("멀티 점프 가능 시간")]
    public float variableJumpHeightMultiplier = 0.5f;

    [Header("Wall Slide State")]
    [Tooltip("벽 슬라이딩 Velocity")]
    public float wallSlideVelocity = 3f;

    [Header("Dash State")]
    [Tooltip("대쉬 쿨타임")]
    public float dashCooldown = 0.5f;
    [Tooltip("대쉬 초기화 쿨타임")]
    public float dashResetCooldown = 0.8f;
    [Tooltip("대쉬 지속시간")]
    public float dashDuration = 0.2f;
    [Tooltip("대쉬후 체공시간")]
    public float dashFlightDuration = 0.2f;
    [Tooltip("대쉬 속도")]
    public float dashVelocity = 30f;
    [Tooltip("대쉬 거리")]
    public float dashDistance = 30f;
    [Tooltip("대쉬 사용가능 횟수")]
    public int dashCount = 1;
    public float drag = 10f;
    public float dashEndYMultiplier = 0.2f;
    [Tooltip("잔상 간의 거리")]
    public float distBetweenAfterImages = 0.5f;

    //WeaponData로 옮길 예정
    [Header("Block State")]
    public float blockTime = 1f;
    public float blockCooldown = 2f;
}
