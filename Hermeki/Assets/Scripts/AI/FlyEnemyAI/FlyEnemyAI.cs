using Pathfinding;
using Pathfinding.Util;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;

public class FlyEnemyAI : AIPath
{
    [Header("Custom")]
    public float activateDistance;
    public bool isFollow = false;
    protected override void FixedUpdate()
    {
        base.FixedUpdate();        
        if (velocity2D.x > 0.05f)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (velocity2D.x < -0.05f)
        {
            transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        Debug.Log($"velocity2D = {velocity2D}");
    }

    protected override void Update()
    {
        base.Update();
        if (!TargetInDistance())
        {
            target = null;
        }
    }

    private bool TargetInDistance()
    {
        if (target == null)
            return false;
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    public override void OnTargetReached()
    {
        base.OnTargetReached();
        target = null;
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Draw.Gizmos.Cylinder(position, Vector3.forward, 0, activateDistance, UnityEngine.Color.blue);
    }    
}
