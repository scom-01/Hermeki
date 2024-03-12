using UnityEngine;

public class Grounded_Melee_Enemy : Enemy
{
    [HideInInspector]
    public MeleeEnemyAI EnemyAI;


    public virtual Grounded_Melee_Enemy_AttackState AttackState { get; private set; }
    public virtual Grounded_Melee_Enemy_IdleState IdleState { get; private set; }
    public virtual Grounded_Melee_Enemy_MoveState MoveState { get; private set; }
    public virtual Grounded_Melee_Enemy_HitState HitState { get; private set; }
    public virtual Grounded_Melee_Enemy_DeathState DeathState { get; private set; }

    public UnitAnimationEventHandler eventHandler;

    protected override void Awake()
    {
        base.Awake();

        //각 State 생성
        IdleState = new Grounded_Melee_Enemy_IdleState(this, "idle");
        MoveState = new Grounded_Melee_Enemy_MoveState(this, "move");
        AttackState = new Grounded_Melee_Enemy_AttackState(this, "action");
        DeathState = new Grounded_Melee_Enemy_DeathState(this, "death");

        eventHandler = this.GetComponentInChildren<UnitAnimationEventHandler>();
        //EnemyAI = this.GetComponent<AIPath>() as MeleeEnemyAI;
        //EnemyAI.radius = CC2D.size.x / 2;

        //EnemyAI.TargetReachedAction += () => FSM.ChangeState(AttackState);
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
        //EnemyAI.target = unit?.transform;
    }
}
