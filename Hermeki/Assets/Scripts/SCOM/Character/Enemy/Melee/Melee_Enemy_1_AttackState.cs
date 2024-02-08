using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee_Enemy_1_AttackState : EnemyAttackState
{
    private Melee_Enemy_1 enemy_Melee1;

    public Melee_Enemy_1_AttackState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
        enemy_Melee1 = enemy as Melee_Enemy_1;
    }
    public override void IdleState()
    {
        enemy.FSM.ChangeState(enemy_Melee1.IdleState);
    }
}
