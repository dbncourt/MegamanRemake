using UnityEngine;
using PlayerController.InputController;

namespace Project.Physics
{
    [RequireComponent(typeof(Animator), typeof(SpriteRenderer), typeof(PlayerInputController))]
    public abstract class PlayerCharacter2D : CharacterMovementController
	{
        protected Animator animator;
        protected SpriteRenderer spriteRenderer;
        protected PlayerInputController playerInputController;

		public PlayerCharacter2D () : base()
		{
		}

		protected new void Start ()
		{
			base.Start ();
			playerInputController = GetComponent<PlayerInputController> ();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();

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