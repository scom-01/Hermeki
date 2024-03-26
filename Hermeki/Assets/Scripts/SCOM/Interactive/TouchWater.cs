using UnityEngine;

public class TouchWater : TouchLowGravity
{
    public override void OnTriggerStay2D(Collider2D collision)
    {
        base.OnTriggerStay2D(collision);
        if (collision.GetComponent<Unit>() == null)
        {
            return;
        }

        Player tempUnit = collision.GetComponent<Player>();
        if (tempUnit != null && !tempUnit.JumpState.CanJump())
        {
            tempUnit.JumpState.ResetAmountOfJumpsLeft();
        }
    }

    public override void UnTouch(GameObject obj)
    {
        base.UnTouch(obj);
        Player tempUnit = obj.GetComponent<Player>();
        if (tempUnit != null && tempUnit.JumpState.CanJump())
        {
            tempUnit.JumpState.DecreaseAmountOfJumpsLeft();
        }
    }
}
