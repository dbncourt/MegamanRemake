using UnityEngine;
using PlayerController.InputController;

public abstract class CharacterMovementController : MonoBehaviour
{
    [Range(1.0f, 5.0f)]
    public float maxSpeed;

    [Range(1.0f, 50.0f)]
    public float jumpSpeed;

    [Range(0.1f, 1.0f)]
    public float timeJump;

    public LayerMask whatIsGround;
    protected new Rigidbody2D rigidbody2D;

    private float verticalSpeed;
    public bool isGrounded;
    protected bool IsGrounded
    {
        get { return isGrounded; }
    }
    private float airTimeCounter;
    private bool isJumping;

    public CharacterMovementController()
    {
        airTimeCounter = 0.0f;
        isJumping = false;
        verticalSpeed = -0.1f;
    }

    protected void Start()
    {
        PlayerInputController playerInputController = GetComponent<PlayerInputController>();
        if(playerInputController != null)
        {
            SetupInputController(playerInputController);
        } else
        {
            Debug.LogError("PlayerInputController not found");
        }
        
        rigidbody2D = GetComponent<Rigidbody2D>();
        if (rigidbody2D == null)
        {
            Debug.LogError("Rigidbody2D not found");
        }
    }

    protected void FixedUpdate()
    {
        Vector2 characterPosition = transform.position;
        characterPosition.y -= 0.025f;
        bool tmpGrounded = Physics2D.OverlapBox(characterPosition, new Vector2(0.4f, 0.008f), 0.0f, whatIsGround);
        if (!isGrounded && tmpGrounded)
        {
            OnLanded();
        }
        isGrounded = tmpGrounded;

        if (isJumping && airTimeCounter < timeJump)
        {
            airTimeCounter += Time.deltaTime;
            rigidbody2D.AddForce(new Vector2(0.0f, jumpSpeed));
        }

        if(rigidbody2D.velocity.y < 0.0f && verticalSpeed >= 0.0f)
        {
            OnFalling();
        }
        verticalSpeed = rigidbody2D.velocity.y;
    }

    protected void Update()
    {
    }

    protected void OnLanded()
    {
        isJumping = false;
    }

    protected void Jump()
    {
        if (isGrounded)
        {
            isJumping = true;
        }
    }

    protected void StopJumping()
    {
        isJumping = false;
        airTimeCounter = 0.0f;
    }

    abstract protected void OnFalling();
    abstract protected void SetupInputController(PlayerInputController inputController);
}