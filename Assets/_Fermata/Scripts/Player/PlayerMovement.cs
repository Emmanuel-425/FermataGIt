using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private int maxJumps = 2;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Camera Support")]
    [SerializeField] private float fallSpeedThreshold = -0.5f;

    [Header("Footstep Sound")]
    [SerializeField] private AudioClip footstepClip;

    [Header("Jump Sounds")]
    [SerializeField] private AudioClip firstJumpClip;
    [SerializeField] private AudioClip doubleJumpClip;

    private Rigidbody2D rb;
    private AudioSource audioSource;

    private float horizontalInput;

    private bool isGrounded;
    private int jumpsRemaining;

    // Base values
    private float baseMoveSpeed;
    private float baseJumpForce;

    // Current values
    private float currentMoveSpeed;
    private float currentJumpForce;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.loop = true;
        audioSource.spatialBlend = 0f;
        audioSource.clip = footstepClip;

        // Store original values
        baseMoveSpeed = moveSpeed;
        baseJumpForce = jumpForce;

        currentMoveSpeed = moveSpeed;
        currentJumpForce = jumpForce;
    }

    private void Start()
    {
        jumpsRemaining = maxJumps;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        CheckGround();

        HandleFacingDirection();

        HandleFallingState();

        HandleWalkingSound();

        if (Input.GetKeyDown(KeyCode.Space) && jumpsRemaining > 0)
        {
            Jump();
        }
    }

    public void SetMoveSpeed(float speed)
    {
        currentMoveSpeed = speed;

        float ratio = currentMoveSpeed / baseMoveSpeed;

        // Scale jump force together with movement speed
        currentJumpForce = baseJumpForce * ratio;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(
            horizontalInput * currentMoveSpeed,
            rb.linearVelocity.y
        );
    }

    private void HandleFacingDirection()
    {
        // Rotate on Y-axis so CameraFollowTarget knows which direction we face
        if (horizontalInput > 0)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (horizontalInput < 0)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }

    private void HandleFallingState()
    {
        // Notify CameraManager when falling downwards in the air
        bool isFalling = rb.linearVelocity.y < fallSpeedThreshold && !isGrounded;

        if (CameraManager.instance != null)
        {
            CameraManager.instance.SetFalling(isFalling);
        }
    }

    private void Jump()
    {
        bool isFirstJump = jumpsRemaining == maxJumps;

        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            currentJumpForce
        );

        jumpsRemaining--;

        if (isFirstJump)
        {
            PlaySound(firstJumpClip);
        }
        else
        {
            PlaySound(doubleJumpClip);
        }
    }

    private void HandleWalkingSound()
    {
        bool isMoving =
            Mathf.Abs(horizontalInput) > 0.01f &&
            isGrounded;

        if (isMoving)
        {
            if (!audioSource.isPlaying && footstepClip != null)
            {
                audioSource.clip = footstepClip;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip == null)
            return;

        AudioSource.PlayClipAtPoint(
            clip,
            transform.position
        );
    }

    private void CheckGround()
    {
        bool wasGrounded = isGrounded;

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        if (!wasGrounded && isGrounded)
        {
            jumpsRemaining = maxJumps;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(
            groundCheck.position,
            groundCheckRadius
        );
    }
}