using System.Collections.Generic;
using UnityEngine;

public class TouchJumpPad : TouchObject
{
    [SerializeField] private float JumpVelocity;
    [SerializeField] private Vector2 angle;
    private List<Unit> _unitList = new List<Unit>();
    private BoxCollider2D BC2D;
    public Animator animator
    {
        get
        {
            if (anim == null)
            {
                anim = GetComponent<Animator>();
            }
            return anim;
        }
    }
    private Animator anim;
    private void Awake()
    {
        if (BC2D == null)
            BC2D = this.GetComponent<BoxCollider2D>();
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerStay2D(collision);

        Collision(collision.GetComponent<Unit>());
    }

    public override void Touch(GameObject obj)
    {
        base.Touch(obj);
        if (animator != null)
            animator.Play("Action", -1, 0);
    }

    public override void UnTouch(GameObject obj)
    {
        base.UnTouch(obj);  
        if (animator != null)
            animator.Play("UnAction", -1, 0);
    }

    private void Collision(Unit unit)
    {
        if (unit == null)
            return;
        
        Touch(unit.gameObject);
        if (EffectObject)
            unit.Core.CoreEffectManager.StartEffectsPos(EffectObject, unit.transform.position, EffectObject.transform.localScale);
        if (SFX.Clip)
            unit.Core.CoreSoundEffect.AudioSpawn(SFX);
        //x = 0 인 위로만 올리는 점프패드일 때는 공중 움직임 제한 X
        if (angle.x == 0)
        {
            unit.Core.CoreMovement.SetVelocityY(JumpVelocity);
        }
        else
        {
            unit.Core.CoreKnockBackReceiver.TrapKnockBack(angle, JumpVelocity, false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 m_angle = angle;
        m_angle.Normalize();
        Debug.DrawRay(transform.position, new Vector3(m_angle.x, m_angle.y, 0), Color.red);
    }
}
