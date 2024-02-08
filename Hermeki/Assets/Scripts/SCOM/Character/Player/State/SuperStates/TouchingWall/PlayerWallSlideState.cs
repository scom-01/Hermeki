using SCOM.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerTouchingWallState
{
    public PlayerWallSlideState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isExitingState) return;

        Movement.SetVelocityX(0);
        Movement.SetVelocityY(-player.playerData.wallSlideVelocity);
    }
}
