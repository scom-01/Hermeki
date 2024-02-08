using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleBoss_Stage_1_TeleportState : EnemyAttackState
{
    private MiddleBoss_Stage_1 MiddleBoss_Stage_1;

    public MiddleBoss_Stage_1_TeleportState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
        MiddleBoss_Stage_1 = enemy as MiddleBoss_Stage_1;
    }
    public override void Enter()
    {
        base.Enter();
        MiddleBoss_Stage_1.isCC_immunity = true;
    }
    public override void Exit()
    {
        base.Exit();
        MiddleBoss_Stage_1.isCC_immunity = false;
    }
    public override void IdleState()
    {
        enemy.FSM.ChangeState(MiddleBoss_Stage_1.IdleState);
    }
}
