using System;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    public bool isAction;
    public event Action OnFinish;

    //Movement
    public event Action OnFixedStartMovement;
    public event Action OnFixedStopMovement;
    public event Action OnMovementAction;
    public event Action OnStartMovement;
    public event Action OnStopMovement;
    public event Action OnStartFlip;
    public event Action OnStopFlip;

    public event Action OnTeleportToTarget;
    public event Action OnTeleportToPoint;
    public event Action OnRushToTargetOn;
    public event Action OnRushToTargetOff;

    //Action
    public event Action OnAttMessageBox;
    public event Action OnAttackAction;
    public event Action OnActionRectOn;
    public event Action OnActionRectOff;
    public event Action OnMultipleActionRectOn;
    public event Action OnMultipleActionRectOff;
    public event Action OnRushActionRectOn;
    public event Action OnRushActionRectOff;
    public event Action OnShootProjectile;

    public event Action OnLeftAction;
    public event Action OnRightAction;
    public event Action OnLeftActionFinish;
    public event Action OnRightActionFinish;

    //Buff
    public event Action OnWeaponBuff;

    //Effect
    public event Action OnEffectSpawn;

    //Sound
    public event Action OnSoundClip;

    //Cam
    public event Action OnShakeCam;

    //State
    public event Action OnStartInvincible;
    public event Action OnStopInvincible;
    public event Action OnStartCCImmunity;
    public event Action OnStopCCImmunity;

    public void AnimationFinishedTrigger()
    {
        isAction = false;
        OnFinish?.Invoke();
    }

    //Movement
    public void FixedStartMovementTrigger() { if (isAction) OnFixedStartMovement?.Invoke(); }
    public void FixedStopMovementTrigger() { if (isAction) OnFixedStopMovement?.Invoke(); }
    public void MovementAction() { if (isAction) OnMovementAction?.Invoke(); }
    public void StartMovementTrigger() { if (isAction) OnStartMovement?.Invoke(); }
    public void StopMovementTrigger() { if (isAction) OnStopMovement?.Invoke(); }
    public void StartFlipTrigger() { if (isAction) OnStartFlip?.Invoke(); }
    public void StopFlipTrigger() { if (isAction) OnStopFlip?.Invoke(); }

    public void TeleportToTargetTrigger() { if (isAction) OnTeleportToTarget?.Invoke(); }
    public void TeleportToPointTrigger() { if (isAction) OnTeleportToPoint?.Invoke(); }
    public void StartRushToTargetTrigger() { if (isAction) OnRushToTargetOn?.Invoke(); }
    public void StopRushToTargetTrigger() { if (isAction) OnRushToTargetOff?.Invoke(); }

    //Action
    public void AttackMessageBoxTrigger() { if (isAction) OnAttMessageBox?.Invoke(); }
    public void AttackActionTrigger() { if (isAction) OnAttackAction?.Invoke(); }
    public void StartActionRectTrigger() { if (isAction) OnActionRectOn?.Invoke(); }
    public void StopActionRectTrigger() { if (isAction) OnActionRectOff?.Invoke(); }
    public void StartMultipleActionRectTrigger() { if (isAction) OnMultipleActionRectOn?.Invoke(); }
    public void StopMultipleActionRectTrigger() { if (isAction) OnMultipleActionRectOff?.Invoke(); }
    public void StartRushActionRectTrigger() { if (isAction) OnRushActionRectOn?.Invoke(); }
    public void StopRushActionRectTrigger() { if (isAction) OnRushActionRectOff?.Invoke(); }
    public void ShootProjectileTrigger() { if (isAction) OnShootProjectile?.Invoke(); }

    public void StartLeftAction() { OnLeftAction?.Invoke(); }
    public void StartRightAction() { OnRightAction?.Invoke(); }
    public void FinishLeftAction() { OnLeftActionFinish?.Invoke(); }
    public void FinishRightAction() { OnRightActionFinish?.Invoke(); }
    
    //Buff
    public void WeaponBuffTrigger() { if (isAction) OnWeaponBuff?.Invoke(); }

    //Effect
    public void SpawnEffectTrigger() { if (isAction) OnEffectSpawn?.Invoke(); }

    //Sound
    public void SpawnSoundClipTrigger() { if (isAction) OnSoundClip?.Invoke(); }

    //Cam
    public void ShakeCamTrigger() { if (isAction) OnShakeCam?.Invoke(); }

    //State
    public void StartInvincibleTrigger() { if (isAction) OnStartInvincible?.Invoke(); }
    public void StopInvincibleTrigger() { if (isAction) OnStopInvincible?.Invoke(); }
    public void StartCCImmunityTrigger() { if (isAction) OnStartCCImmunity?.Invoke(); }
    public void StopCCImmunityTrigger() { if (isAction) OnStopCCImmunity?.Invoke(); }
}
