public class PlayerDeathState : PlayerAbilityState
{
    public PlayerDeathState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        unit.Core.CoreMovement.SetVelocityX(0);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}