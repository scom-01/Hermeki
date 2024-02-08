using SCOM.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LR_Enemy_1_HitState : EnemyHitState
{
    private LR_Enemy_1 enemy_LR;
    public LR_Enemy_1_HitState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
        enemy_LR = enemy as LR_Enemy_1;
    }

    public override void IdleState()
    {
        unit.FSM.ChangeState(enemy_LR.IdleState);
    }
}