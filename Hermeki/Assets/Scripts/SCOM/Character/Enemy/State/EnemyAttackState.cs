using SCOM.CoreSystem;
using SCOM.Weapons;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

[Serializable]
public abstract class EnemyAttackState : EnemyState
{
    private Weapon weapon;
    public EnemyAttackState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        this.weapon.OnExit += ExitHandler;

        weapon.InAir = !isGrounded;
        weapon.EnterWeapon();

        startTime = Time.time;
    }

    private void ExitHandler()
    {
        AnimationFinishTrigger();
    }

    public override void Exit()
    {
        base.Exit();
        weapon.EventHandler.AnimationFinishedTrigger();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }
    public abstract void IdleState();

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (isExitingState || isAnimationFinished)
        {
            IdleState();
        }
    }

    public void SetWeapon(Weapon weapon)
    {
        this.weapon = weapon;
        weapon.InitializeWeapon(this, unit.Core);
    }
}
