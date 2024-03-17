using SCOM.Weapons.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    private bool oldIsTouchingWall;     //Old 전방 벽과 붙어있는지 체크
    private bool oldIsTouchingWallBack; //Old 후방 벽과 붙어있는지 체크

    private bool coyoteTime;            //코요테 타임 체크
    private bool wallJumpCoyoteTime;    //벽 점프 코요테 타임 체크
    private bool isJumping;             //점프 중인지 체크

    private float startWallJumpCoyoteTime;

    public PlayerInAirState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        //oldIsTouchingWall = isTouchingWall;
        //oldIsTouchingWallBack = isTouchingWallBack;

        isTouchingWall = CollisionSenses.CheckIfTouchingWall;
        isTouchingWallBack = CollisionSenses.CheckIfTouchingWallBack;


        if (!wallJumpCoyoteTime && !isTouchingWall && !isTouchingWallBack && (oldIsTouchingWall || oldIsTouchingWallBack))
        {
            StartWallJumpCoyoteTime();
        }

        CheckCoyoteTime();
        //CheckWallJumpCoyoteTime();

        CheckJumpMultiplier();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        player?.SetAnimParam("yVelocity", 0);
        player?.SetAnimParam("xVelocity", 0);

        //oldIsTouchingWall = false;
        //oldIsTouchingWallBack = false;
        isTouchingWall = false;
        isTouchingWallBack = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();


        if (CheckActionInput())
            return;

        //Platform 착지
        else if ((CollisionSenses.CheckIfPlatform) && Movement.CurrentVelocity.y <= Mathf.Abs(0.01f))
        {
            player.FSM.ChangeState(player.LandState);
            return;
        }
        //Ground 착지
        else if ((isGrounded) && Movement.CurrentVelocity.y <= Mathf.Abs(0.01f))
        {
            player.FSM.ChangeState(player.LandState);
            return;
        }
        //else if (JumpInput && (isTouchingWall || isTouchingWallBack || wallJumpCoyoteTime))
        //{
        //    StopWallJumpCoyoteTime();
        //    unit.isFixedMovement = false;
        //    isTouchingWall = CollisionSenses.CheckIfTouchingWall;
        //    player.WallJumpState.DetermineWallJumpDirection(isTouchingWall);
        //    player.FSM.ChangeState(player.WallJumpState);
        //    return;
        //}
        else if (coyoteTime && JumpInput && player.JumpState.CanJump() && !player.CC2D.isTrigger)
        {            
            coyoteTime = false;
            player.FSM.ChangeState(player.JumpState);
            return;
        }
        //else if (isTouchingWall && xInput == Movement.FancingDirection && Movement.CurrentVelocity.y <= 0f)
        //{
        //    player.FSM.ChangeState(player.WallSlideState);
        //    return;
        //}
        //else if (dashInput && player.DashState.CheckIfCanDash())
        //{
        //    player?.SetAnimParam("JumpFlip", false);
        //    player.FSM.ChangeState(player.DashState);
        //    return;
        //}

        Movement.CheckIfShouldFlip(xInput);
        //if(Movement.CanSetVelocity)
        if (!unit.isFixedMovement)
            Movement.SetVelocityX(UnitStats.CalculStatsData.DefaultMoveSpeed * ((100f + UnitStats.CalculStatsData.MovementVEL_Per) / 100f) * xInput);

        player?.SetAnimParam("yVelocity", Mathf.Clamp(Movement.CurrentVelocity.y, -3, 13));
        player?.SetAnimParam("xVelocity", Mathf.Abs(Movement.CurrentVelocity.x));
    }


    private void CheckJumpMultiplier()
    {
        if (isJumping)
        {
            if (JumpInputStop)
            {
                //TODO:InputTime으로 점프 Velocity 조절 
                //Movement.SetVelocityY(Movement.CurrentVelocity.y * player.playerData.variableJumpHeightMultiplier);
                isJumping = false;
            }
            else if (Movement.CurrentVelocity.y <= 0f)
            {
                isJumping = false;
            }
        }
    }

    private void CheckCoyoteTime()
    {
        if (coyoteTime && Time.time > startTime + player.playerData.coyeteTime)
        {
            coyoteTime = false;            
            //player.JumpState.DecreaseAmountOfJumpsLeft();
        }
    }

    private void CheckWallJumpCoyoteTime()
    {
        if (wallJumpCoyoteTime && Time.time > startWallJumpCoyoteTime + player.playerData.coyeteTime)
        {
            wallJumpCoyoteTime = false;
        }
    }
    public void StartCoyoteTime() => coyoteTime = true;

    public void StartWallJumpCoyoteTime()
    {
        wallJumpCoyoteTime = true;
        startWallJumpCoyoteTime = Time.time;
    }
    public void StopWallJumpCoyoteTime() => wallJumpCoyoteTime = false;
    public void SetIsJumping() => isJumping = true;
}
