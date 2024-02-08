using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleBoss_Stage_1_AttackState : EnemyAttackState
{
    private MiddleBoss_Stage_1 MiddleBoss_Stage_1_Stage_1;

    public MiddleBoss_Stage_1_AttackState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
        MiddleBoss_Stage_1_Stage_1 = enemy as MiddleBoss_Stage_1;
    }
    public override void Enter()
    {
        base.Enter();
        unit.Core.CoreMovement.FlipToTarget();
    }
    public override void IdleState()
    {
        enemy.FSM.ChangeState(MiddleBoss_Stage_1_Stage_1.IdleState);
    }
}
