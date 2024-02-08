using UnityEngine;

namespace SCOM.CoreSystem
{
    public class Movement : CoreComponent
    {
        public Rigidbody2D RB { get; private set; }

        public int FancingDirection
        {
            get
            {
                return fancingDirection;
            }
            private set { fancingDirection = value; }
        }
        private int fancingDirection = 1;
        public bool CanSetVelocity { get; set; }
        public Vector2 CurrentVelocity { get; private set; }
        public Vector2 FixedVelocity { get; private set; }

        [HideInInspector] public bool CanFlip = false;
        [HideInInspector] public bool CanMovement = false;

        protected Vector2 workspace;

        protected override void Awake()
        {
            base.Awake();
            RB = GetComponentInParent<Rigidbody2D>();

            FancingDirection = 1;
            CanSetVelocity = true;
        }

        public override void LogicUpdate()
        {
            CurrentVelocity = RB.velocity;
        }

        #region Set Func
        public void SetVelocityZero()
        {
            workspace = Vector2.zero;
            SetFinalVelocity();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="velocity">움직임 크기</param>
        /// <param name="angle">캐릭터가 움직일 각도</param>
        /// <param name="direction">캐릭터가 향하는 방향</param>
        public void SetVelocity(float velocity, Vector2 angle, int direction)
        {
            angle.Normalize();
            workspace.Set(angle.x * velocity * direction, angle.y * velocity);
            SetFinalVelocity();
        }
        public void AddVelocity(Vector2 velocity)
        {
            workspace.Set(CurrentVelocity.x + velocity.x * FancingDirection, CurrentVelocity.y + velocity.y);
            SetFinalVelocity();
        }
        public void SetVelocityX(float velocity)
        {
            workspace.Set(velocity, CurrentVelocity.y);
            SetFinalVelocity();
        }
        public void SetVelocityY(float velocity)
        {
            workspace.Set(CurrentVelocity.x, velocity);
            SetFinalVelocity();
        }
        private void SetFinalVelocity()
        {
            if (CanSetVelocity)
            {
                RB.velocity = workspace;
                CurrentVelocity = workspace;
            }
        }
        #endregion Set Func

        #region Flip
        public void CheckIfShouldFlip(int xInput)
        {
            if (xInput != 0 && xInput != FancingDirection)
            {
                Flip();
            }
        }

        //2D Filp
        public void Flip()
        {
            FancingDirection *= -1;
            RB.transform.Rotate(0.0f, 180.0f, 0.0f);
        }

        public void FlipToTarget()
        {
            if (core.Unit.TargetUnit == null)
                return;

            if ((core.Unit.TargetUnit.Core.CoreCollisionSenses.UnitCenterPos.x - core.CoreCollisionSenses.UnitCenterPos.x > 0) && (FancingDirection == -1) ||
                    (core.Unit.TargetUnit.Core.CoreCollisionSenses.UnitCenterPos.x - core.CoreCollisionSenses.UnitCenterPos.x < 0) && (FancingDirection == 1))
            {
                Flip();
            }
        }
        #endregion Flip
    }
}