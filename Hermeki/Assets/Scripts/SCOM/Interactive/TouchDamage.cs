using UnityEngine;

public class TouchDamage : TouchObject
{
    public override void Touch(GameObject obj)
    {
        Unit tempUnit = obj.GetComponent<Unit>();

        if (tempUnit == null)
            return;

        tempUnit.Core?.CoreDamageReceiver?.Damage(null, 1);
        if (EffectObject)
            tempUnit?.Core.CoreEffectManager.StartEffectsPos(EffectObject, tempUnit.transform.position, EffectObject.transform.localScale);
        if (SFX.Clip)
            tempUnit?.Core.CoreSoundEffect.AudioSpawn(SFX);
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.GetComponent<Unit>() == null)
            return;

        Touch(collision.gameObject);        
    }
}
