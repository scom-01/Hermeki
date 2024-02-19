using SCOM.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState
{
    protected Unit unit;

    protected bool isAnimationFinished;
    public bool isExitingState = true;

    protected float startTime;

    protected string animBoolName;
    protected Movement Movement
    {
        get => movement ?? unit.Core.GetCoreComponent(ref movement);
    }
    protected CollisionSenses CollisionSenses
    {
        get => collisionSenses ?? unit.Core.GetCoreComponent(ref collisionSenses);
    }
    protected DamageReceiver DamageReceiver
    {
        get => damageReceiver ?? unit.Core.GetCoreComponent(ref damageReceiver);
    }
    protected KnockBackReceiver KnockBackReceiver
    {
        get => knockBackReceiver ?? unit.Core.GetCoreComponent(ref knockBackReceiver);
    }
    protected UnitStats UnitStats
    {
        get => unitStats ?? unit.Core.GetCoreComponent(ref unitStats);
    }

    protected EffectManager EffectManager
    {
        get => effectManager ?? unit.Core.GetCoreComponent(ref effectManager);
    }
    protected SoundEffect SoundEffect
    {
        get => soundEffect ?? unit.Core.GetCoreComponent(ref soundEffect);
    }
    protected Death Death
    {
        get => death ?? unit.Core.GetCoreComponent(ref death);
    }


    private Movement movement;
    private CollisionSenses collisionSenses;
    private DamageReceiver damageReceiver;
    private KnockBackReceiver knockBackReceiver;
    private UnitStats unitStats;
    private EffectManager effectManager;
    private SoundEffect soundEffect;
    private Death death;
    public UnitState(Unit unit, string animBoolName)
    {
        this.unit = unit;
        this.animBoolName = animBoolName;
    }
    public virtual void Enter()
    {
        startTime = Time.time;
        unit?.SetAnimParam(animBoolName, true);
        Debug.Log(unit.name + " Enter State : " + animBoolName);
        isAnimationFinished = false;
        isExitingState = false;
        DoChecks();
    }
    public virtual void Exit()
    {
        Debug.Log(unit.name + " Exit State : " + animBoolName);
        unit?.SetAnimParam(animBoolName, false);
        isExitingState = true;
    }

    public virtual void LogicUpdate()
    {
    }

    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }
    public virtual void DoChecks() { }

    public virtual void AnimationTrigger() { }

    public virtual void AnimationFinishTrigger()
    {
        isAnimationFinished = true;
        unit.RB.gravityScale = unit.UnitData.UnitGravity;
    }

    public virtual void UseInput(ref bool input) => input = false;
}
