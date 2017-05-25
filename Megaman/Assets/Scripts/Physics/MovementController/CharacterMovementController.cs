using System;
using UnityEngine;

namespace Project.Physics
{
    public class CharacterMovementController : PhysicsController2D
    {
        private struct CharacterAction
        {
            public bool IsJumpDown { get; set; }
            public bool IsJumpPressed { get; set; }
            public bool IsWallSliding { get; set; }
            public bool IsFalling { get; set; }
            public bool IsLanded { get; set; }
        }

        [SerializeField]
        private float jumpHeight;
        [SerializeField]
        private float timeToJumpApex;
        [SerializeField]
        private float accelerationTimeAirbone;
        [SerializeField]
        private float accelerationTimeGrounded;
        [SerializeField]
        private float speed;
        [SerializeField]
        private float moveSpeed;
        [SerializeField]
        private float wallSlideSpeedMax;
        [SerializeField]
        private float wallStickTime;
        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float maxTimeJump;
        [SerializeField]
        private Vector2 wallJumpClimb;
        [SerializeField]
        private Vector2 wallJumpOff;
        [SerializeField]
        private Vector2 wallLeap;

        private float timeJumping;
        private float timeToWallUnstick;
        private float gravity;
        private float jumpVelocity;
        private float velocityXSmoothing;
        private bool isFalling;
        private CollisionInfo previousFrameCollisionInfo;

        private Vector3 velocity;

        private Vector2 input;
        private CharacterAction characterStatus;

        public float Speed
        {
            get
            {
                return speed;
            }

            set
            {
                speed = value;
            }
        }

        public float MoveSpeed
        {
            get
            {
                return moveSpeed;
            }

            set
            {
                moveSpeed = value;
            }
        }

        public CharacterMovementController() : base()
        {
            jumpHeight = 1.0f;
            timeToJumpApex = 0.4f;
            accelerationTimeAirbone = 0.2f;
            accelerationTimeGrounded = 0.1f;
            moveSpeed = 6.0f;
            speed = moveSpeed;
            wallSlideSpeedMax = 3.0f;
            wallStickTime = 0.0f;
            timeToWallUnstick = wallStickTime;
            maxTimeJump = 0.2f;
            timeJumping = 0.0f;
            wallJumpClimb = new Vector2(7.5f, 16.0f);
            wallJumpOff = new Vector2(8.5f, 7.0f);
            wallLeap = new Vector2(18.0f, 17.0f);
            maxTimeJump = 0.2f;
        }

        protected new void Start()
        {
            base.Start();

            gravity = CalculateGravity(jumpHeight, timeToJumpApex);
            jumpVelocity = CalculateJumpVelocity(gravity, timeToJumpApex);
        }

        protected void FixedUpdate()
        {
            int wallDirectionX = (collisionInfo.left) ? -1 : 1;

            float targetVelocityX = input.x * speed;
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (collisionInfo.below ? accelerationTimeGrounded : accelerationTimeAirbone));

            characterStatus.IsWallSliding = false;
            if ((collisionInfo.left || collisionInfo.right) && !collisionInfo.below && velocity.y < 0.0f)
            {
                characterStatus.IsWallSliding = true;
                if (velocity.y < -wallSlideSpeedMax)
                {
                    velocity.y = -wallSlideSpeedMax;
                }

                int inputDirection = input.x > 0.0f ? 1 : -1;

                if (inputDirection != wallDirectionX && input.x != 0.0f)
                {
                    velocity.x = -wallDirectionX * wallLeap.x;
                    velocity.y = wallLeap.y;
                }
            }

            if (collisionInfo.above || collisionInfo.below)
            {
                velocity.y = 0.0f;
            }

            if (characterStatus.IsJumpDown)
            {
                characterStatus.IsJumpDown = false;
                if (characterStatus.IsWallSliding)
                {
                    if (wallDirectionX == input.x)
                    {
                        velocity.x = -wallDirectionX * wallJumpClimb.x;
                        velocity.y = wallJumpClimb.y;
                    }
                    else if (input.x == 0.0f)
                    {
                        velocity.x = -wallDirectionX * wallJumpOff.x;
                        velocity.y = wallJumpOff.x;
                    }
                    else
                    {
                        velocity.x = -wallDirectionX * wallLeap.x;
                        velocity.y = wallLeap.y;
                    }

                    if (collisionInfo.below || timeJumping <= maxTimeJump)
                    {
                        velocity.y = jumpVelocity;
                        timeJumping += Time.deltaTime;
                    }
                }
            }
            else if (characterStatus.IsJumpPressed && timeJumping <= maxTimeJump)
            {
                velocity.y = jumpVelocity;
                timeJumping += Time.deltaTime;
            }
            if (!isFalling && velocity.y < -0.45f)
            {
                isFalling = true;
                OnFalling();
            }
            TriggerCollisionStateChangesEvents();
            velocity.y += gravity * Time.deltaTime;
            Move(velocity * Time.deltaTime);
        }

        private void TriggerCollisionStateChangesEvents()
        {
            if (!previousFrameCollisionInfo.below && collisionInfo.below)
            {
                isFalling = false;
                BelowCollisionEnter();
            }
            else if (!collisionInfo.below && previousFrameCollisionInfo.below)
            {
                BelowCollisionExit();
            }

            if (!previousFrameCollisionInfo.above && collisionInfo.above)
            {
                AboveCollisionEnter();
            }
            else if (!collisionInfo.above && previousFrameCollisionInfo.above)
            {
                AboveCollisionExit();
            }
            if (!previousFrameCollisionInfo.right && collisionInfo.right)
            {
                RightCollisionEnter();
            }
            else if (!collisionInfo.right && previousFrameCollisionInfo.right)
            {
                RightCollisionExit();
            }
            if (!previousFrameCollisionInfo.left && collisionInfo.left)
            {
                LeftCollisionEnter();
            }
            else if (!collisionInfo.left && previousFrameCollisionInfo.left)
            {
                LeftCollisionExit();
            }
            SetPreviousFrameValues();
        }

        private void SetPreviousFrameValues()
        {
            previousFrameCollisionInfo = collisionInfo;
        }

        protected void Update()
        {
        }

        protected virtual void BelowCollisionEnter()
        {
        }

        protected virtual void BelowCollisionExit()
        {
        }

        protected virtual void AboveCollisionEnter()
        {
        }

        protected virtual void AboveCollisionExit()
        {
        }

        protected virtual void RightCollisionEnter()
        {
        }

        protected virtual void RightCollisionExit()
        {
        }

        protected virtual void LeftCollisionEnter()
        {
        }

        protected virtual void LeftCollisionExit()
        {
        }

        protected virtual void MoveHorizontally(float value)
        {
            input.x = value;
            input.y = 0.0f;
        }

        protected virtual void Jump()
        {
            characterStatus.IsJumpDown = true;
            if (collisionInfo.below)
            {
                timeJumping = 0.0f;
            }
        }

        protected virtual void JumpPressed()
        {
            characterStatus.IsJumpPressed = true;
        }

        protected virtual void StopJumping()
        {
            characterStatus.IsJumpPressed = false;
            timeJumping = float.MaxValue;
        }

        protected virtual void OnFalling()
        {
        }

        private static float CalculateGravity(float jumpHeight, float timeToJumpApex)
        {
            return -(2 * jumpHeight) / (timeToJumpApex * timeToJumpApex);
        }

        private static float CalculateJumpVelocity(float gravity, float timeToJumpApex)
        {
            return Mathf.Abs(gravity) * timeToJumpApex;
        }

        public bool IsCharacterLanded()
        {
            return collisionInfo.below;
        }

        public bool IsCharacterFalling()
        {
            return isFalling;
        }
    }
}