using SCOM.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LR_Enemy_1 : Enemy
{
    #region State Variables
    public LR_Enemy_1_AttackState AttackState { get; private set; }
    public LR_Enemy_1_IdleState IdleState { get; private set; }
    public LR_Enemy_1_MoveState RunState { get; private set; }
    public LR_Enemy_1_HitState HitState { get; private set; }
    public LR_Enemy_1_DeathState DeathState { get; private set; }
    #endregion

    #region Unity Callback Func
    protected override void Awake()
    {
        base.Awake();

        AttackState = new LR_Enemy_1_AttackState(this, "action");
        IdleState = new LR_Enemy_1_IdleState(this, "idle");
        RunState = new LR_Enemy_1_MoveState(this, "run");
        HitState = new LR_Enemy_1_HitState(this, "hit");
        DeathState = new LR_Enemy_1_DeathState(this, "death");
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
