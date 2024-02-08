using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee_Enemy_1_MoveState : EnemyRunState
{
    private Melee_Enemy_1 enemy_Melee;
    public Melee_Enemy_1_MoveState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
        this.enemy_Melee = enemy as Melee_Enemy_1;
    }
    public override void Pattern()
    {
        enemy_Melee.EnemyPattern();
    }
    public override void IdleState()
    {
        unit.FSM.ChangeState(enemy_Melee.IdleState);
    }
}
