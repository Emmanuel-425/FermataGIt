using UnityEngine;

public class ListenSystem : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform player;

    [Header("Sequence System")]
    [SerializeField] private SequenceManager sequenceManager;

    [Header("Listen Settings")]
    [SerializeField] private float revealDuration = 3f;
    [SerializeField] private float cooldown = 8f;
    [SerializeField] private float maximumListenDistance = 15f;

    private float cooldownTimer;

    private void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (
            Input.GetKeyDown(KeyCode.Q) &&
            cooldownTimer <= 0f
        )
        {
            Listen();
        }
    }

    private void Listen()
    {
        if (sequenceManager == null)
        {
            Debug.LogWarning(
                "ListenSystem: SequenceManager is not assigned."
            );

            return;
        }

        SequenceZone activeZone =
            sequenceManager.GetActiveZone();

        if (activeZone == null)
        {
            Debug.LogWarning(
                "ListenSystem: No active Sequence Zone."
            );

            return;
        }

        MusicPlatform nearestPlatform =
            activeZone.GetNearestHiddenPlatform(
                player.position
            );

        if (nearestPlatform == null)
        {
            Debug.Log(
                "Listen: No hidden platform found nearby."
            );

            return;
        }

        float distance = Vector2.Distance(
            player.position,
            nearestPlatform.transform.position
        );

        if (distance > maximumListenDistance)
        {
            Debug.Log(
                "Listen: No hidden platform is within range."
            );

            return;
        }

        Debug.Log(
            "Listen: Revealed nearby platform."
        );

        nearestPlatform.ListenReveal(
            revealDuration
        );

        cooldownTimer = cooldown;
    }

    public float GetCooldownRemaining()
    {
        return Mathf.Max(cooldownTimer, 0f);
    }
}