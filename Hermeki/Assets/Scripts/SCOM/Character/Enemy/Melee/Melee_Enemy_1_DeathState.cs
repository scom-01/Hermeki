using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Melee_Enemy_1_DeathState : EnemyDeathState
{
    private Melee_Enemy_1 enemy_Melee1;
    public Melee_Enemy_1_DeathState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
        enemy_Melee1 = enemy as Melee_Enemy_1;
    }

}
