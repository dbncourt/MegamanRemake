using System;
using PlayerController.InputController;
using Project.Physics;
using UnityEngine;

[RequireComponent(typeof(PlayerInputController))]
public class PlayerCharacterMovementController : CharacterMovementController
{
    private new void Start()
    {
        base.Start();
        SetupInputController();
    }

    private new void Update()
    {
        base.Update();
    }

    private new void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void OnFalling()
    {
        throw new NotImplementedException();
    }

    private void SetupInputController()
    {
        PlayerInputController playerInputController = GetComponent<PlayerInputController>();
        playerInputController.BindAxis(AxisInputConstants.HORIZONTAL, MoveHorizontally);
        playerInputController.BindAction(ActionInputConstants.JUMP, Jump, Key.EKeyStatus.Down);
        playerInputController.BindAction(ActionInputConstants.JUMP, JumpPressed, Key.EKeyStatus.Pressed);
        playerInputController.BindAction(ActionInputConstants.JUMP, StopJumping, Key.EKeyStatus.Up);
    }
}