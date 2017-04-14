using UnityEngine;
using PlayerController.InputController;
using Megaman.Constants;
using System.Collections;

public class CharacterMovementController : MonoBehaviour
{

    private Animator characterAnimator;
    private SpriteRenderer spriteRenderer;

    private int idleFrameThresholdCounter;
    private bool isRunStartPlaying;

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
    }

    void Update()
    {
    }

    private void Run(float value)
    {
        if (value > 0.0f)
        {
            value = 1.0f;
            spriteRenderer.flipX = false;
            idleFrameThresholdCounter = 0;
            characterAnimator.SetBool(AnimatorConditionConstant.RUN, true);
        }
        else if (value < 0.0f)
        {
            value = -1.0f;
            spriteRenderer.flipX = true;
            idleFrameThresholdCounter = 0;
            characterAnimator.SetBool(AnimatorConditionConstant.RUN, true);
        }
        else
        {
            if (idleFrameThresholdCounter == 3)
            {
                characterAnimator.SetBool(AnimatorConditionConstant.RUN, false);
                idleFrameThresholdCounter = 0;
            }
            else
            {
                ++idleFrameThresholdCounter;
            }
        }

        if (isRunStartPlaying)
        {
            value *= (Time.deltaTime / 2.0f);
        }
        else
        {
            value *= Time.deltaTime;
        }
        transform.Translate(value, 0.0f, 0.0f);
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