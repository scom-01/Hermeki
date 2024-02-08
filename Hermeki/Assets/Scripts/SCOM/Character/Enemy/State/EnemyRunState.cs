using SCOM.CoreSystem;
using UnityEngine;

public abstract class EnemyRunState : EnemyState
{  
    public EnemyRunState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        if (!isGrounded)
        {
            IdleState();
            return;
        }

        if ((!isCliff && !isCliffBack) || (isTouchingWall && isTouchingWallBack))
        {
            enemy.SetTarget(null);
            IdleState();
            return;
        }
        else if (!isCliff || isTouchingWall)
        {
            Movement.SetVelocityX(0);
            Movement.Flip();
        }
        Movement.SetVelocityX(UnitStats.CalculStatsData.DefaultMoveSpeed * ((100f + UnitStats.CalculStatsData.MovementVEL_Per) / 100f) * Movement.FancingDirection);
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

        if (unit.UnitData.GetType() != typeof(EnemyData))
            return;
        if (EnemyCollisionSenses.UnitFrontDetectArea || EnemyCollisionSenses.UnitBackDetectArea)
        {
            enemy.SetTarget(EnemyCollisionSenses.UnitFrontDetectArea?.GetComponent<Unit>());
            Pattern();
            return;
        }

        //패턴 딜레이
        if (Time.time >= startTime + enemy.enemyData.maxIdleTime)
        {
            isDelayCheck = true;
        }

        if (!isDelayCheck)
            return;

        isDelayCheck = false;
        IdleState();
    }
    public abstract void IdleState();

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
