using UnityEngine;

namespace SCOM.Weapons.Components
{
    public class WeaponMovement : WeaponComponent<MovementData, ActionMovement>
    {
        private int currentMovementIndex = -1;
        private bool isFixedRush = false;
        private Vector3 RushPoint;
        private Vector3 RushStartPoint;
        private float RushDurationTime = 0f;
        private float timer = 0;
        protected override void HandleEnter()
        {
            base.HandleEnter();
            currentMovementIndex = 0;
        }

        #region 공격시 방향키 이동이 가능한 Movement
        //만약 방향키를 누르지 않으면 
        private void HandleStartMovement()
        {
            CoreMovement.CanMovement = true;
        }
        private void HandleStopMovement()
        {
            CoreMovement.CanMovement = false;
            CoreMovement.SetVelocityX(0f);
        }
        #endregion

        private void HandleMovementAction()
        {
            if (currentActionData.movements.Length == 0)
                return;

            CoreMovement.CanMovement = currentActionData.movements[currentMovementIndex].CanMoveCtrl;

            if (currentActionData.movements[currentMovementIndex].Direction.x != 0 && currentActionData.movements[currentMovementIndex].Direction.y == 0)
            {
                CoreMovement.SetVelocityX(currentActionData.movements[currentMovementIndex].Velocity * CoreMovement.FancingDirection);
            }
            else if (currentActionData.movements[currentMovementIndex].Direction.x == 0 && currentActionData.movements[currentMovementIndex].Direction.y != 0)
            {
                CoreMovement.SetVelocityY(currentActionData.movements[currentMovementIndex].Velocity);
            }
            else
            {
                CoreMovement.SetVelocity(currentActionData.movements[currentMovementIndex].Velocity, currentActionData.movements[currentMovementIndex].Direction, CoreMovement.FancingDirection);
            }
        }

        #region 고정 Movement
        private void HandleFixedStartMovement()
        {
            if (currentActionData?.movements == null)
                return;

            if (currentActionData.movements.Length == 0 || currentActionData.movements.Length < currentMovementIndex - 1)
            {
                Debug.Log("Movement Length zero");
                return;
            }

            Debug.Log("HandleFixedStart");
            CoreMovement.CanMovement = currentActionData.movements[currentMovementIndex].CanMoveCtrl;
            if (currentActionData?.movements != null)
            {
                CheckMovementAction(currentActionData);
            }
            FixedGravityOn();
            currentMovementIndex++;
        }
        private void HandleFixedStopMovement()
        {
            if (currentActionData?.movements == null)
                return;

            Debug.Log("HandleFixedStop");
            if (currentActionData.movements.Length == 0)
            {
                return;
            }
            if (!CoreMovement.CanMovement)
            {
                CoreMovement.SetVelocityX(0f);
            }
            CoreMovement.CanMovement = false;
            FixedGravityOff();
        }
        private void FixedGravityOn()
        {
            core.Unit.RB.gravityScale = 0f;
        }
        private void FixedGravityOff()
        {
            core.Unit.RB.gravityScale = core.Unit.UnitData.UnitGravity;
        }

        #endregion

        #region Teleport
        private void HandleTeleport()
        {
            if (core.Unit.TargetUnit == null)
            {
                Debug.LogWarning("Target not found");
                return;
            }
            CoreMovement.SetVelocityZero();

            //타겟 유닛의 뒤로 이동
            core.Unit.transform.position = core.Unit.TargetUnit.Core.CoreCollisionSenses.UnitCenterBack + Vector3.right * 2 * -core.Unit.TargetUnit.Core.CoreMovement.FancingDirection;
        }

        private void HandleToPointTeleport()
        {
            //if ((GameManager.Inst?.StageManager as BossStageManager)?.TeleportPoints.Count > 0)
            //{
            //    CoreMovement.SetVelocityZero();

            //    var temp = UnityEngine.Random.Range(0, (GameManager.Inst.StageManager as BossStageManager).TeleportPoints.Count - 1);
            //    core.Unit.transform.position = (GameManager.Inst.StageManager as BossStageManager).TeleportPoints[temp].transform.position;
            //}
        }

        private void HandleRushOn()
        {
            isFixedRush = true;
            if (currentActionData != null)
            {
                CheckRushAction(currentActionData);
            }
            else
            {
                RushDurationTime = 0.5f;
                timer = RushDurationTime;
            }
            RushStartPoint = core.CoreCollisionSenses.UnitCenterPos;
            if (core.Unit.TargetUnit.Core.CoreCollisionSenses.UnitCenterPos.x >= core.CoreCollisionSenses.UnitCenterPos.x)
            {
                RushPoint = core.Unit.TargetUnit.Core.CoreCollisionSenses.UnitCenterPos + new Vector3(1.25f, 0, 0);
            }
            else
            {
                RushPoint = core.Unit.TargetUnit.Core.CoreCollisionSenses.UnitCenterPos + new Vector3(-1.25f, 0, 0);
            }
            FixedGravityOn();
            currentMovementIndex++;
        }

        private void HandleRushOff()
        {
            isFixedRush = false;
            CoreMovement.SetVelocityX(0f);
        }

        private void FixedUpdate()
        {
            if (!isFixedRush)
                return;

            if (timer > 0f)
            {
                float t = Mathf.Clamp01(1 - timer / RushDurationTime);
                core.Unit.RB.MovePosition(Vector2.Lerp(RushStartPoint, RushPoint, t));
                timer -= Time.fixedDeltaTime;
                if (t >= 1f)
                {
                    core.Unit.transform.position = RushPoint;
                    HandleRushOff();
                }
            }
        }

        private void CheckRushAction(ActionMovement actionData)
        {
            if (actionData == null)
                return;

            var currMovement = actionData.movements;
            if (currMovement.Length < currentMovementIndex)
            {
                return;
            }
            RushDurationTime = currMovement[currentMovementIndex].Velocity;
            timer = RushDurationTime;
        }
        #endregion

        #region Immunity

        private void HandleStartInvincible()
        {
            core.Unit.Set_Fixed_Hit_Immunity(true);
            core.Unit.isCC_immunity = true;
        }

        private void HandleStopInvincible()
        {
            core.Unit.Set_Fixed_Hit_Immunity(false);
            core.Unit.isCC_immunity = false;
        }
        private void HandleStartCCImmunity()
        {
            core.Unit.isCC_immunity = true;
        }
        private void HandleStopCCImmunity()
        {
            core.Unit.isCC_immunity = false;
        }

        #endregion

        #region Handle Flip
        private void HandleStartFlip()
        {
            CoreMovement.CanFlip = true;
            if (unit.GetType() != typeof(Player) &&  unit.TargetUnit?.GetType() == typeof(Player))
            {                
                CoreMovement.FlipToTarget();
            }
        }
        private void HandleStopFlip()
        {
            CoreMovement.CanFlip = false;
        }
        #endregion

        public void CheckMovementAction(ActionMovement actionData)
        {
            if (actionData == null)
                return;
            var currMovement = actionData.movements;
            if (currMovement.Length < currentMovementIndex)
            {
                return;
            }
            if (currMovement[currentMovementIndex].Direction.magnitude == 0)
            {
                return;
            }
            if (currMovement[currentMovementIndex].Direction.x != 0 && currMovement[currentMovementIndex].Direction.y == 0)
            {
                CoreMovement.SetVelocityX(currMovement[currentMovementIndex].Velocity * CoreMovement.FancingDirection);
            }
            else if (currMovement[currentMovementIndex].Direction.x == 0 && currMovement[currentMovementIndex].Direction.y != 0)
            {
                CoreMovement.SetVelocityY(currMovement[currentMovementIndex].Velocity);
            }
            else
            {
                CoreMovement.SetVelocity(currMovement[currentMovementIndex].Velocity, currMovement[currentMovementIndex].Direction, CoreMovement.FancingDirection);
            }
        }

        protected override void Start()
        {
            base.Start();

            eventHandler.OnFixedStartMovement -= HandleFixedStartMovement;
            eventHandler.OnFixedStartMovement += HandleFixedStartMovement;

            eventHandler.OnFixedStopMovement -= HandleFixedStopMovement;
            eventHandler.OnFixedStopMovement += HandleFixedStopMovement;

            eventHandler.OnMovementAction -= HandleMovementAction;
            eventHandler.OnMovementAction += HandleMovementAction;

            eventHandler.OnTeleportToTarget -= HandleTeleport;
            eventHandler.OnTeleportToTarget += HandleTeleport;

            eventHandler.OnTeleportToPoint -= HandleToPointTeleport;
            eventHandler.OnTeleportToPoint += HandleToPointTeleport;

            eventHandler.OnRushToTargetOn -= HandleRushOn;
            eventHandler.OnRushToTargetOn += HandleRushOn;

            eventHandler.OnRushToTargetOff -= HandleRushOff;
            eventHandler.OnRushToTargetOff += HandleRushOff;


            eventHandler.OnStartMovement -= HandleStartMovement;
            eventHandler.OnStartMovement += HandleStartMovement;

            eventHandler.OnStopMovement -= HandleStopMovement;
            eventHandler.OnStopMovement += HandleStopMovement;

            eventHandler.OnStartFlip -= HandleStartFlip;
            eventHandler.OnStartFlip += HandleStartFlip;

            eventHandler.OnStopFlip -= HandleStopFlip;
            eventHandler.OnStopFlip += HandleStopFlip;

            eventHandler.OnStartInvincible -= HandleStartInvincible;
            eventHandler.OnStartInvincible += HandleStartInvincible;

            eventHandler.OnStopInvincible -= HandleStopInvincible;
            eventHandler.OnStopInvincible += HandleStopInvincible;

            eventHandler.OnStartCCImmunity -= HandleStartCCImmunity;
            eventHandler.OnStartCCImmunity += HandleStartCCImmunity;

            eventHandler.OnStopCCImmunity -= HandleStopCCImmunity;
            eventHandler.OnStopCCImmunity += HandleStopCCImmunity;

            //애니메이션 종료 시 원래 상태로 돌리기 위함
            eventHandler.OnFinish += HandleStopInvincible;
            eventHandler.OnFinish += HandleStopCCImmunity;
            eventHandler.OnFinish += FixedGravityOff;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            eventHandler.OnFixedStartMovement -= HandleFixedStartMovement;
            eventHandler.OnFixedStopMovement -= HandleFixedStopMovement;
            eventHandler.OnMovementAction -= HandleMovementAction;
            eventHandler.OnTeleportToTarget -= HandleTeleport;
            eventHandler.OnTeleportToPoint -= HandleToPointTeleport;
            eventHandler.OnRushToTargetOn -= HandleRushOn;
            eventHandler.OnRushToTargetOff -= HandleRushOff;
            eventHandler.OnStartMovement -= HandleStartMovement;
            eventHandler.OnStopMovement -= HandleStopMovement;
            eventHandler.OnStartInvincible -= HandleStartInvincible;
            eventHandler.OnStartFlip -= HandleStartFlip;
            eventHandler.OnStopFlip -= HandleStopFlip;
            eventHandler.OnStopInvincible -= HandleStopInvincible;
            eventHandler.OnStartCCImmunity -= HandleStartCCImmunity;
            eventHandler.OnStopCCImmunity -= HandleStopCCImmunity;

            //애니메이션 종료 시 원래 상태로 돌리기 위함
            eventHandler.OnFinish -= HandleStopInvincible;
            eventHandler.OnFinish -= HandleStopCCImmunity;
            eventHandler.OnFinish -= FixedGravityOff;
        }
    }
}
