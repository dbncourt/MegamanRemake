using System;
using PlayerController.InputController;
using Project.Physics;
using UnityEngine;

public class PlayerCharacterMovementController : PlayerCharacter2D
{
	private new void Start ()
	{
		base.Start ();
	}

	private new void Update ()
	{
		base.Update ();
	}

	private new void FixedUpdate ()
	{
		base.FixedUpdate ();
	}

	protected override void OnFalling ()
	{
		throw new NotImplementedException ();
	}

	protected override void SetupPlayerInputController (ref PlayerInputController playerInputController)
	{
		playerInputController.BindAxis (AxisInputConstants.HORIZONTAL, MoveHorizontally);
		playerInputController.BindAction (ActionInputConstants.JUMP, Jump, Key.EKeyStatus.Down);
		playerInputController.BindAction (ActionInputConstants.JUMP, JumpPressed, Key.EKeyStatus.Pressed);
		playerInputController.BindAction (ActionInputConstants.JUMP, StopJumping, Key.EKeyStatus.Up);
	}

	public void NotifySpawnEnd ()
	{
	}
}