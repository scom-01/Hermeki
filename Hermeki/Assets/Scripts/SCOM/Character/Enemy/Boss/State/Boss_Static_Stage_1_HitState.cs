using SCOM.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boss_Static_Stage_1_HitState : EnemyHitState
{
    private Boss_Static_Stage_1 boss_Static_Stage_1;
    public Boss_Static_Stage_1_HitState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
        boss_Static_Stage_1 = enemy as Boss_Static_Stage_1;
    }

    public override void IdleState()
    {
        unit.FSM.ChangeState(boss_Static_Stage_1.IdleState);
    }
}