using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flying_Melee_Enemy : Enemy
{
    [HideInInspector]
    public FlyEnemyAI EnemyAI;


    public virtual Flying_Enemy_AttackState AttackState { get; private set; }
    public virtual Flying_Enemy_IdleState IdleState { get; private set; }
    public virtual Flying_Enemy_MoveState MoveState { get; private set; }
    public virtual Flying_Enemy_HitState HitState { get; private set; }
    public virtual Flying_Enemy_DeathState DeathState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        //각 State 생성
        IdleState = new Flying_Enemy_IdleState(this, "idle");
        MoveState = new Flying_Enemy_MoveState(this, "move");
        AttackState = new Flying_Enemy_AttackState(this, "action");
        DeathState = new Flying_Enemy_DeathState(this, "death");
        EnemyAI = this.GetComponent<AIPath>() as FlyEnemyAI;
        if (EnemyAI != null)
        {
            EnemyAI.radius = CC2D.size.x / 2;
            EnemyAI.TargetReachedAction += () => FSM.ChangeState(AttackState);
        }
    }
    protected override void Init()
    {
        base.Init();
        FSM.Initialize(IdleState);
    }

    public override void DieEffect()
    {
        base.DieEffect();
        if (EnemyAI != null) 
            EnemyAI.enabled = false;
        FSM.ChangeState(DeathState);
    }
    public override void SetTarget(Unit unit)
    {
        base.SetTarget(unit);
        if (EnemyAI != null)
            EnemyAI.target = unit?.transform;
    }
}
