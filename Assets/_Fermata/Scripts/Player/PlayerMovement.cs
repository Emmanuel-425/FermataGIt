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
    }

    private void Start()
    {
        jumpsRemaining = maxJumps;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        CheckGround();

        HandleWalkingSound();

        if (Input.GetKeyDown(KeyCode.Space) && jumpsRemaining > 0)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(
            horizontalInput * moveSpeed,
            rb.linearVelocity.y
        );
    }

    private void Jump()
    {
        bool isFirstJump = jumpsRemaining == maxJumps;

        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            jumpForce
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