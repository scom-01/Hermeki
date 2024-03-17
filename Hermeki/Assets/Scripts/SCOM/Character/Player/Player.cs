using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Player : Unit
{
    #region State Variables

    //PlayerState
    public PlayerInAirState InAirState { get; private set; }

    //PlayerGroundedState
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerLandState LandState { get; private set; }

    //PlayerAbilityState
    public PlayerJumpState JumpState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    //public PlayerDashState DashState { get; private set; }
    public PlayerDeathState DeathState { get; private set; }

    public PlayerWeaponState PrimaryAttackState { get; private set; }
    public PlayerWeaponState SecondaryAttackState { get; private set; }
    public PlayerWeaponState PrimarySkillState { get; private set; }
    public PlayerWeaponState SecondarySkillState { get; private set; }

    #endregion

    #region Components
        
    public PlayerInputHandler InputHandler;// { get; private set; }
    [HideInInspector]
    public PlayerData playerData;
    public GameObject Controller;
    #endregion

    #region Unity Callback Func
    protected override void Awake()
    {
        base.Awake();
        playerData = UnitData as PlayerData;

        //각 State 생성
        IdleState = new PlayerIdleState(this, "idle");
        MoveState = new PlayerMoveState(this, "move");
        JumpState = new PlayerJumpState(this, "inAir");    //점프하는 순간 공중상태이므로
        InAirState = new PlayerInAirState(this, "inAir");
        LandState = new PlayerLandState(this, "idle");
        DeathState = new PlayerDeathState(this, "death");       
        //Inventory?.Weapon.SetCore(Core);
    }

    protected void Init()
    {
        FSM.Initialize(IdleState);
    }

    protected override void Start()
    {
        base.Start();
        //if (PV != null && PV.IsMine)
        //{
        //    InputHandler.gameObject.SetActive(true);
        //    //InputHandler = Instantiate(Controller, this.transform).GetComponent<PlayerInputHandler>();
        //}
        //else if(!isMulti)
        {
            InputHandler.gameObject.SetActive(true);
            //InputHandler = Instantiate(Controller, this.transform).GetComponent<PlayerInputHandler>();
        }
        Init();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        //fsm.CurrentState.PhysicsUpdate();
    }
    #endregion

    #region Other Func

    private void AnimationTrigger() => FSM.CurrentState.AnimationTrigger();

    #endregion

    #region Override

    public override void DieEffect()
    {
        base.DieEffect();

        FSM.ChangeState(DeathState);
    }

    public override void HitEffect()
    {
        base.HitEffect();
        Core.CoreKnockBackReceiver.TrapKnockBack(new Vector2(-1 * Core.CoreMovement.FancingDirection, 1f) , 10, false);
        //GameManager.Inst.MainUI.animator?.Play("Action", -1, 0f);
    }
    #endregion
}
