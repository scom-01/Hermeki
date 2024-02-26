using Cinemachine;
using UnityEngine;

public class Trap : TouchObject
{
    public UnitData UnitData;
    public Vector2 knockbackAngle;
    [TagField]
    public string IgnoreTag;
    public override void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(IgnoreTag))
        {
            return;
        }
        if (collision.GetComponent<Unit>() == null)
            return;

        if (knockbackAngle.x != 0 || knockbackAngle.y != 0)
        {
            var knockback = collision.GetComponent<Unit>().Core.CoreKnockBackReceiver;
            if (knockback != null)
            {
                knockback.TrapKnockBack(knockbackAngle, knockbackAngle.magnitude);
                Debug.LogWarning(collision.name + "KnockBackReceiver" + "Trap");
            }
        }

        var damage = collision.GetComponent<Unit>().Core.CoreDamageReceiver;
        if (damage != null)
        {
            if (damage.TrapDamage(UnitData.statsStats, UnitData.statsStats.DefaultPower) > 0f)
            {
                if (EffectObject)
                    collision.GetComponent<Unit>()?.Core.CoreEffectManager.StartEffectsPos(EffectObject, collision.GetComponent<Unit>().transform.position, EffectObject.transform.localScale);
                if (SFX.Clip)
                    collision.GetComponent<Unit>()?.Core.CoreSoundEffect.AudioSpawn(SFX);
            }
            Debug.LogWarning(collision.name + "DamageReceiver" + "Trap");
        }
    }
}
