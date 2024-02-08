using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LR_Enemy_1_IdleState : EnemyIdleState
{
    private LR_Enemy_1 enemy_LR;

    public LR_Enemy_1_IdleState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
        enemy_LR = enemy as LR_Enemy_1;
    }

    public override void Pattern()
    {
        enemy_LR.EnemyPattern();
    }

    public override void MoveState()
    {
        if (Random.Range(0.5f, 1f) >= 0.5f)
        {
            Movement.Flip();
        }
        unit.FSM.ChangeState(enemy_LR.RunState);
    }
}
