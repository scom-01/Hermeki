using SCOM.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CrushUnit : CoreComponent
{
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
            if (unit == null)
                return;

            if (collision.TryGetComponent(out IDamageable victim))
            {
                //머리 밟기(플레이어의 바닥 y좌표가 해당 유닛의 (머리 + 중앙)/2 보다 높거나 같을 때
                if (unit.Core.CoreCollisionSenses.GroundCenterPos.y >= ((core.CoreCollisionSenses.HeaderCenterPos.y + core.CoreCollisionSenses.UnitCenterPos.y) / 2))
                {
                    core.CoreDamageReceiver.Damage(unit, 1, 1);
                    unit.Core.CoreMovement.SetVelocityY(10);
                    //unit.Core.CoreKnockBackReceiver.TrapKnockBack(new Vector2(0, 1f), 10, false);
                }
                else
                {
                    victim.Damage(core.Unit, 1, 1);
                }
            }
        }
    }
}
