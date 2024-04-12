using UnityEngine;

/// <summary>
/// Weapon의 형태를 가진 투사체
/// </summary>
public class ProjectileObject : ItemObject
{
    /// <summary>
    /// 투사체를던진 유닛
    /// </summary>
    protected Unit unit;
    protected LayerMask LM_Transparent;
    protected LayerMask LM_Object;
    protected override void Awake()
    {
        base.Awake();
        LM_Transparent = LayerMask.NameToLayer("Transparent");
        LM_Object = LayerMask.NameToLayer("Object");

        //충돌 가능하도록 Object로 변경
        SetLayerMask(LM_Object);

        this.tag = "Player";
        //collision
        this.GetComponentInParent<Collider2D>().isTrigger = false;
    }

    public virtual void InIt()
    {

    }
    public bool SetUnit(Unit _unit)
    {
        if (_unit == null)
            return false;
        this.unit = _unit;
        return true;
    }

    /// <summary>
    /// LayerMask 변경
    /// </summary>
    /// <param name="LM"></param>
    /// <returns></returns>
    public bool SetLayerMask(LayerMask LM)
    {
        if (this.gameObject.layer == LM_Object)
            return false;
        this.gameObject.layer = LM;
        return true;
    }

    public void SetGravityScale(float _value)
    {
        if (rb2d == null)
            return;
        rb2d.gravityScale = _value;
    }
    /// <summary>
    /// Ground 충돌 Enter 시
    /// </summary>
    public virtual void OnCollisionEnterGround(Collider2D collision)
    {

    }
    /// <summary>
    /// Ground 충돌 Exit 시
    /// </summary>
    /// <param name="collision"></param>
    public virtual void OnCollisionExitGround(Collider2D collision)
    {

    }
    /// <summary>
    /// Unit 충돌 Enter 시
    /// </summary>
    public virtual void OnCollisionEnterUnit(Collider2D collision)
    {

    }

    /// <summary>
    ///  Unit 충돌 Exit 시
    /// </summary>
    /// <param name="collision"></param>
    public virtual void OnCollisionExitUnit(Collider2D collision)
    {

    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        //Collision 처리될 시 하위 오브젝트의 Trigger호출로 인한 오류 방지
        if (!isTrigger)
            return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            OnCollisionEnterGround(collision);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Unit") && !collision.transform.CompareTag("Player"))
        {
            OnCollisionEnterUnit(collision);
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        //Collision 처리될 시 하위 오브젝트의 Trigger호출로 인한 오류 방지
        if (!isTrigger)
            return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            OnCollisionExitGround(collision);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Unit") && !collision.transform.CompareTag("Player"))
        {
            OnCollisionExitUnit(collision);
        }
    }
}
