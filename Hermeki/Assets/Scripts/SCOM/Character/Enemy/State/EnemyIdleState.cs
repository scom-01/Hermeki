using SCOM.CoreSystem;
using UnityEngine;

public abstract class EnemyIdleState : EnemyState
{
    protected float idleTime;
    protected bool isIdleTimeOver;

    public EnemyIdleState(Unit unit, string animBoolName) : base(unit, animBoolName)
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
        isIdleTimeOver = false;
        SetRandomIdleTime();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (EnemyCollisionSenses.isUnitDetectedCircle)// EnemyCollisionSenses.isUnitInFrontDetectedArea || EnemyCollisionSenses.isUnitInBackDetectedArea)
        {
            enemy.SetTarget(EnemyCollisionSenses.UnitDetectedCircle?.GetComponent<Unit>());
        }


        //패턴 딜레이
        if (Time.time >= startTime + Random.Range(enemy.enemyData.minIdleTime, enemy.enemyData.maxIdleTime))
        {
            isDelayCheck = true;
        }

        //타겟 방향 회전
        unit.Core.CoreMovement.FlipToTarget();


        if (!isDelayCheck)
            return;

        isDelayCheck = false;

        if (enemy.TargetUnit == null)
        {
            MoveState();
            return;
        }

        Pattern();
        return;
    }

    public abstract void MoveState();

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void SetRandomIdleTime()
    {
        idleTime = Random.Range(enemy.enemyData.minIdleTime, enemy.enemyData.maxIdleTime);
    }
}
