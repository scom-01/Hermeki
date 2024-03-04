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
        unit.Core.CoreMovement.SetVelocityX(0);
    }

    public override void DoChecks()
    {
        base.DoChecks();

        if (EnemyCollisionSenses.isUnitDetectedCircle)
        {
            grounded_Melee_Enemy.SetTarget(EnemyCollisionSenses.UnitDetectedCircle?.GetComponent<Unit>());
        }
        else
        {
            grounded_Melee_Enemy.SetTarget(null);
        }

    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Death.isDead)
            return;

        //타겟 방향 회전
        unit.Core.CoreMovement.FlipToTarget();

        if (enemy.GetTarget() != null)
        {
            //앞 감지
            if (EnemyCollisionSenses.isUnitInFrontDetectedArea)
            {
                //앞 절벽 혹은 벽
                if (!isCliff || isTouchingWall)
                {
                }
                else
                {
                    grounded_Melee_Enemy.FSM.ChangeState(grounded_Melee_Enemy.MoveState);
                    return;
                }
            }
            //뒤 감지
            else if (EnemyCollisionSenses.isUnitInBackDetectedArea)
            {
                //뒤 절벽 혹은 벽
                if (!isCliffBack || isTouchingWallBack)
                {
                }
                else
                {
                    grounded_Melee_Enemy.FSM.ChangeState(grounded_Melee_Enemy.MoveState);
                    return;
                }
            }
            else
            {
                return;
            }
        }
        else
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

        //(!isCliff && !isCliffBack) : 앞 뒤 모두 절벽
        //(isTouchingWall && isTouchingWallBack) : 앞 뒤 모두 벽
        if ((!isCliff && !isCliffBack) || (isTouchingWall && isTouchingWallBack))
        {
            enemy.SetTarget(null);
            return;
        }
        //(!isCliff || isTouchingWall) : 앞 절벽 혹은 벽
        else if (!isCliff || isTouchingWall)
        {
            Movement.SetVelocityX(0);
            Movement.Flip();
        }
        Movement.SetVelocityX(UnitStats.CalculStatsData.DefaultMoveSpeed * ((100f + UnitStats.CalculStatsData.MovementVEL_Per) / 100f) * Movement.FancingDirection);
    }

    private float filpDelay = 0f;
    private float flipStartTime = 0f;
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Death.isDead)
            return;

        //앞 감지
        if (EnemyCollisionSenses.isUnitInFrontDetectedArea)
        {
            //앞 절벽 혹은 벽
            if (!isCliff || isTouchingWall)
            {
                grounded_Melee_Enemy.FSM.ChangeState(grounded_Melee_Enemy.IdleState);
                return;
            }
            else
            {

            }
        }
        //뒤 감지
        else if (EnemyCollisionSenses.isUnitInBackDetectedArea)
        {
            //뒤 절벽 혹은 벽
            if (!isCliffBack || isTouchingWallBack)
            {
                grounded_Melee_Enemy.FSM.ChangeState(grounded_Melee_Enemy.IdleState);
                return;
            }
            else
            {

            }
        }
        else
        {
            return;
        }

        enemy.SetTarget(EnemyCollisionSenses.UnitDetectedBox?.GetComponent<Unit>());

        if (EnemyCollisionSenses.CheckUnitDistBox(enemy.enemyData.UnitAttackDistance))
        {
            grounded_Melee_Enemy.FSM.ChangeState(grounded_Melee_Enemy.AttackState);
        }
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