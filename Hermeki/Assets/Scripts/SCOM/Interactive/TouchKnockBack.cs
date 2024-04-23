using UnityEngine;

public class TouchKnockBack : TouchObject
{
    public override void Touch(GameObject obj)
    {
        Unit tempUnit = obj.GetComponent<Unit>();

        if (tempUnit == null)
            return;

        tempUnit.Core?.CoreKnockBackReceiver?.KnockBack(new Vector2(-0.5f, 0.5f), 1, -tempUnit.Core.CoreMovement.FancingDirection);
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.GetComponent<Unit>() == null)
            return;

        Touch(collision.gameObject);
    }
}
