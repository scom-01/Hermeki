using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LR_Enemy_1_AttackState : EnemyAttackState
{
    private LR_Enemy_1 enemy_LR;

    public LR_Enemy_1_AttackState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
        enemy_LR = enemy as LR_Enemy_1;
    }
    public override void IdleState()
    {
        enemy.FSM.ChangeState(enemy_LR.IdleState);
    }
}
