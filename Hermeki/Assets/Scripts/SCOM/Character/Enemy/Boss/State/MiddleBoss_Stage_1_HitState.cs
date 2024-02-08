using SCOM.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MiddleBoss_Stage_1_HitState : EnemyHitState
{
    private MiddleBoss_Stage_1 MiddleBoss_Stage_1;
    public MiddleBoss_Stage_1_HitState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
        MiddleBoss_Stage_1 = enemy as MiddleBoss_Stage_1;
    }

    public override void IdleState()
    {
        unit.FSM.ChangeState(MiddleBoss_Stage_1.IdleState);
    }
}