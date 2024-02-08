using SCOM.CoreSystem;

public class EnemyState : UnitState
{
    protected bool isAbilityDone;
    protected bool isGrounded;
    protected Enemy enemy;
    protected bool isCliff;
    protected bool isCliffBack;
    protected bool CheckifTouchingGrounded;
    protected bool isTouchingWall;
    protected bool isTouchingWallBack;

    protected bool isDelayCheck = false;
    protected EnemyCollisionSenses EnemyCollisionSenses
    {
        get => collisionSenses ?? enemy.Core.GetCoreComponent(ref collisionSenses);
    }

    private EnemyCollisionSenses collisionSenses;
    public EnemyState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
        enemy = unit as Enemy;
        this.animBoolName = animBoolName;
    }
    public override void DoChecks()
    {
        base.DoChecks();

        if (EnemyCollisionSenses)
        {
            isGrounded = (EnemyCollisionSenses.CheckIfGrounded || EnemyCollisionSenses.CheckIfPlatform);
            isCliff = EnemyCollisionSenses.CheckIfCliff;
            isCliffBack = EnemyCollisionSenses.CheckIfCliffBack;
            isTouchingWall = EnemyCollisionSenses.CheckIfTouchingWall;
            isTouchingWallBack = EnemyCollisionSenses.CheckIfTouchingWallBack;
        }
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

    public virtual void Pattern() { }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Death.isDead)
            return;
    }
}
