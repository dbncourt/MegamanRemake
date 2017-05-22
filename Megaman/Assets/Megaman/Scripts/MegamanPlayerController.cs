using PlayerController.InputController;
using Project.Physics;
using Megaman.Constants;
using UnityEngine;

public class MegamanPlayerController : PlayerCharacter2D
{
    [SerializeField]
    private float dashSpeed;
    private bool isCrouching;
    private bool isDashing;
    private bool isWalking;

    public MegamanPlayerController() : base()
    {
        IsSpawning = true;
        isCrouching = false;
        isDashing = false;
        isWalking = false;
    }

    private new void Start()
    {
        base.Start();
        if (IsSpawning)
        {
            animator.speed = 0.0f;
            playerInputController.enabled = false;
        }
    }

    private new void Update()
    {
        base.Update();
        CheckMegamanIsGrounded();
    }

    private void CheckMegamanIsGrounded()
    {
        animator.SetBool(AnimatorConditionConstant.IS_GROUNDED, IsCharacterLanded());
    }

    private void CheckMegamanDashingInterrupted(float value)
    {
        bool isDirectionChanged = spriteRenderer.flipX && value > 0.0f || !spriteRenderer.flipX && value < 0.0f;
        if (IsDashing && isDirectionChanged)
        {
            IsDashing = false;
        }
    }

    private new void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void SetupPlayerInputController(ref PlayerInputController playerInputController)
    {
        playerInputController.BindAxis(AxisInputConstants.HORIZONTAL, MoveHorizontally);
        playerInputController.BindAxis(AxisInputConstants.VERTICAL, MoveVertically);

        playerInputController.BindAction(ActionInputConstants.JUMP, Jump, Key.EKeyStatus.Down);
        playerInputController.BindAction(ActionInputConstants.JUMP, JumpPressed, Key.EKeyStatus.Pressed);
        playerInputController.BindAction(ActionInputConstants.JUMP, StopJumping, Key.EKeyStatus.Up);

        playerInputController.BindAction(ActionInputConstants.DASH, Dash, Key.EKeyStatus.Down);
        playerInputController.BindAction(ActionInputConstants.DASH, StopDashing, Key.EKeyStatus.Up);
    }

    private void MoveVertically(float value)
    {
        IsCrouching = value < 0.0f;
    }

    protected override void MoveHorizontally(float value)
    {
        isWalking = value != 0.0f;
        CheckMegamanDashingInterrupted(value);
        FlipSprite(value);
        if (IsDashing)
        {
            value = spriteRenderer.flipX ? -1.0f : 1.0f;
        }
        else if (IsCharacterLanded() && IsCrouching)
        {
            value = 0.0f;
        }

        animator.SetFloat(AnimatorConditionConstant.HORIZONTAL_SPEED, Mathf.Abs(value));
        base.MoveHorizontally(value);
    }

    private void FlipSprite(float value)
    {
        if (value > 0.0f)
        {
            spriteRenderer.flipX = false;
        }
        else if (value < 0.0f)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void NotifySpawnEnd()
    {
        playerInputController.enabled = true;
        IsSpawning = false;
    }

    protected override void Jump()
    {
        base.Jump();
        if (IsCharacterLanded())
        {
            animator.SetTrigger(AnimatorConditionConstant.JUMP);
        }
        IsDashing = false;
    }

    private void Dash()
    {
        if (!IsDashing && IsCharacterLanded())
        {
            IsDashing = true;
        }
    }

    private void StopDashing()
    {
        if (!isWalking)
        {
            IsDashing = false;
        }
    }

    private void NotifyDashEnd()
    {
        IsDashing = false;
    }

    protected override void OnLanded()
    {
        base.OnLanded();

        if (IsSpawning)
        {
            animator.speed = 1.0f;
        }
        animator.SetBool(AnimatorConditionConstant.IS_GROUNDED, true);
    }

    protected override void OnFalling()
    {
        base.OnFalling();
        if (!IsSpawning)
        {
            animator.SetTrigger(AnimatorConditionConstant.FALL);
        }
        if (IsDashing)
        {
            Speed = 0.0f;
            IsDashing = false;
        }
    }

    private bool IsCrouching
    {
        get
        {
            return isCrouching;
        }
        set
        {
            isCrouching = value;
            animator.SetBool(AnimatorConditionConstant.CROUCH, isCrouching);
        }
    }

    private bool IsDashing
    {
        get
        {
            return isDashing;
        }

        set
        {
            if (value)
            {
                Speed = dashSpeed;
            }
            else
            {
                Speed = MoveSpeed;
            }
            isDashing = value;
            animator.SetBool(AnimatorConditionConstant.DASH, isDashing);
        }
    }

    private bool IsSpawning { get; set; }

    private void NotifyDashToIdleStopStart()
    {
        IsDashing = false;
    }
}