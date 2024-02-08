using SCOM.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Melee_Enemy_1_HitState : EnemyHitState
{
    private Melee_Enemy_1 enemy_Melee1;
    public Melee_Enemy_1_HitState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
        enemy_Melee1 = enemy as Melee_Enemy_1;
    }

    public override void IdleState()
    {
        unit.FSM.ChangeState(enemy_Melee1.IdleState);
    }
}