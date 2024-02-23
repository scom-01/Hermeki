using SCOM.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class Melee_Enemy_1 : Enemy
{
    #region State Variables
    public Melee_Enemy_1_AttackState AttackState { get;  set; }
    public Melee_Enemy_1_IdleState IdleState { get;  set; }
    public Melee_Enemy_1_MoveState MoveState { get;  set; }
    public Melee_Enemy_1_HitState HitState { get;  set; }
    public Melee_Enemy_1_DeathState DeathState { get;  set; }
    #endregion

    #region Unity Callback Func
    protected override void Awake()
    {
        base.Awake();

        AttackState = new Melee_Enemy_1_AttackState(this, "action");
        IdleState = new Melee_Enemy_1_IdleState(this, "idle");
        MoveState = new Melee_Enemy_1_MoveState(this, "run");
        HitState = new Melee_Enemy_1_HitState(this, "hit");
        DeathState = new Melee_Enemy_1_DeathState(this, "death");
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    #endregion

    public override void HitEffect()
    {
        base.HitEffect();
        if (!isCC_immunity)
        {
            FSM.ChangeState(HitState);
        }
    }

    public override void EnemyPattern()
    {
    }
    public override void DieEffect()
    {
        base.DieEffect();
        FSM.ChangeState(DeathState);
    }

    protected override void Init()
    {
        base.Init();
        FSM.Initialize(IdleState);
    }
}
