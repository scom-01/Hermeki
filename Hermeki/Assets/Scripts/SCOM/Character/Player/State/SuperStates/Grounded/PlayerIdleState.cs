using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.JumpState.ResetAmountOfJumpsLeft();
        Movement.SetVelocityZero();
        unit.RB.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }

    public override void Exit()
    {
        base.Exit();
        unit.RB.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        Movement.SetVelocityX(0f);
        if (xInput != 0f && !isExitingState)
        {
            player.FSM.ChangeState(player.MoveState);
        }
    }
}
