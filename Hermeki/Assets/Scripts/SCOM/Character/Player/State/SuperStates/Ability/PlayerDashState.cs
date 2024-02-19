using SCOM.CoreSystem;
using UnityEngine;


public class PlayerDashState : PlayerAbilityState
{
    public bool CanDash { get; private set; }
    public int DashCount { get; private set; }
    private float lastDashTime;

    private Vector2 lastAIPos;

    private GameObject Dash_Effect;
    private GameObject AirDash_Effect;
    private AudioClip Dash_SFX;

    public PlayerDashState(Unit unit, string animBoolName) : base(unit, animBoolName)
    {
        if (Dash_Effect == null)
        {
            Dash_Effect = Resources.Load<GameObject>("Prefabs/Effects/Dash_Smoke");
        }
        if (AirDash_Effect == null)
        {
            AirDash_Effect = Resources.Load<GameObject>("Prefabs/Effects/Dash_Smoke");
        }
        if (Dash_SFX == null)
        {
            Dash_SFX = Resources.Load<AudioClip>("Sounds/Effects/SFX_Dash_01");
        }
    }

    public override void Enter()
    {
        player.isFixedMovement = false;
        if (isGrounded)
        {
            animBoolName = "dash";
        }
        else
        {
            animBoolName = "airdash";
        }
        base.Enter();
        Dash();
        startTime = Time.time;
    }

    public override void Exit()
    {
        base.Exit();
        player.RB.gravityScale = unit.UnitData.UnitGravity;
        player.Set_Fixed_Hit_Immunity(false);
        player.isCC_immunity = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isExitingState)
            return;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        Movement.SetVelocityX(player.playerData.dashVelocity * Movement.FancingDirection);
        Movement.SetVelocityY(0);

        CheckIfShouldPlaceAfterImage();

        if (CheckActionInput())
            return;

        //대쉬 지속시간 종료
        if (Time.time >= startTime + player.playerData.dashDuration)
        {
            if (DashCount > 0)
            {
                CanDash = true;
            }
            if (player.InputHandler.DashInput)
            {
                lastDashTime = Time.time;
                if (CheckIfCanDash())
                {
                    Movement.CheckIfShouldFlip(player.InputHandler.NormInputX);
                    player.FSM.ChangeState(this);
                }
            }

            Movement.SetVelocityZero();
            if (Time.time >= startTime + player.playerData.dashDuration + player.playerData.dashFlightDuration)
            {
                isAbilityDone = true;
                lastDashTime = Time.time;
            }
        }
    }

    private void Dash()
    {
        player.Inventory.ItemExeDash(unit, CanDash);

        Movement.CheckIfShouldFlip(player.InputHandler.NormInputX);
        SoundEffect.AudioSpawn(Dash_SFX);

        player.Set_Fixed_Hit_Immunity(true);
        player.isCC_immunity = true;

        CanDash = false;

        player.InputHandler.UseInput(ref player.InputHandler.DashInput);
        if(isGrounded)
        {
            if (Dash_Effect != null)
            {
                EffectManager.StartEffectsPos(Dash_Effect, CollisionSenses.GroundCenterPos, Dash_Effect.transform.localScale);
            }
        }
        else
        {
            if (AirDash_Effect != null)
            {
                EffectManager.StartEffectsPos(AirDash_Effect, CollisionSenses.GroundCenterPos, AirDash_Effect.transform.localScale);
            }
        }
        
        Movement.SetVelocityY(0f);
        player.RB.gravityScale = 0f;
        DecreaseDashCount();
    }

    private void CheckIfShouldPlaceAfterImage()
    {
        if (Vector2.Distance(player.transform.position, lastAIPos) >= player.playerData.distBetweenAfterImages)
        {
            PlaceAfterImage();
        }
    }

    private void PlaceAfterImage()
    {
        PlayerAfterImagePool.Instance.GetFromPool();
        lastAIPos = player.transform.position;
    }

    public bool CheckIfCanDash() => CanDash && Time.time >= lastDashTime + player.playerData.dashCooldown && DashCount > 0;
    public void DecreaseDashCount() => DashCount--;
    public bool CheckIfResetDash()
    {
        return Time.time >= lastDashTime + player.playerData.dashResetCooldown;
    }
    public void ResetCanDash() => CanDash = true;

    public void ResetDash(int count) => DashCount = count;

}
