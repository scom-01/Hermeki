using SCOM.CoreSystem;
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

        if (tempUnit != null)
        {
            tempUnit.Core.CoreMovement.SetVelocityY(
                    Mathf.Clamp(
                        tempUnit.RB.velocity.y,
                        -7f,
                        tempUnit.Core.CoreUnitStats.CalculStatsData.DefaultJumpVelocity * (100f + tempUnit.Core.CoreUnitStats.CalculStatsData.JumpVEL_Per) / 100f
                        )
                    );
        }

        if (tempUnit != null && tempUnit.JumpState.GetJumpInput /*&&!tempUnit.JumpState.CanJump()*/)
        {
            tempUnit.FSM.ChangeState(tempUnit.JumpState);
            //tempUnit.JumpState.ResetAmountOfJumpsLeft();
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
