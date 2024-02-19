using System.Collections.Generic;
using SCOM.CoreSystem;
using SCOM.Weapons;
using UnityEngine;

public class PlayerWeaponState : PlayerAbilityState
{
    public bool CanAttack { get; private set; }

    public Weapon weapon;

    private bool isPrimary;

    public PlayerWeaponState(Unit unit, string animBoolName, bool primary) : base(unit, animBoolName)
    {
        isPrimary = primary;
    }

    public override void Enter()
    {
        base.Enter();
        this.weapon.OnExit += ExitHandler;
        int idx = 1;
        if (isPrimary)
            idx = 0;
        player.InputHandler.UseInput(ref player.InputHandler.ActionInputs[idx]);
        //setVelocity = false;

        if (isPrimary)
        {
            weapon.Command = CommandEnum.Primary;
        }
        else
        {
            weapon.Command = CommandEnum.Secondary;
        }
        weapon.InAir = !isGrounded;
        weapon.EnterWeapon();
        CanAttack = false;
        startTime = Time.time;
    }

    private void ExitHandler()
    {
        AnimationFinishTrigger();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        //아래로 점프
        if (JumpInput && isPlatform && yInput < 0)
        {
            player.InputHandler.JumpInput = false;
            player.StartCoroutine(player.DisableCollision());
            return;
        }
        else if (JumpInput && player.JumpState.CanJump() && CollisionSenses.CheckIfPlatform && yInput < 0)
        {
            player.StartCoroutine(player.DisableCollision());
            return;
        }
        else if (JumpInput && player.JumpState.CanJump() && !player.CC2D.isTrigger)
        {
            Movement.SetVelocityZero();
            weapon.EventHandler.AnimationFinishedTrigger();
            player.FSM.ChangeState(player.JumpState);
            return;
        }

        //공중에서 공격 후 착지상태
        if ( weapon.InAir && isGrounded)
        {
            weapon.EventHandler.AnimationFinishedTrigger();
            player.FSM.ChangeState(player.LandState);
            return;
        }

        //shouldCheckFlip = weapon.weaponData.GetData<MovementData>().ActionData[weapon.CurrentActionCounter].CanFlip;

        if (Movement.CanFlip)
        {
            Movement.CheckIfShouldFlip(xInput);
        }

        //setVelocity = weapon.weaponData.GetData<MovementData>().ActionData[weapon.CurrentActionCounter].CanMoveCtrl;
        if (Movement.CanMovement)
        {
            Movement.SetVelocityX(UnitStats.CalculStatsData.DefaultMoveSpeed * ((100f + UnitStats.CalculStatsData.MovementVEL_Per) / 100f) * xInput);
        }


        //if (player.InputHandler.DashInput && player.DashState.CheckIfCanDash())
        //{
        //    weapon.EventHandler.AnimationFinishedTrigger();
        //    player.FSM.ChangeState(player.DashState);
        //    return;
        //}
    }

    public void SetWeapon(Weapon weapon)
    {
        this.weapon = weapon;
        weapon.InitializeWeapon(this, player.Core);
    }

    public bool CheckCommand(bool isGround, ref List<CommandEnum> q)
    {
        CommandEnum command = CommandEnum.Secondary;
        if (isPrimary)
            command = CommandEnum.Primary;
        q.Add(command);
        bool isCommandLock = false;
        if (!isGround)
        {
            if (CalCommand(weapon.weaponData.weaponCommandDataSO.AirCommandList, q, ref isCommandLock))
            {
                return true;
            }
            else
            {
                if (isCommandLock)
                {
                    for (int i = 0; i < player.InputHandler.ActionInputDelayCheck.Length; i++)
                    {
                        player.InputHandler.ActionInputDelayCheck[i] = false;
                        player.InputHandler.ActionInputs[i] = false;
                    }
                }
            }
        }
        else
        {
            if(CalCommand(weapon.weaponData.weaponCommandDataSO.GroundedCommandList, q))
            {
                return true;
            }
        }
        weapon.ChangeActionCounter(0);
        return false;
    }

    protected bool CalCommand(List<CommandList> commandLists, List<CommandEnum> q, ref bool isCommandLock)
    {
        bool isCountOver = false;
        for (int i = 0; i < commandLists.Count; i++)
        {
            bool pass = true;
            for (int j = 0; j < q.Count; j++)
            {
                if (commandLists[i].commands.Count < j + 1)
                {
                    isCountOver = true;
                    pass = false;
                    break;
                }
                else if (commandLists[i].commands[j].command != q[j])
                {
                    isCountOver = false;
                    pass = false;
                    break;
                }
                if (commandLists[i].commands[j].animOC == null)
                {
                    weapon.oc = weapon.weaponData.weaponCommandDataSO.DefaultAnimator;
                    weapon.weaponGenerator.GenerateWeapon(commandLists[i].commands[j].data);
                }
                else
                {
                    weapon.weaponGenerator.GenerateWeapon(commandLists[i].commands[j]);
                }
            }
            if (pass)
            {
                return true;
            }
            else
            {
                continue;
            }
        }
        if (isCountOver)
        {
            isCommandLock = true;
            return false;
        }
        return false;
    }
    protected bool CalCommand(List<CommandList> commandLists, List<CommandEnum> q)
    {
        for (int i = 0; i < commandLists.Count; i++)
        {
            bool pass = true;
            for (int j = 0; j < q.Count; j++)
            {
                if (commandLists[i].commands.Count < j + 1)
                {
                    pass = false;
                    break;
                }
                else if(commandLists[i].commands[j].command != q[j])
                {
                    pass = false;
                    break;
                }
                if (commandLists[i].commands[j].animOC == null)
                {
                    weapon.oc = weapon.weaponData.weaponCommandDataSO.DefaultAnimator;
                    weapon.weaponGenerator.GenerateWeapon(commandLists[i].commands[j].data);
                }
                else
                {
                    weapon.weaponGenerator.GenerateWeapon(commandLists[i].commands[j]);
                }
            }            
            if (pass)
            {
                return true;
            }
            else
            {
                continue;
            }
        }
        return false;
    }
}
