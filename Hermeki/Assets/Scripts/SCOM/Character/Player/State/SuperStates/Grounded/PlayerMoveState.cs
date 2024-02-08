using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        Movement.SetVelocityX(0f);
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

        Movement.CheckIfShouldFlip(xInput);

        if (isExitingState) return;

        Movement.SetVelocityX(UnitStats.CalculStatsData.DefaultMoveSpeed * ((100f + UnitStats.CalculStatsData.MovementVEL_Per) / 100f) * xInput);

        if (xInput == 0f && !isExitingState)
        {
            player.FSM.ChangeState(player.IdleState);
        }
    }
}
