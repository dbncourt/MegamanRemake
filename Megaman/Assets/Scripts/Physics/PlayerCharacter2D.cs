using UnityEngine;
using PlayerController.InputController;

namespace Project.Physics
{
	[RequireComponent (typeof(PlayerInputController))]
	public abstract class PlayerCharacter2D : CharacterMovementController
	{
		public PlayerCharacter2D ()
		{
		}

		protected new void Start ()
		{
			base.Start ();
			PlayerInputController playerInputController = GetComponent<PlayerInputController> ();
			SetupPlayerInputController (ref playerInputController);
		}

		protected new void Update ()
		{
			base.Update ();
		}

		protected new void FixedUpdate ()
		{
			base.FixedUpdate ();
		}

		protected abstract void SetupPlayerInputController (ref PlayerInputController playerInputController);
	}
}