using UnityEngine;

namespace SCOM.CoreSystem
{
    public class KnockBackReceiver : CoreComponent, IKnockBackable
    {
        /// <summary>
        /// true = 넉백 중인 상태
        /// </summary>
        private bool isKnockBackActive;
        private float knockBackStartTime;

        private CoreComp<Movement> movement;
        private CoreComp<CollisionSenses> collisionSenses;
        private CoreComp<Death> death;
        private CoreComp<DamageReceiver> damageReceiver;

        public override void LogicUpdate()
        {
            CheckKnockBack();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="angle">각도</param>
        /// <param name="strength">세기</param>
        /// <param name="direction">방향</param>
        public void KnockBack(Vector2 angle, float strength, int direction)
        {

            if (death.Comp.isDead)
            {
                Debug.Log(core.Unit.name + "is Dead");
                return;
            }

            //CC기 면역
            if (core.Unit.isCC_immunity)
            {
                return;
            }
            SetKnockBack(angle, strength, direction);
        }
        public void TrapKnockBack(Vector2 angle, float strength, bool isUnitFancingDirection = true)
        {

            if (death.Comp.isDead)
            {
                Debug.Log(core.Unit.name + "is Dead");
                return;
            }
            if (damageReceiver.Comp.isTouch)
            {
                return;
            }
            if (isUnitFancingDirection)
            {
                SetKnockBack(angle, strength, movement.Comp.FancingDirection);
            }
            else
            {
                SetKnockBack(angle, strength, 1);
            }
        }

        private void SetKnockBack(Vector2 angle, float strength, int direction)
        {
            if (isKnockBackActive)
                return;

            movement.Comp?.SetVelocity(strength, angle, direction);
            movement.Comp.CanSetVelocity = false;
            core.Unit.isFixedMovement = true;
            isKnockBackActive = true;
            knockBackStartTime = Time.time;
        }
        private void CheckKnockBack()
        {
            if (!isKnockBackActive)
                return;

            if(movement.Comp?.CurrentVelocity.y <= 0.01f && (collisionSenses.Comp.CheckIfGrounded))
            {
                movement.Comp.CanSetVelocity = true;
            }
            if (Time.time >= knockBackStartTime + core.Unit.UnitData.touchDamageinvincibleTime)
            {
                isKnockBackActive = false;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            movement = new CoreComp<Movement>(core);
            collisionSenses = new CoreComp<CollisionSenses>(core);
            death = new CoreComp<Death>(core);
            damageReceiver = new CoreComp<DamageReceiver>(core);
        }
    }
}