using UnityEngine;

public class ShadowController : MonoBehaviour
{
    Unit unit;
    /// <summary>
    /// 그림자 오브젝트 Transform
    /// </summary>
    [SerializeField]
    Transform ShadowSprite;
    Vector3 ShadowScale;
    SpriteRenderer SR;
    float ShadowAlpha = 1f;
    /// <summary>
    /// 그림자 감지 거리
    /// </summary>
    [SerializeField]
    private float ShadowDistance = 5f;
    protected ContactFilter2D contactFilter_Ground;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    private void Awake()
    {
        unit = this.GetComponentInParent<Unit>();
        contactFilter_Ground.SetLayerMask(unit.Core.CoreCollisionSenses.WhatIsGround);
        contactFilter_Ground.useLayerMask = true;
        if (ShadowSprite != null)
        {
            ShadowScale = ShadowSprite.localScale;
            SR = ShadowSprite.GetComponent<SpriteRenderer>();
            ShadowAlpha = SR.color.a;
        }
    }
    private void Update()
    {
        if (unit == null || ShadowSprite == null)
            return;

        //GroundCenterPos에서 Vector2.down 방향으로 ShadowDistance만큼 Raycast
        var count = Physics2D.Raycast(unit.Core.CoreCollisionSenses.GroundCenterPos, Vector2.down, contactFilter_Ground, hitBuffer, ShadowDistance);
        if (count > 0)
        {
            foreach (var hit in hitBuffer)
            {
                if (hit.rigidbody == null || hit.transform.CompareTag("Platform"))
                    continue;

                //hit의 기울기(양수면 hit의 y가 더 낮은 위치, 즉 GroundCenterPos가 hit.point보다 위에 있으면 양수)
                if (hit.normal.y < 0.9f)
                    continue;
                
                //raycast hit와의 거리가 그림자 감지 거리보다 작을 때
                if (hit.distance <= ShadowDistance)
                {
                    ShadowSprite.transform.position = new Vector3(ShadowSprite.transform.position.x, hit.point.y, 0);
                    ShadowSprite.localScale = ShadowScale * ((ShadowDistance - hit.distance) / ShadowDistance);
                    SR.color = new Color(SR.color.r, SR.color.g, SR.color.b, (ShadowAlpha * ((ShadowDistance - hit.distance) / ShadowDistance)));
                }
                else
                {                    
                    SR.color = new Color(SR.color.r, SR.color.g, SR.color.b, 0);
                }
                break;
            }
        }
        else
        {
            ShadowSprite.localScale = Vector3.zero;
        }
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
