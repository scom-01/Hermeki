using UnityEngine;

public class SpearObject : ProjectileObject
{
    /// <summary>
    /// 중력
    /// </summary>
    public float gravityScale;
    /// <summary>
    /// 이전 중력
    /// </summary>
    private float oldgravityScale;
    /// <summary>
    /// true 시 관통 
    /// </summary>
    public bool isThrough = false;
    protected override void Awake()
    {
        base.Awake();
        oldgravityScale = rb2d.gravityScale;
        rb2d.gravityScale = gravityScale;
    }

    public override void InIt()
    {
        this.transform.localScale = Vector3.one * 1.6f;
        this.transform.eulerAngles = Vector3.forward * 90 * -unit.Core.CoreMovement.FancingDirection;
        this.GetComponentInParent<Rigidbody2D>().AddForce(Vector2.right * unit.Core.CoreMovement.FancingDirection * 500f);
    }
    public override void OnCollisionEnterGround(Collider2D collision)
    {
        //충돌 시 isTrigger = false;
        isTrigger = false;
        pc2d.isTrigger = isTrigger;

        //튕기기
        Vector2 oldvec = rb2d.velocity;                
        rb2d.velocity = -oldvec * 0.3f;

        //이전 중력으로 되돌림
        rb2d.gravityScale = oldgravityScale;

        //충돌 시 Layer = "Transparent";
        SetLayerMask(LM_Transparent);

        //아이템 효과
        EquipItemEventSet ItemEvent = new EquipItemEventSet(data.dataSO);
        unit?.ItemManager?.ExeItemEvent(ItemEvent, ItemEvent_Type.OnHitGround);
    }

    public override void OnCollisionEnterUnit(Collider2D collision)
    {
        //사망 시 무시
        if (collision.gameObject?.GetComponent<Unit>()?.IsAlive == false)
            return;

        //관통 여부
        if (!isThrough)
        {
            //충돌 시 isTrigger = false;
            isTrigger = false;
            pc2d.isTrigger = isTrigger;

            //튕기기
            Vector2 oldvec = rb2d.velocity;
            rb2d.velocity = -oldvec * 0.3f;

            //이전 중력으로 되돌림
            rb2d.gravityScale = oldgravityScale;

            //충돌 시 Layer = "Transparent";
            SetLayerMask(LM_Transparent);
        }

        //데미지
        collision.gameObject.GetComponent<Unit>().Core.CoreDamageReceiver.Damage(null, 1);

        //아이템 효과
        EquipItemEventSet ItemEvent = new EquipItemEventSet(data.dataSO);
        unit?.ItemManager?.ExeItemEvent(ItemEvent, ItemEvent_Type.OnHitEnemy);
    }
}
