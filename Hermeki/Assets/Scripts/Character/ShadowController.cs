using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowController : MonoBehaviour
{
    Unit unit;
    [SerializeField]
    Transform ShadowSprite;
    Vector3 ShadowScale;
    [SerializeField]
    private float ShadowDistance = 5f;
    protected ContactFilter2D contactFilter_Ground;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    private void Awake()
    {
        unit = this.transform.root.GetComponent<Unit>();
        contactFilter_Ground.SetLayerMask(unit.Core.CoreCollisionSenses.WhatIsGround);
        contactFilter_Ground.useLayerMask = true;
        if (ShadowSprite != null)
            ShadowScale = ShadowSprite.localScale;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (unit == null || ShadowSprite == null)
            return;
        
        var count = Physics2D.Raycast(unit.Core.CoreCollisionSenses.GroundCenterPos, Vector2.down, contactFilter_Ground, hitBuffer, ShadowDistance);
        if (count > 0)
        {
            foreach (var hit in hitBuffer)
            {
                if (hit.rigidbody == null)
                    continue;

                if (hit.transform.CompareTag("Platform"))
                    continue;

                //hit의 기울기(양수면 hit의 y가 더 낮은 위치, 즉 GroundCenterPos가 hit.point보다 위에 있으면 양수)
                if (hit.normal.y < 0.9f)
                    continue;

                if (hit.distance <= ShadowDistance)
                {
                    ShadowSprite.transform.position = new Vector3(ShadowSprite.transform.position.x, hit.point.y, 0);
                    ShadowSprite.localScale = ShadowScale * ((ShadowDistance - hit.distance) / ShadowDistance);
                }
                Debug.Log($"Shadow ShadowSprite.transform.position= {ShadowSprite.transform.position}");
                //hit의 포인트(Ray가 부딪힌 지점)
                if (unit.Core.CoreCollisionSenses.GroundCenterPos.y > hit.point.y)
                {
                    Debug.Log($"Shadow hit = {hit}");
                    break;
                    //return;
                }
            }
        }
        else
        {
            ShadowSprite.localScale = Vector3.zero;
        }
        //return;

        //if (unit.Core.CoreCollisionSenses.CheckIfGrounded)
    }

    private void OnDrawGizmos()
    {
        if (unit == null)
            return;

        //checkGround,Platform
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(unit.Core.CoreCollisionSenses.GroundCenterPos, unit.Core.CoreCollisionSenses.GroundCenterPos + Vector3.down * (unit.Core.CoreCollisionSenses.GroundCheckDistance));
    }
}
