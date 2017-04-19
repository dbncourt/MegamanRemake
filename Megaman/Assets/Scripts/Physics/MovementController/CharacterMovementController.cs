using UnityEngine;

namespace Project.Physics
{
    public class CharacterMovementController : PhysicsController2D
    {
        protected struct CharacterAction
        {
            public bool isJumpDown { get; set; }
            public bool isJumpPressed { get; set; }
            public bool isWallSliding { get; set; }
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

        private Vector3 velocity;

        protected Vector2 input;
        protected CharacterAction characterStatus;

        public CharacterMovementController() : base()
        {
            jumpHeight = 1.0f;
            timeToJumpApex = 0.4f;
            accelerationTimeAirbone = 0.2f;
            accelerationTimeGrounded = 0.1f;
            moveSpeed = 6.0f;
            wallSlideSpeedMax = 3.0f;
            wallStickTime = 0.25f;
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

            float targetVelocityX = input.x * moveSpeed;
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (collisionInfo.below ? accelerationTimeGrounded : accelerationTimeAirbone));

            characterStatus.isWallSliding = false;
            if ((collisionInfo.left || collisionInfo.right) && !collisionInfo.below && velocity.y < 0.0f)
            {
                characterStatus.isWallSliding = true;
                if (velocity.y < -wallSlideSpeedMax)
                {
                    velocity.y = -wallSlideSpeedMax;
                }

                if (timeToWallUnstick > 0.0f)
                {
                    velocityXSmoothing = 0.0f;
                    velocity.x = 0.0f;

                    if (input.x != wallDirectionX && input.x != 0.0f)
                    {
                        timeToWallUnstick -= Time.deltaTime;
                    }
                    else
                    {
                        timeToWallUnstick = wallStickTime;
                    }
                }
            }

            if (collisionInfo.above || collisionInfo.below)
            {
                velocity.y = 0.0f;
            }

            if (characterStatus.isJumpDown)
            {
                characterStatus.isJumpDown = false;
                if (characterStatus.isWallSliding)
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
            else if (characterStatus.isJumpPressed && timeJumping <= maxTimeJump)
            {
                velocity.y = jumpVelocity;
                timeJumping += Time.deltaTime;
            }
            velocity.y += gravity * Time.deltaTime;
            Move(velocity * Time.deltaTime);
        }

        protected void Update()
        {

        }

        protected virtual void OnLanded()
        {
        }

        protected void MoveHorizontally(float value)
        {
            input.x = value;
            input.y = 0.0f;
        }

        protected void Jump()
        {
            characterStatus.isJumpDown = true;
            if (collisionInfo.below)
            {
                timeJumping = 0.0f;
            }
        }

        protected void JumpPressed()
        {
            characterStatus.isJumpPressed = true;
        }

        protected void StopJumping()
        {
            characterStatus.isJumpPressed = false;
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
    }
}