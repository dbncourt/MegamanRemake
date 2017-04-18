using UnityEngine;
using PlayerController.InputController;

namespace Project.Physics
{
    [RequireComponent(typeof(PhysicsController2D))]
    public abstract class CharacterMovementController : PhysicsController2D
    {
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

        private float timeJumping;
        private float timeToWallUnstick;
        private float gravity;
        private float jumpVelocity;
        private float velocityXSmoothing;

        private Vector3 velocity;
        private Vector2 wallJumpClimb;
        private Vector2 wallJumpOff;
        private Vector2 wallLeap;

        protected Vector2 input;

        public CharacterMovementController()
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
        }

        protected void Start()
        {
            gravity = CalculateGravity(jumpHeight, timeToJumpApex);
            jumpVelocity = CalculateJumpVelocity(gravity, timeToJumpApex);
        }

        protected void FixedUpdate() { }

        protected void Update()
        {
            
        }

        protected virtual void OnLanded() { }

        protected void Jump() { }

        protected void StopJumping()
        {
        }

        abstract protected void OnFalling();
        abstract protected void SetupInputController(PlayerInputController inputController);

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