using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Static_Stage_1_AttackState : EnemyAttackState
{
    private Boss_Static_Stage_1 boss_Static_Stage_1;

    public Boss_Static_Stage_1_AttackState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
        boss_Static_Stage_1 = enemy as Boss_Static_Stage_1;
    }
    public override void Enter()
    {
        base.Enter();
        boss_Static_Stage_1.Core.CoreMovement.FlipToTarget();
    }
    public override void IdleState()
    {
        enemy.FSM.ChangeState(boss_Static_Stage_1.IdleState);
    }
}
