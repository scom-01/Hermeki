public class PlayerAbilityState : PlayerState
{
    protected bool isAbilityDone;

    public PlayerAbilityState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        isAbilityDone = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAbilityDone && player.IsAlive)
        {
            if (isGrounded && Movement.CurrentVelocity.y <= 0.01f)
            {
                player.FSM.ChangeState(player.IdleState);
            }
            else
            {
                player.FSM.ChangeState(player.InAirState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAbilityDone = true;
    }
}
