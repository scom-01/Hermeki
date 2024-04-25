using SCOM.CoreSystem;
using UnityEngine;

public class PlayerJumpState : PlayerInAirState
{
    private int amountOfJumpLeft;
    private GameObject Jump_Effect;
    private AudioClip Jump_Sfx;
    public PlayerJumpState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
        amountOfJumpLeft = player.playerData.amountOfJumps;

        if (Jump_Effect == null)
        {
            Jump_Effect = Resources.Load<GameObject>("Prefabs/Effects/Jump_Smoke");
        }
        if (Jump_Sfx == null)
        {
            Jump_Sfx = Resources.Load<AudioClip>("Sounds/Effects/SFX_Jump_01");
        }
    }

    public override void Enter()
    {
        base.Enter();
        //unit.RB.gravityScale = unit.UnitData.UnitGravity;                
        Jump();
    }

    private void Jump()
    {
        Debug.Log("Jump");
        player.isFixedMovement = false;
        player.ItemManager?.ItemExeOnJump();
        player.InputHandler.UseInput(ref player.InputHandler.JumpInput);
        Movement.SetVelocityY(UnitStats.CalculStatsData.DefaultJumpVelocity * (100f + UnitStats.CalculStatsData.JumpVEL_Per) / 100f);
        SoundEffect.AudioSpawn(Jump_Sfx);
        //Debug.Log("JumpSFX");
        //if (amountOfJumpLeft < player.playerData.amountOfJumps)
        //{
        //    player.Core.CoreEffectManager.StartEffectsPos(Jump_Effect, CollisionSenses.GroundCenterPos, Jump_Effect.transform.localScale);
        //    player?.SetAnimParam("JumpFlip", true);
        //}
        //if (player.PrimaryAttackState.weapon != null)
        //{
        //    player.PrimaryAttackState.weapon.ChangeActionCounter(0);
        //}

        DecreaseAmountOfJumpsLeft();
        Debug.Log("DecreaseJump");
        player.InAirState.SetIsJumping();
    }

    public bool CanJump() => (amountOfJumpLeft > 0);

    public void ResetAmountOfJumpsLeft()
    {
        amountOfJumpLeft = player.playerData.amountOfJumps;
        Debug.Log($"Reset amountOfJumpLeft = {amountOfJumpLeft}");
        player?.SetAnimParam("JumpFlip", false);
    }

    public void DecreaseAmountOfJumpsLeft()
    {
        Debug.Log($"amountOfJumpLeft = {amountOfJumpLeft}");
        amountOfJumpLeft--;
    }
}
