using SCOM.CoreSystem;
using System.Diagnostics;
using UnityEngine;

public class PlayerState : UnitState
{
    protected Player player;
    //Input
    protected int xInput;                   //x축 이동 입력값
    protected int yInput;                   //y축 이동 입력값
    protected bool JumpInput;               //점프 입력값
    protected bool JumpInputStop;           //점프 입력 해체 값
    protected bool dashInput;               //Dash 입력값
    protected bool skill1Input;             //Skill1 입력값
    protected bool skill2Input;             //Skill2 입력값
    //Checks
    protected bool isGrounded;              //Grounded 체크
    protected bool isPlatform;              //Platform 체크
    protected bool isTouchingWall;          //벽 체크 
    protected bool isTouchingWallBack;      //후방 벽과 붙어있는지 체크

    public bool input = false;
    public void SetInput(ref bool input) => this.input = input;

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Death.isDead)
            return;
    }

    public PlayerState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
        player = unit as Player;
        this.animBoolName = animBoolName;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        //Check
        isGrounded = CollisionSenses.CheckIfGrounded || CollisionSenses.CheckIfPlatform;
        isPlatform = CollisionSenses.CheckIfPlatform;
        isTouchingWall = CollisionSenses.CheckIfTouchingWall;
        isTouchingWallBack = CollisionSenses.CheckIfTouchingWallBack;

        //Input
        xInput = player.InputHandler.NormInputX;
        yInput = player.InputHandler.NormInputY;
        JumpInput = player.InputHandler.JumpInput;
        JumpInputStop = player.InputHandler.JumpInputStop;
        dashInput = player.InputHandler.DashInput;

        skill1Input = player.InputHandler.PrimarySkillInput;
        skill2Input = player.InputHandler.SecondarySkillInput;

        dashInput = player.InputHandler.DashInput;
    }

    /// <summary>
    /// true => 입력이 있고 상태 전환
    /// </summary>
    /// <returns></returns>
    protected bool CheckActionInput()
    {
        if(player.InputHandler.PrimaryInput)
        {
            player?.SetAnimParam("leftAction", true);
        }
        
        if(player.InputHandler.SecondaryInput)
        {
            player?.SetAnimParam("rightAction", true);
        }
        return false;
        if (player.InputHandler.ActionInputs[(int)CombatInputs.primary])
        {
            player.PrimaryAttackState.SetWeapon(player.Inventory.Weapon);
            if (player.PrimaryAttackState.CheckCommand(isGrounded, ref player.Inventory.Weapon.CommandList))
            {
                player.FSM.ChangeState(player.PrimaryAttackState);
                return true;
            }
        }
        else if (player.InputHandler.ActionInputs[(int)CombatInputs.secondary])
        {
            player.SecondaryAttackState.SetWeapon(player.Inventory.Weapon);
            if (player.SecondaryAttackState.CheckCommand(isGrounded, ref player.Inventory.Weapon.CommandList))
            {
                player.FSM.ChangeState(player.SecondaryAttackState);
                return true;
            }
        }

        if (player.InputHandler.PrimarySkillInput)
        {
            if (
                unit.Inventory.Weapon.PrimarySkillStartTime != 0f &&
                GameManager.Inst.PlayTime < (unit.Inventory.Weapon.PrimarySkillStartTime + unit.Inventory.Weapon.weaponData.weaponCommandDataSO.PrimarySkillData.SkillCoolTime)
                )
                return false;
            player.PrimarySkillState.SetWeapon(player.Inventory.Weapon);

            if (isGrounded)
            {
                if (!player.Inventory.Weapon.weaponGenerator.GenerateWeapon(unit.Inventory.Weapon.weaponData.weaponCommandDataSO.PrimarySkillData.GroundweaponData))
                    return false;
            }
            else
            {
                if (!player.Inventory.Weapon.weaponGenerator.GenerateWeapon(unit.Inventory.Weapon.weaponData.weaponCommandDataSO.PrimarySkillData.AirweaponData))
                    return false;
            }
            unit.Inventory.Weapon.PrimarySkillStartTime = GameManager.Inst.PlayTime;
            //DataManager.Inst.JSON_DataParsing.m_JSON_SceneData.PrimarySkillStartTime = unit.Inventory.Weapon.PrimarySkillStartTime;
            player.FSM.ChangeState(player.PrimarySkillState);
            //GameManager.Inst?.MainUI?.MainPanel?.SkillPanelSystem?.PrimarySkillPanel?.UpdateSkillPanel(
            //    unit.Inventory.Weapon.PrimarySkillStartTime,
            //    unit.Inventory.Weapon.weaponData.weaponCommandDataSO.PrimarySkillData.SkillCoolTime);
            player.Inventory.ItemExeOnSKill(unit);
            return true;
        }
        else if (player.InputHandler.SecondarySkillInput)
        {
            if (
                unit.Inventory.Weapon.SecondarySkillStartTime != 0f &&
                GameManager.Inst.PlayTime < (unit.Inventory.Weapon.SecondarySkillStartTime + unit.Inventory.Weapon.weaponData.weaponCommandDataSO.SecondarySkillData.SkillCoolTime)
                )
                return false;
            player.SecondarySkillState.SetWeapon(player.Inventory.Weapon);
            if (isGrounded)
            {
                if (!player.Inventory.Weapon.weaponGenerator.GenerateWeapon(unit.Inventory.Weapon.weaponData.weaponCommandDataSO.SecondarySkillData.GroundweaponData))
                    return false;
            }
            else
            {
                if (!player.Inventory.Weapon.weaponGenerator.GenerateWeapon(unit.Inventory.Weapon.weaponData.weaponCommandDataSO.SecondarySkillData.AirweaponData))
                    return false;
            }
            unit.Inventory.Weapon.SecondarySkillStartTime = GameManager.Inst.PlayTime;
            //DataManager.Inst.JSON_DataParsing.m_JSON_SceneData.SecondarySkillStartTime = unit.Inventory.Weapon.SecondarySkillStartTime;
            player.FSM.ChangeState(player.SecondarySkillState);
            //GameManager.Inst?.MainUI?.MainPanel?.SkillPanelSystem?.SecondarySkillPanel?.UpdateSkillPanel(
                //unit.Inventory.Weapon.SecondarySkillStartTime,
                //unit.Inventory.Weapon.weaponData.weaponCommandDataSO.SecondarySkillData.SkillCoolTime);
            player.Inventory.ItemExeOnSKill(unit);
            return true;
        }

        return false;
    }
}
