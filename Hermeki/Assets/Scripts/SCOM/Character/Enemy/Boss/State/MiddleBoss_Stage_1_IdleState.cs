using System.Collections.Generic;

public class MiddleBoss_Stage_1_IdleState : EnemyIdleState
{
    private MiddleBoss_Stage_1 MiddleBoss_Stage_1;

    private List<bool> Phase = new List<bool>() { false, false, false };
    private List<bool> PhasePowerless = new List<bool>() { false, false};

    public class AnimPattern
    {
        public AnimCommand command = new AnimCommand();
        public bool isSet = false;
        public AnimPattern(AnimCommand _command,bool _isSet)
        {
            command = _command;
            isSet = _isSet;
        }
    }
    private List<AnimPattern> PatternPair_1 = new List<AnimPattern>();
    private List<AnimPattern> PatternPair_2 = new List<AnimPattern>();
    private List<AnimPattern> PatternPair_3 = new List<AnimPattern>();
    public MiddleBoss_Stage_1_IdleState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
        MiddleBoss_Stage_1 = enemy as MiddleBoss_Stage_1;

        MiddleBoss_Stage_1.AttackState.SetWeapon(unit.Inventory.Weapon);
        unit.Inventory.Weapon.weaponGenerator.Init();

        //직선형 돌진
        PatternPair_1.Add(new AnimPattern(unit.Inventory.Weapon.weaponData.weaponCommandDataSO.GroundedCommandList[0].commands[0], false));
        PatternPair_1.Add(new AnimPattern(unit.Inventory.Weapon.weaponData.weaponCommandDataSO.GroundedCommandList[1].commands[0], false));
        PatternPair_1.Add(new AnimPattern(unit.Inventory.Weapon.weaponData.weaponCommandDataSO.GroundedCommandList[0].commands[0], false));
        //직선형 투사체 발사

        //타겟형 돌진
        PatternPair_2.Add(new AnimPattern(unit.Inventory.Weapon.weaponData.weaponCommandDataSO.GroundedCommandList[2].commands[0], false));
        //직선형 투사체 발사
        PatternPair_2.Add(new AnimPattern(unit.Inventory.Weapon.weaponData.weaponCommandDataSO.GroundedCommandList[1].commands[0], false));
        //타겟형 돌진
        PatternPair_2.Add(new AnimPattern(unit.Inventory.Weapon.weaponData.weaponCommandDataSO.GroundedCommandList[2].commands[0], false));
        //5개의 직선형 투사체
        PatternPair_2.Add(new AnimPattern(unit.Inventory.Weapon.weaponData.weaponCommandDataSO.GroundedCommandList[3].commands[0], false));
        //타겟형 돌진
        PatternPair_2.Add(new AnimPattern(unit.Inventory.Weapon.weaponData.weaponCommandDataSO.GroundedCommandList[2].commands[0], false));
        //직선형 투사체 발사
        PatternPair_2.Add(new AnimPattern(unit.Inventory.Weapon.weaponData.weaponCommandDataSO.GroundedCommandList[1].commands[0], false));
        //타겟형 돌진
        PatternPair_2.Add(new AnimPattern(unit.Inventory.Weapon.weaponData.weaponCommandDataSO.GroundedCommandList[2].commands[0], false));
        //5개의 직선형 투사체
        PatternPair_2.Add(new AnimPattern(unit.Inventory.Weapon.weaponData.weaponCommandDataSO.GroundedCommandList[3].commands[0], false));
        //랜덤위치로 텔레포트
        PatternPair_2.Add(new AnimPattern(unit.Inventory.Weapon.weaponData.weaponCommandDataSO.AirCommandList[0].commands[4], false));
        //범위 공격형
        PatternPair_2.Add(new AnimPattern(unit.Inventory.Weapon.weaponData.weaponCommandDataSO.AirCommandList[0].commands[2], false));

        //타겟형 투사체
        PatternPair_3.Add(new AnimPattern(unit.Inventory.Weapon.weaponData.weaponCommandDataSO.GroundedCommandList[1].commands[1], false));
        //타겟형 돌진3회
        PatternPair_3.Add(new AnimPattern(unit.Inventory.Weapon.weaponData.weaponCommandDataSO.GroundedCommandList[2].commands[1], false));        
        //직선형 투사체
        PatternPair_3.Add(new AnimPattern(unit.Inventory.Weapon.weaponData.weaponCommandDataSO.GroundedCommandList[3].commands[0], false));
        //타겟형 투사체
        PatternPair_3.Add(new AnimPattern(unit.Inventory.Weapon.weaponData.weaponCommandDataSO.GroundedCommandList[1].commands[1], false));
        //타겟형 돌진3회
        PatternPair_3.Add(new AnimPattern(unit.Inventory.Weapon.weaponData.weaponCommandDataSO.GroundedCommandList[2].commands[1], false));        
        //직선형 투사체
        PatternPair_3.Add(new AnimPattern(unit.Inventory.Weapon.weaponData.weaponCommandDataSO.GroundedCommandList[3].commands[0], false));
        //랜덤위치로 텔레포트
        PatternPair_3.Add(new AnimPattern(unit.Inventory.Weapon.weaponData.weaponCommandDataSO.AirCommandList[0].commands[4], false));
        //범위 공격형
        PatternPair_3.Add(new AnimPattern(unit.Inventory.Weapon.weaponData.weaponCommandDataSO.AirCommandList[0].commands[2], false));
        PatternPair_3.Add(new AnimPattern(unit.Inventory.Weapon.weaponData.weaponCommandDataSO.GroundedCommandList[1].commands[1], false));        
        //범위 공격형 2
        PatternPair_3.Add(new AnimPattern(unit.Inventory.Weapon.weaponData.weaponCommandDataSO.AirCommandList[0].commands[3], false));
    }

    private void Phase_Pattern(List<AnimPattern> animPatterns)
    {
        if (animPatterns.Count == 0)
            return;

        for (int i = 0; i < animPatterns.Count; i++)
        {
            if (animPatterns[i].isSet)
                continue;

            unit.Inventory.Weapon.weaponGenerator.GenerateWeapon(animPatterns[i].command);
            unit.FSM.ChangeState(MiddleBoss_Stage_1.AttackState);
            animPatterns[i].isSet = true;
            return;
        }

        for (int i = 0; i < animPatterns.Count; i++)
        {
            animPatterns[i].isSet = false;
        }

        Phase_Pattern(animPatterns);
    }

    public override void Pattern()
    {
        MiddleBoss_Stage_1.AttackState.SetWeapon(unit.Inventory.Weapon);
        //현재 체력 50% ~ 100%
        if (unit.Core.CoreUnitStats.CurrentHealth >= unit.Core.CoreUnitStats.CalculStatsData.MaxHealth / 2f)
        {
            if (!Phase[0])
            {
                Phase[0] = true;
                return;
            }

            if ((MiddleBoss_Stage_1.TargetUnit.Core.CoreCollisionSenses.UnitCenterPos - MiddleBoss_Stage_1.Core.CoreCollisionSenses.UnitCenterPos).magnitude <= MiddleBoss_Stage_1.enemyData.UnitDetectedDistance)
            {
                Phase_Pattern(PatternPair_1);
            }
            else
            {
                MiddleBoss_Stage_1.TeleportState.SetWeapon(unit.Inventory.Weapon);
                unit.Inventory.Weapon.weaponGenerator.GenerateWeapon(unit.Inventory.Weapon.weaponData.weaponCommandDataSO.AirCommandList[0].commands[0]);
                unit.FSM.ChangeState(MiddleBoss_Stage_1.TeleportState);
            }
            return;
        }
        //현재 체력 20% ~ 49%
        else if (unit.Core.CoreUnitStats.CurrentHealth >= unit.Core.CoreUnitStats.CalculStatsData.MaxHealth / 5f)
        {            
            //페이즈당 한 번 실행 BloodWave            
            if (!Phase[1])
            {
                //if (GameManager.Inst?.StageManager.GetType() == typeof(BossStageManager))
                //{
                //    //BloodWave
                //    unit.Inventory.Weapon.weaponGenerator.GenerateWeapon(unit.Inventory.Weapon.weaponData.weaponCommandDataSO.AirCommandList[0].commands[1]);
                //    unit.FSM.ChangeState(MiddleBoss_Stage_1.AttackState);
                //    var boss_stage = GameManager.Inst?.StageManager as BossStageManager;
                //}
                Phase[1] = true;
                return;
            }        
            //페이즈당 한 번 실행 BloodWave            
            if (!PhasePowerless[0])
            {
                Powerless();
                PhasePowerless[0] = true;
                return;
            }            
            //인식 범위 내 
            if ((MiddleBoss_Stage_1.TargetUnit.Core.CoreCollisionSenses.UnitCenterPos - MiddleBoss_Stage_1.Core.CoreCollisionSenses.UnitCenterPos).magnitude <= MiddleBoss_Stage_1.enemyData.UnitDetectedDistance)
            {
                Phase_Pattern(PatternPair_2);
            }
            //인식 범위 밖
            else
            {
                //돌진기
                unit.Inventory.Weapon.weaponGenerator.GenerateWeapon(unit.Inventory.Weapon.weaponData.weaponCommandDataSO.GroundedCommandList[2].commands[0]);
                unit.FSM.ChangeState(MiddleBoss_Stage_1.AttackState);
            }
            return;
        }
        //현재 체력 0 ~ 19%
        else
        {
            //페이즈당 한 번 실행 BloodWave            
            if (!Phase[2])
            {
                //if (GameManager.Inst?.StageManager.GetType() == typeof(BossStageManager))
                //{
                //    //BloodWave
                //    unit.Inventory.Weapon.weaponGenerator.GenerateWeapon(unit.Inventory.Weapon.weaponData.weaponCommandDataSO.AirCommandList[0].commands[1]);
                //    unit.FSM.ChangeState(MiddleBoss_Stage_1.AttackState);
                //    var boss_stage = GameManager.Inst?.StageManager as BossStageManager;
                //}
                Phase[2] = true;
                return;
            }
            //페이즈당 한 번 실행 BloodWave            
            if (!PhasePowerless[1])
            {
                Powerless();
                PhasePowerless[1] = true;
                return;
            }
            if ((MiddleBoss_Stage_1.TargetUnit.Core.CoreCollisionSenses.UnitCenterPos - MiddleBoss_Stage_1.Core.CoreCollisionSenses.UnitCenterPos).magnitude <= MiddleBoss_Stage_1.enemyData.UnitDetectedDistance)
            {
                Phase_Pattern(PatternPair_3);
            }
            else
            {
                ////돌진기
                //unit.Inventory.Weapon.weaponGenerator.GenerateWeapon(unit.Inventory.Weapon.weaponData.weaponCommandDataSO.GroundedCommandList[2].commands[0]);
                //unit.FSM.ChangeState(MiddleBoss_Stage_1.AttackState);
            }
            return;
        }
    }

    private void Powerless()
    {
        unit.Inventory.Weapon.weaponGenerator.GenerateWeapon(unit.Inventory.Weapon.weaponData.weaponCommandDataSO.AirCommandList[1].commands[0]);
        unit.FSM.ChangeState(MiddleBoss_Stage_1.AttackState);
    }
    public override void MoveState()
    {
    }
}
