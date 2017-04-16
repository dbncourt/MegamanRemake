using System;
using System.Collections;
using System.Collections.Generic;
using PlayerController.InputController;
using Megaman.Constants;
using UnityEngine;

public class MegamanCharacterMovementController : CharacterMovementController
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool isCrouch;
    private bool isDash;

    public MegamanCharacterMovementController() : base()
    {
    }

    private new void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator not found");
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found");
        }
    }

    private new void Update()
    {
        base.Update();
        animator.SetBool(AnimatorConditionConstant.IS_GROUNDED, IsGrounded);
    }

    private new void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void OnFalling()
    {
        animator.SetTrigger(AnimatorConditionConstant.FALL);
    }

    protected override void SetupInputController(PlayerInputController playerInputController)
    {
        playerInputController.BindAxis(AxisInputConstants.HORIZONTAL, Run);
        playerInputController.BindAxis(AxisInputConstants.VERTICAL, Crouch);
        playerInputController.BindAction(ActionInputConstants.JUMP, JumpDown, PlayerInputController.KeyStatus.Down);
        playerInputController.BindAction(ActionInputConstants.JUMP, JumpPressed, PlayerInputController.KeyStatus.Pressed);
        playerInputController.BindAction(ActionInputConstants.JUMP, JumpUp, PlayerInputController.KeyStatus.Up);
        playerInputController.BindAction(ActionInputConstants.DASH, Dash, PlayerInputController.KeyStatus.Down);
    }

    private void JumpDown()
    {
        base.Jump();
        if (IsGrounded)
        {
            animator.SetTrigger(AnimatorConditionConstant.JUMP);
        }
    }

    private void JumpPressed()
    {
        base.Jump();
    }

    private void JumpUp()
    {
        base.StopJumping();
    }

    private void Run(float value)
    {
        if (isDash)
        {
            float multiplier = spriteRenderer.flipX ? -1.0f : 1.0f;
            rigidbody2D.velocity = new Vector2(multiplier * 2.0f * maxSpeed, rigidbody2D.velocity.y);
        }
        else if (!isCrouch || !IsGrounded)
        {
            rigidbody2D.velocity = new Vector2(value * maxSpeed, rigidbody2D.velocity.y);
            animator.SetFloat(AnimatorConditionConstant.HORIZONTAL_SPEED, Mathf.Abs(value));
        }
        else
        {
            rigidbody2D.velocity = new Vector2(0.0f, rigidbody2D.velocity.y);
            animator.SetFloat(AnimatorConditionConstant.HORIZONTAL_SPEED, Mathf.Abs(0.0f));
        }

        FlipSprite(value);
    }

    private void Crouch(float value)
    {
        if (value < 0.0f)
        {
            isCrouch = true;
            animator.SetBool(AnimatorConditionConstant.CROUCH, true);
        }
        else
        {
            isCrouch = false;
            animator.SetBool(AnimatorConditionConstant.CROUCH, false);
        }
    }

    private void FlipSprite(float value)
    {
        if (value > 0.0f && spriteRenderer.flipX)
        {
            spriteRenderer.flipX = false;
        }
        else if (value < 0.0f && !spriteRenderer.flipX)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void Dash()
    {
        if (isGrounded)
        {
            isDash = true;
            animator.SetTrigger(AnimatorConditionConstant.DASH);
        }
    }

    public void NotifyDashEnd()
    {
        isDash = false;
    }
}
