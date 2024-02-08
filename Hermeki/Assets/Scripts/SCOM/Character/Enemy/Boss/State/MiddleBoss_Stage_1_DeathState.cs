using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MiddleBoss_Stage_1_DeathState : EnemyDeathState
{
    private MiddleBoss_Stage_1 MiddleBoss_Stage_1;
    public MiddleBoss_Stage_1_DeathState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
        MiddleBoss_Stage_1 = enemy as MiddleBoss_Stage_1;
    }
}
