using UnityEngine;

public class TouchLowGravity : TouchObject
{
    protected virtual void Awake()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Area");
    }
    public override void Touch(GameObject obj)
    {
        base.Touch(obj);
        Unit tempUnit = obj.GetComponent<Unit>();
        if (tempUnit != null)
        {
            tempUnit.RB.drag = 1.75f;
            tempUnit.RB.angularDrag = 0.5f;
            tempUnit.RB.gravityScale = 2f;
        }
    }

    public override void UnTouch(GameObject obj)
    {
        base.UnTouch(obj);
        Unit tempUnit = obj.GetComponent<Unit>();
        if (tempUnit != null)
        {
            tempUnit.RB.drag = 0f;
            tempUnit.RB.angularDrag = 0.05f;
            tempUnit.RB.gravityScale = tempUnit.UnitData.UnitGravity;
        }
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.GetComponent<Unit>() == null)
        {
            return;
        }
        Touch(collision.gameObject);
    }

    public override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        if (collision.GetComponent<Unit>() == null)
        {
            return;
        }
        UnTouch(collision.gameObject);
    }
}
