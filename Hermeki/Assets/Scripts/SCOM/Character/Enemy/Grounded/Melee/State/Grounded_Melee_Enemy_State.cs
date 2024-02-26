using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Grounded_Melee_Enemy_IdleState : EnemyState
{
    Grounded_Melee_Enemy grounded_Melee_Enemy;
    public Grounded_Melee_Enemy_IdleState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
        grounded_Melee_Enemy = unit as Grounded_Melee_Enemy;
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
            grounded_Melee_Enemy.SetTarget(EnemyCollisionSenses.UnitDetectedCircle?.GetComponent<Unit>());
        }
        else
        {
            grounded_Melee_Enemy.SetTarget(null);
        }

        //타겟 방향 회전
        unit.Core.CoreMovement.FlipToTarget();

        if (enemy.GetTarget() != null)
        {
            grounded_Melee_Enemy.FSM.ChangeState(grounded_Melee_Enemy.MoveState);
            return;
        }
    }
}

public class Grounded_Melee_Enemy_AttackState : EnemyState
{
    Grounded_Melee_Enemy grounded_Melee_Enemy;
    public Grounded_Melee_Enemy_AttackState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
        grounded_Melee_Enemy = unit as Grounded_Melee_Enemy;
    }
    public override void Enter()
    {
        base.Enter();
        Movement.SetVelocityX(0);
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (isAnimationFinished)
        {
            grounded_Melee_Enemy.FSM.ChangeState(grounded_Melee_Enemy.IdleState);
        }
    }
}
public class Grounded_Melee_Enemy_MoveState : EnemyState
{
    Grounded_Melee_Enemy grounded_Melee_Enemy;
    public Grounded_Melee_Enemy_MoveState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
        grounded_Melee_Enemy = unit as Grounded_Melee_Enemy;
    }

    public override void Enter()
    {
        base.Enter();
        //grounded_Melee_Enemy.EnemyAI.canMove = true;
    }
    public override void Exit()
    {
        base.Exit();
        //grounded_Melee_Enemy.EnemyAI.canMove = false;
    }
    public override void DoChecks()
    {
        base.DoChecks();
        if (!isGrounded)
        {
            return;
        }

        if ((!isCliff && !isCliffBack) || (isTouchingWall && isTouchingWallBack))
        {
            enemy.SetTarget(null);
            return;
        }
        else if (!isCliff || isTouchingWall)
        {
            Movement.SetVelocityX(0);
            Movement.Flip();
        }
        Movement.SetVelocityX(UnitStats.CalculStatsData.DefaultMoveSpeed * ((100f + UnitStats.CalculStatsData.MovementVEL_Per) / 100f) * Movement.FancingDirection);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Death.isDead)
            return;
        
        if (EnemyCollisionSenses.isUnitDetectedBox)
        {
            //타겟 방향 회전
            unit.Core.CoreMovement.FlipToTarget();
            enemy.SetTarget(EnemyCollisionSenses.UnitDetectedBox?.GetComponent<Unit>());

            if(EnemyCollisionSenses.CheckUnitDistBox(enemy.enemyData.UnitAttackDistance))
            {
                grounded_Melee_Enemy.FSM.ChangeState(grounded_Melee_Enemy.AttackState);
            }
            return;
        }

        //if (!EnemyCollisionSenses.CheckUnitDetectedCircle(unit.GetTarget()))
        //{
        //    grounded_Melee_Enemy.SetTarget(null);
        //    grounded_Melee_Enemy.FSM.ChangeState(grounded_Melee_Enemy.IdleState);
        //    return;
        //}        
    }
}

public class Grounded_Melee_Enemy_HitState : EnemyHitState
{
    public Grounded_Melee_Enemy_HitState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
    }

    public override void IdleState()
    {

    }
}

public class Grounded_Melee_Enemy_DeathState : EnemyDeathState
{
    public Grounded_Melee_Enemy_DeathState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        unit.RB.gravityScale = 5;
        unit.Core.CoreMovement.SetVelocityX(0);
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        unit.Core.CoreMovement.SetVelocityX(0);
    }
}