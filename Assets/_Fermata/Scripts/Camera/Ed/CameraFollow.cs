using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Follow")]
    public float smoothTime = 0.25f;

    private Vector3 velocity;

    [Header("Look Ahead")]
    public float lookAheadDistance = 3f;

    public float lookAheadSmooth = 5f;

    public float returnSpeed = 2f;

    private float currentLookAhead;

    [Header("Vertical")]
    public bool followY = true;

    public float yOffset = 1f;

    private float lastX;

    void Start()
    {
        lastX = target.position.x;
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        float moveAmount = target.position.x - lastX;

        if (Mathf.Abs(moveAmount) > 0.01f)
        {
            currentLookAhead = Mathf.Lerp(
                currentLookAhead,
                Mathf.Sign(moveAmount) * lookAheadDistance,
                lookAheadSmooth * Time.deltaTime
            );
        }
        else
        {
            currentLookAhead = Mathf.Lerp(
                currentLookAhead,
                0,
                returnSpeed * Time.deltaTime
            );
        }

        Vector3 desiredPosition = new Vector3(
            target.position.x + currentLookAhead,
            followY ? target.position.y + yOffset : transform.position.y,
            -10
        );

        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref velocity,
            smoothTime
        );

        lastX = target.position.x;
    }
}