using SCOM.CoreSystem;
using UnityEngine;

public class HeadStomp : CoreComponent
{
    /// <summary>
    /// 머리밟기 여부 false = 밟기 가능
    /// </summary>
    bool isHeadStomp = false;
    [SerializeField]
    private float HeadStompCoolTime = 0.1f;
    private float HeadStompStartTime;
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isHeadStomp && Time.time >= HeadStompStartTime + HeadStompCoolTime)
        {
            isHeadStomp = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (core?.Unit == null)
            return;

        //사망 시
        if (!core.Unit.IsAlive)
            return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Damageable") && !collision.CompareTag(this.tag))
        {
            Unit unit = collision.GetComponentInParent<Unit>();
            if (unit == null || !unit.IsAlive)
                return;

            if (collision.TryGetComponent(out IDamageable victim))
            {
                //머리 밟기(플레이어의 바닥 y좌표가 해당 유닛의 (머리 + 중앙) / 2 보다 높거나 같을 때
                if (core.CoreCollisionSenses.GroundCenterPos.y >= ((unit.Core.CoreCollisionSenses.HeaderCenterPos.y + unit.Core.CoreCollisionSenses.UnitCenterPos.y) * 2 / 3))
                {
                    //한 번에 여러 유닛 머리 밟기로 데미지 입히지 못하도록
                    if (!isHeadStomp)
                    {
                        victim.Damage(core.Unit, 1);
                        core.CoreMovement.SetVelocityY(10);
                        HeadStompStartTime = Time.time;
                        isHeadStomp = true;
                    }
                }
                else
                {
                    core.CoreDamageReceiver.Damage(unit, 1);
                }
            }
        }
    }
}
