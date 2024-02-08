using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boss_Static_Stage_1_DeathState : EnemyDeathState
{
    private Boss_Static_Stage_1 boss_Static_Stage_1;
    public Boss_Static_Stage_1_DeathState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
        boss_Static_Stage_1 = enemy as Boss_Static_Stage_1;
    }
}
