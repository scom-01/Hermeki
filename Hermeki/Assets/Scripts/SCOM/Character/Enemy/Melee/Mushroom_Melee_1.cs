using SCOM.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Mushroom_Melee_1 : Melee_Enemy_1
{
    public override void EnemyPattern()
    {
        if (TargetUnit == null)
            return;

        //인식 범위 내 
        if ((TargetUnit.Core.CoreCollisionSenses.UnitCenterPos - Core.CoreCollisionSenses.UnitCenterPos).magnitude <= enemyData.UnitAttackDistance)
        {
            AttackState.SetWeapon(Inventory.Weapon);
            //일직선 상
            if ((Core.CoreCollisionSenses as EnemyCollisionSenses).isUnitInFrontDetectedArea || (Core.CoreCollisionSenses as EnemyCollisionSenses).isUnitInBackDetectedArea)
            {
                //달려듬
                Core.CoreMovement.FlipToTarget();
                AttackState.SetWeapon(Inventory.Weapon);
                FSM.ChangeState(AttackState);
                return;
            }
        }
        Core.CoreMovement.FlipToTarget();
        FSM.ChangeState(RunState);
    }
}
