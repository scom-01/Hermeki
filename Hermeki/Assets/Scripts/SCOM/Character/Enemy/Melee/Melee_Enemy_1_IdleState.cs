using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee_Enemy_1_IdleState : EnemyIdleState
{
    private Melee_Enemy_1 enemy_Melee1;

    public Melee_Enemy_1_IdleState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
        enemy_Melee1 = enemy as Melee_Enemy_1;
    }

    public override void Pattern()
    {
        enemy_Melee1.EnemyPattern();
    }

    public override void MoveState()
    {
        if (Random.Range(0.5f, 1f) >= 0.5f)
        {
            Movement.Flip();
        }
        unit.FSM.ChangeState(enemy_Melee1.RunState);
    }
}
