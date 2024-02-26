using SCOM.CoreSystem;
using System;
using UnityEngine;
using UnityEngine.Localization.PropertyVariants.TrackedProperties;

public class EnemyCollisionSenses : CollisionSenses
{
    #region DetectedArea
    public bool isUnitDetectedBox
    {
        get
        {
            return (isUnitInFrontDetectedArea || isUnitInBackDetectedArea);
        }
    }
    public bool isUnitInFrontDetectedArea
    {
        get
        {
            var RayHit = Physics2D.BoxCastAll
                (
                    new Vector2(UnitCenterFront.x, UnitCenterFront.y),
                    CC2D.bounds.size,
                    0f,
                    Vector2.right * Movement.FancingDirection,
                    (core.Unit.UnitData as EnemyData).UnitDetectedDistance,
                    1 << LayerMask.NameToLayer("Unit")
                );
            foreach (var hit in RayHit)
            {
                if (hit.transform.tag != core.Unit.tag)
                {
                    return true;
                }
            }
            return false;
        }
    }
    public bool isUnitInBackDetectedArea
    {
        get
        {
            var RayHit = Physics2D.BoxCastAll
                (
                    new Vector2(UnitCenterBack.x, UnitCenterBack.y),
                    CC2D.bounds.size,
                    0f,
                    Vector2.right * -Movement.FancingDirection,
                    (core.Unit.UnitData as EnemyData).UnitDetectedDistance,
                    1 << LayerMask.NameToLayer("Unit")
                );

            foreach (var hit in RayHit)
            {
                if (hit.transform.tag != core.Unit.tag)
                {
                    return true;
                }
            }
            return false;
        }
    }
    public bool isUnitDetectedCircle
    {
        get
        {
            var RayHit = Physics2D.CircleCastAll
                (
                    new Vector2(UnitCenterPos.x, UnitCenterPos.y),
                    (core.Unit.UnitData as EnemyData).UnitDetectedDistance,
                    Vector2.right * Movement.FancingDirection,
                    0,
                    1 << LayerMask.NameToLayer("Unit")
                );

            foreach (var hit in RayHit)
            {
                if (hit.transform.tag != core.Unit.tag)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public bool CheckUnitDistBox(float _dist)
    {
        return (CheckUnitFrontDistBox(_dist) || CheckUnitBackDistBox(_dist));
    }
    public bool CheckUnitFrontDistBox(float _dist)
    {
        var RayHit = Physics2D.BoxCastAll
                (
                    new Vector2(UnitCenterFront.x, UnitCenterFront.y),
                    CC2D.bounds.size,
                    0f,
                    Vector2.right * Movement.FancingDirection,
                    _dist,
                    1 << LayerMask.NameToLayer("Unit")
                );
        foreach (var hit in RayHit)
        {
            if (hit.transform.tag != core.Unit.tag)
            {
                return true;
            }
        }
        return false;
    }
    public bool CheckUnitBackDistBox(float _dist)
    {
        var RayHit = Physics2D.BoxCastAll
                (
                    new Vector2(UnitCenterBack.x, UnitCenterBack.y),
                    CC2D.bounds.size,
                    0f,
                    Vector2.right * -Movement.FancingDirection,
                    _dist,
                    1 << LayerMask.NameToLayer("Unit")
                );

        foreach (var hit in RayHit)
        {
            if (hit.transform.tag != core.Unit.tag)
            {
                return true;
            }
        }
        return false;
    }
    public bool CheckUnitDetectedCircle(Unit unit)
    {
        if (unit == null)
            return false;

        Vector2 offset = Vector2.zero;
        float size = (core.Unit.UnitData as EnemyData).UnitDetectedDistance;
        offset.Set(GroundCenterPos.x + (-CC2D.size.x * Movement.FancingDirection), GroundCenterPos.y);
        var detected = Physics2D.OverlapCircleAll(offset, size, 1 << LayerMask.NameToLayer("Unit"));

        foreach (Collider2D coll in detected)
        {
            if (coll.transform.CompareTag(core.Unit.tag))
                continue;


            if (coll.GetComponent<Unit>() == core.Unit.GetTarget())
            {
                return true;
            }
        }
        return false;
    }
    #endregion

    #region DetectedUnit
    public GameObject UnitDetectedBox
    {
        get
        {
            float FrontDist = -1f, BackDist = -1f;
            if (UnitFrontDetectArea != null)
                FrontDist = (UnitFrontDetectArea.transform.position - this.transform.position).magnitude;
            if (UnitBackDetectArea != null)
                BackDist = (UnitBackDetectArea.transform.position - this.transform.position).magnitude;

            if (FrontDist >= BackDist)
            {
                return UnitFrontDetectArea;
            }
            else
            {
                return UnitBackDetectArea;
            }
        }
    }
    public GameObject UnitFrontDetectArea
    {
        get
        {
            var RayHit = Physics2D.BoxCastAll
                (
                    new Vector2(UnitCenterFront.x, UnitCenterFront.y),
                    CC2D.bounds.size,
                    0f,
                    Vector2.right * Movement.FancingDirection,
                    ((core.Unit.UnitData as EnemyData).UnitDetectedDistance - CC2D.size.x < 0) ? 0 : (core.Unit.UnitData as EnemyData).UnitDetectedDistance - CC2D.size.x,
                    1 << LayerMask.NameToLayer("Unit")
                );
            foreach (var coll in RayHit)
            {
                if (coll.transform.CompareTag(core.Unit.tag))
                    continue;

                if (coll.collider.GetComponent<Unit>())
                {
                    return coll.collider.gameObject;
                }
            }
            return null;
        }
    }
    public GameObject UnitBackDetectArea
    {
        get
        {
            var RayHit = Physics2D.BoxCastAll
                (
                    new Vector2(UnitCenterBack.x, UnitCenterBack.y),
                    CC2D.bounds.size,
                    0f,
                    Vector2.right * -Movement.FancingDirection,
                     ((core.Unit.UnitData as EnemyData).UnitDetectedDistance - CC2D.size.x < 0) ? 0 : (core.Unit.UnitData as EnemyData).UnitDetectedDistance - CC2D.size.x,
                    1 << LayerMask.NameToLayer("Unit")
                );
            foreach (var coll in RayHit)
            {
                if (coll.transform.CompareTag(core.Unit.tag))
                    continue;

                if (coll.collider.GetComponent<Unit>())
                {
                    return coll.collider.gameObject;
                }
            }
            return null;
        }
    }
    public GameObject UnitDetectedCircle
    {
        get
        {
            Vector2 offset = Vector2.zero;
            float size = (core.Unit.UnitData as EnemyData).UnitDetectedDistance;
            offset.Set(GroundCenterPos.x + (-CC2D.size.x * Movement.FancingDirection), GroundCenterPos.y);
            var detected = Physics2D.OverlapCircleAll(offset, size, 1 << LayerMask.NameToLayer("Unit"));

            foreach (Collider2D coll in detected)
            {
                if (coll.transform.CompareTag(core.Unit.tag))
                    continue;

                if (coll.GetComponent<Unit>())
                {
                    return coll.gameObject;
                }
            }
            return null;
        }
    }
    #endregion

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (CC2D == null)
            return;

        Gizmos.color = Color.cyan;
        //AttackArea
        Gizmos.DrawWireCube(
            new Vector3(UnitCenterPos.x + ((core.Unit.UnitData as EnemyData).UnitAttackDistance / 2 * Movement.FancingDirection), UnitCenterPos.y, 0),
            new Vector2((core.Unit.UnitData as EnemyData).UnitAttackDistance, CC2D.bounds.size.y));
        Gizmos.DrawWireCube(
            new Vector3(UnitCenterPos.x + ((core.Unit.UnitData as EnemyData).UnitAttackDistance / 2 * -Movement.FancingDirection), UnitCenterPos.y, 0),
            new Vector2((core.Unit.UnitData as EnemyData).UnitAttackDistance, CC2D.bounds.size.y));

        //UnitDetectedDistance
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(UnitCenterPos, (core.Unit.UnitData as EnemyData).UnitDetectedDistance);

        Gizmos.color = Color.red;
        //front
        Gizmos.DrawWireCube(
            new Vector3(UnitCenterPos.x + (((core.Unit.UnitData as EnemyData).UnitDetectedDistance / 2) * Movement.FancingDirection), UnitCenterPos.y, 0),
            new Vector2((core.Unit.UnitData as EnemyData).UnitDetectedDistance, CC2D.bounds.size.y));

        Gizmos.color = Color.blue;
        //back
        Gizmos.DrawWireCube(
            new Vector3(UnitCenterPos.x + (((core.Unit.UnitData as EnemyData).UnitDetectedDistance / 2) * -1f * Movement.FancingDirection), UnitCenterPos.y, 0),
            new Vector2((core.Unit.UnitData as EnemyData).UnitDetectedDistance, CC2D.bounds.size.y));
    }

    protected override void Awake()
    {
        base.Awake();
        this.tag = core.Unit.gameObject.tag;
    }
}
