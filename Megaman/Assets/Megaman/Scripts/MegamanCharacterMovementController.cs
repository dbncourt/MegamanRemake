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

    public MegamanCharacterMovementController() : base()
    {
    }

    // Use this for initialization
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

    protected override void OnFalling()
    {
        animator.SetTrigger(AnimatorConditionConstant.FALL);
    }

    protected override void SetupInputController(PlayerInputController playerInputController)
    {
        playerInputController.BindAxis(AxisInputConstants.HORIZONTAL, Run);
        playerInputController.BindAction(ActionInputConstants.JUMP, JumpDown, PlayerInputController.KeyStatus.Down);
        playerInputController.BindAction(ActionInputConstants.JUMP, JumpPressed, PlayerInputController.KeyStatus.Pressed);
        playerInputController.BindAction(ActionInputConstants.JUMP, JumpUp, PlayerInputController.KeyStatus.Up);
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
        rigidbody2D.velocity = new Vector2(value * maxSpeed, rigidbody2D.velocity.y);
        if (value > 0.0f && spriteRenderer.flipX)
        {
            spriteRenderer.flipX = false;
        }
        else if (value < 0.0f && !spriteRenderer.flipX)
        {
            spriteRenderer.flipX = true;
        }

        animator.SetFloat(AnimatorConditionConstant.HORIZONTAL_SPEED, Mathf.Abs(value));
    }
}
