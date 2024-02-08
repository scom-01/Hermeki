using SCOM.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyHitState : EnemyState
{
    public EnemyHitState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //enemy.Anim.speed = 0.0f;
        startTime = Time.time;
        Movement.SetVelocityX(0);
        unit.Core.CoreMovement.FlipToTarget();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }
    public abstract void IdleState();

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (isExitingState || isAnimationFinished)
        {
            IdleState();
        }
    }
}
