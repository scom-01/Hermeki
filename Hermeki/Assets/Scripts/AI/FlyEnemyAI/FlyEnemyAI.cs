using Pathfinding;
using Pathfinding.Util;
using System;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;

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
