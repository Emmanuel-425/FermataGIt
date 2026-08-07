using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatformDown : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float targetY = -5f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float startDelay = 0.5f;

    private Rigidbody2D rb;

    private bool activated;
    private bool moving;
    private float delayTimer;
    private Vector2 startPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        startPosition = rb.position;
    }

    private void FixedUpdate()
    {
        if (!activated)
            return;

        if (!moving)
        {
            delayTimer += Time.fixedDeltaTime;

            if (delayTimer >= startDelay)
                moving = true;

            return;
        }

        Vector2 target = new Vector2(
            rb.position.x,
            targetY
        );

        rb.MovePosition(
            Vector2.MoveTowards(
                rb.position,
                target,
                moveSpeed * Time.fixedDeltaTime
            )
        );
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        activated = true;
    }

    public void Reset()
    {
        activated = false;
        moving = false;
        delayTimer = 0f;
        rb.MovePosition(startPosition);
    }
}