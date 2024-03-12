using Pathfinding;
using System;

public class FlyEnemyAI : AIPath
{
    private Enemy enemy;
    public Action TargetReachedAction;
    protected override void Awake()
    {
        base.Awake();
        enemy = this.GetComponent<Enemy>();
    }
    public override void OnTargetReached()
    {
        base.OnTargetReached();
        TargetReachedAction?.Invoke();
    }
}
