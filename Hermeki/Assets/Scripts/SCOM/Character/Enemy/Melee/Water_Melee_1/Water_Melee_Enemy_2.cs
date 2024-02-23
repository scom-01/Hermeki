public class Water_Melee_Enemy_2 : Melee_Enemy_1
{
    public override void EnemyPattern()
    {
        if (GetTarget() == null)
            return;

        //인식 범위 내 
        if ((GetTarget().Core.CoreCollisionSenses.UnitCenterPos - Core.CoreCollisionSenses.UnitCenterPos).magnitude <= enemyData.UnitAttackDistance)
        {
            if (Pattern_Idx.Count == 0)
                return;
            int patternCount = Pattern_Idx.Count;
            for (int i = 0; i < patternCount; i++)
            {
                //패턴 스킵 여부
                if (Pattern_Idx[i].Used)
                    continue;

                if (Pattern_Idx[i].Boundary != 0 && (Core.CoreUnitStats.CurrentHealth / Core.CoreUnitStats.CalculStatsData.MaxHealth) < Pattern_Idx[i].Boundary)
                {
                    Pattern_Idx[i].Used = true;
                    continue;
                }
                switch (Pattern_Idx[i].DetectedType)
                {
                    case ENEMY_DetectedType.Box:
                        //일직선 상
                        if ((Core.CoreCollisionSenses as EnemyCollisionSenses).UnitFrontDetectArea || (Core.CoreCollisionSenses as EnemyCollisionSenses).UnitBackDetectArea)
                        {
                            if (Inventory.Weapon.weaponData.weaponCommandDataSO.GroundedCommandList.Count > i)
                            {
                                if ((GetTarget().Core.CoreCollisionSenses.UnitCenterPos - Core.CoreCollisionSenses.UnitCenterPos).magnitude > Pattern_Idx[i].Detected_Distance)
                                    continue;

                                if (Inventory.Weapon.weaponData.weaponCommandDataSO.GroundedCommandList[i].commands[0] == null)
                                    break;
                                AttackState.SetWeapon(Inventory.Weapon);
                                Core.CoreMovement.FlipToTarget();
                                Inventory.Weapon.weaponGenerator.GenerateWeapon(Inventory.Weapon.weaponData.weaponCommandDataSO.GroundedCommandList[i].commands[0]);
                                FSM.ChangeState(AttackState);
                                return;
                            }
                        }
                        break;
                    case ENEMY_DetectedType.Circle:
                        if ((Core.CoreCollisionSenses as EnemyCollisionSenses).isUnitDetectedCircle)
                        {
                            if (Inventory.Weapon.weaponData.weaponCommandDataSO.GroundedCommandList.Count > i)
                            {
                                if ((GetTarget().Core.CoreCollisionSenses.UnitCenterPos - Core.CoreCollisionSenses.UnitCenterPos).magnitude > Pattern_Idx[i].Detected_Distance)
                                    continue;

                                if (Inventory.Weapon.weaponData.weaponCommandDataSO.GroundedCommandList[i].commands[0] == null)
                                    break;
                                AttackState.SetWeapon(Inventory.Weapon);
                                Core.CoreMovement.FlipToTarget();
                                Inventory.Weapon.weaponGenerator.GenerateWeapon(Inventory.Weapon.weaponData.weaponCommandDataSO.GroundedCommandList[i].commands[0]);
                                FSM.ChangeState(AttackState);
                                return;
                            }
                        }
                        break;
                }
            }
        }

        //RunState.FlipToTarget();
        if ((!(Core.CoreCollisionSenses as EnemyCollisionSenses).CheckIfCliff && !(Core.CoreCollisionSenses as EnemyCollisionSenses).CheckIfCliffBack) || ((Core.CoreCollisionSenses as EnemyCollisionSenses).CheckIfTouchingWall && (Core.CoreCollisionSenses as EnemyCollisionSenses).CheckIfTouchingWallBack))
        {
            SetTarget(null);
            return;
        }
        else if (!(Core.CoreCollisionSenses as EnemyCollisionSenses).CheckIfCliff || (Core.CoreCollisionSenses as EnemyCollisionSenses).CheckIfTouchingWall)
        {
            Core.CoreMovement.SetVelocityX(0);
            Core.CoreMovement.Flip();
        }

        if (FSM.CurrentState != MoveState)
            FSM.ChangeState(MoveState);
    }
}
