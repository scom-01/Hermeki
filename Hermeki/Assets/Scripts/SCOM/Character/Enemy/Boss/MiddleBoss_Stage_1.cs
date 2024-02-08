using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MiddleBoss_Stage_1 : Enemy
{
    #region State Variables
    public MiddleBoss_Stage_1_AttackState AttackState { get; private set; }
    public MiddleBoss_Stage_1_TeleportState TeleportState { get; private set; }
    public MiddleBoss_Stage_1_IdleState IdleState { get; private set; }

    public MiddleBoss_Stage_1_HitState HitState { get; private set; }
    public MiddleBoss_Stage_1_DeathState DeathState { get; private set; }
    #endregion

    #region Unity Callback Func
    protected override void Awake()
    {
        base.Awake();

        AttackState = new MiddleBoss_Stage_1_AttackState(this, "action");
        TeleportState = new MiddleBoss_Stage_1_TeleportState(this, "action");
        IdleState = new MiddleBoss_Stage_1_IdleState(this, "idle");
        //RunState = new Boss_Static_Stage_1_MoveState(this, "run");
        HitState = new MiddleBoss_Stage_1_HitState(this, "hit");
        DeathState = new MiddleBoss_Stage_1_DeathState(this, "death");
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
