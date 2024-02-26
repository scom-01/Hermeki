using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Flying_Enemy_IdleState : EnemyState
{
    Flying_Melee_Enemy FlyingEnemy;
    public Flying_Enemy_IdleState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
        FlyingEnemy = unit as Flying_Melee_Enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Death.isDead)
            return;

        if (EnemyCollisionSenses.isUnitDetectedCircle)
        {
            FlyingEnemy.SetTarget(EnemyCollisionSenses.UnitDetectedCircle?.GetComponent<Unit>());
        }
        else
        {
            FlyingEnemy.SetTarget(null);
        }    

        //타겟 방향 회전
        unit.Core.CoreMovement.FlipToTarget();

        if (enemy.GetTarget() != null)
        {
            FlyingEnemy.FSM.ChangeState(FlyingEnemy.MoveState);
            return;
        }
    }
}

public class Flying_Enemy_AttackState : EnemyState
{
    Flying_Melee_Enemy FlyingEnemy;
    public Flying_Enemy_AttackState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
        FlyingEnemy = unit as Flying_Melee_Enemy;
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if(isAnimationFinished)
        {
            FlyingEnemy.FSM.ChangeState(FlyingEnemy.IdleState);
        }
    }
}
public class Flying_Enemy_MoveState : EnemyState
{
    Flying_Melee_Enemy FlyingEnemy;
    public Flying_Enemy_MoveState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
        FlyingEnemy = unit as Flying_Melee_Enemy;
    }

    public override void Enter()
    {
        base.Enter();
        FlyingEnemy.EnemyAI.canMove = true;
    }
    public override void Exit()
    {
        base.Exit();
        FlyingEnemy.EnemyAI.canMove = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Death.isDead)
            return;
        //타겟 방향 회전
        unit.Core.CoreMovement.FlipToTarget();

        if (!EnemyCollisionSenses.CheckUnitDetectedCircle(unit.GetTarget()))
        {
            FlyingEnemy.SetTarget(null);
            FlyingEnemy.FSM.ChangeState(FlyingEnemy.IdleState);
            return;
        }
    }
}

public class Flying_Enemy_HitState : EnemyHitState
{
    public Flying_Enemy_HitState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
    }

    public override void IdleState()
    {
        
    }
}

public class Flying_Enemy_DeathState : EnemyDeathState
{
    public Flying_Enemy_DeathState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        unit.RB.gravityScale = 5;
        unit.Core.CoreMovement.SetVelocityZero();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }
}