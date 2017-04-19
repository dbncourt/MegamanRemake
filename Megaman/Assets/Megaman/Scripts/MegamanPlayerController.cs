using PlayerController.InputController;
using Project.Physics;
using Megaman.Constants;

public class MegamanPlayerController : PlayerCharacter2D
{
    private bool isCrouching;
    private bool isDashing;
    private bool isSpawning;

    public MegamanPlayerController() : base()
    {
        isSpawning = false;
    }

    private new void Start()
    {
        base.Start();
        if (isSpawning)
        {
            animator.speed = 0.0f;
            playerInputController.enabled = false;
        }
    }

    private new void Update()
    {
        base.Update();
    }

    private new void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void SetupPlayerInputController(ref PlayerInputController playerInputController)
    {
        playerInputController.BindAxis(AxisInputConstants.HORIZONTAL, MoveHorizontally);

        playerInputController.BindAction(ActionInputConstants.JUMP, Jump, Key.EKeyStatus.Down);
        playerInputController.BindAction(ActionInputConstants.JUMP, JumpPressed, Key.EKeyStatus.Pressed);
        playerInputController.BindAction(ActionInputConstants.JUMP, StopJumping, Key.EKeyStatus.Up);
    }

    private void NotifySpawnEnd()
    {

    }
}