using UnityEngine;
using PlayerController.InputController;
using Megaman.Constants;
using System.Collections;

public class CharacterMovementController : MonoBehaviour
{
    [Range(1.0f, 5.0f)]
    public float maxSpeed;
    public bool facingRight;

    private Animator characterAnimator;
    private SpriteRenderer spriteRenderer;
    private new Rigidbody2D rigidbody2D;

    private int idleFrameThresholdCounter;
    private bool isRunStartPlaying;

    public CharacterMovementController()
    {
        maxSpeed = 10.0f;
        facingRight = true;
    }

    void Start()
    {
        idleFrameThresholdCounter = 0;

        InputPlayerController playerController = GetComponent<InputPlayerController>();
        if (playerController != null)
        {
            playerController.BindAxis(AxisInputConstants.HORIZONTAL, Run);
        }
        else
        {
            Debug.LogError("InputPlayerController not found");
        }

        characterAnimator = GetComponent<Animator>();
        if (characterAnimator == null)
        {
            Debug.LogError("Animator not found");
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found");
        }

        rigidbody2D = GetComponent<Rigidbody2D>();
        if (rigidbody2D == null)
        {
            Debug.LogError("Rigidbody2D not found");
        }
    }

    void Update()
    {
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

        characterAnimator.SetFloat(AnimatorConditionConstant.SPEED, Mathf.Abs(value));
    }

    private void NotifyRunStartFinished()
    {
        isRunStartPlaying = false;
    }

    private void NotifyRunStartStarted()
    {
        isRunStartPlaying = true;
    }
}