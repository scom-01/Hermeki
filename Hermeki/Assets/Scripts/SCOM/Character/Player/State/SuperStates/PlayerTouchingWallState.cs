using SCOM.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchingWallState : PlayerState
{
    protected bool jumpInput;

    public PlayerTouchingWallState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
    }
    

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

    public override void DoChecks()
    {
        base.DoChecks();

        xInput = player.InputHandler.NormInputX;
        yInput = player.InputHandler.NormInputY;
        jumpInput = player.InputHandler.JumpInput;
        isGrounded = CollisionSenses.CheckIfGrounded || CollisionSenses.CheckIfPlatform ;
        isTouchingWall = CollisionSenses.CheckIfTouchingWall;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (jumpInput)
        {
            player.WallJumpState.DetermineWallJumpDirection(isTouchingWall);
            player.FSM.ChangeState(player.WallJumpState);
        }
        else if (isGrounded)
        {
            player.FSM.ChangeState(player.IdleState);
        }
        else if (!isTouchingWall || (xInput != Movement.FancingDirection))
        {
            player.FSM.ChangeState(player.InAirState);
        }
    }
}
