using UnityEngine;

public class PlayerSpeedController : MonoBehaviour
{
    [Header("Speed Settings")]
    [SerializeField] private float baseSpeed = 6f;
    [SerializeField] private float slowPerMiss = 0.35f;
    [SerializeField] private float recoveryPerHit = 0.25f;

    private PlayerMovement playerMovement;
    private float currentSpeed;

    public bool IsFrozen => currentSpeed <= 0f;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        currentSpeed = baseSpeed;
    }

    public void ApplySlow()
    {
        currentSpeed = Mathf.Max(0f, currentSpeed - slowPerMiss);
        ApplySpeed();
    }

    public void ApplyRecovery()
    {
        currentSpeed = Mathf.Min(baseSpeed, currentSpeed + recoveryPerHit);
        ApplySpeed();
    }

    private void ApplySpeed()
    {
        playerMovement.SetMoveSpeed(currentSpeed);
    }
}
