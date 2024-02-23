using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LR_Enemy_1_MoveState : EnemyMoveState
{
    private LR_Enemy_1 enemy_LR;
    public LR_Enemy_1_MoveState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
        this.enemy_LR = enemy as LR_Enemy_1;
    }
    public override void Pattern()
    {
        //패턴 딜레이
        if (Time.time >= startTime + Random.Range(enemy.enemyData.minIdleTime, enemy.enemyData.maxIdleTime))
        {
            isDelayCheck = true;
        }

        if(isDelayCheck)
        {
            enemy_LR.EnemyPattern();
            isDelayCheck = false;
        }
    }
    public override void IdleState()
    {
        unit.FSM.ChangeState(enemy_LR.IdleState);
    }
}
