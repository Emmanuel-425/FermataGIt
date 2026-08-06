using UnityEngine;

public class HazardBlock : MonoBehaviour
{
    [Header("Checkpoint")]
    [SerializeField] private CheckpointManager checkpointManager;

    private bool respawning;

    private void Awake()
    {
        if (checkpointManager == null)
        {
            checkpointManager =
                FindFirstObjectByType<CheckpointManager>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (respawning)
            return;

        if (!other.CompareTag("Player"))
            return;

        RespawnPlayer();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (respawning)
            return;

        if (!collision.gameObject.CompareTag("Player"))
            return;

        RespawnPlayer();
    }

    private void RespawnPlayer()
    {
        respawning = true;

        if (checkpointManager != null)
        {
            checkpointManager.RespawnPlayer();
        }
        else
        {
            Debug.LogError("HazardBlock: No CheckpointManager found.");
        }

        Invoke(nameof(ResetRespawn), 0.2f);
    }

    private void ResetRespawn()
    {
        respawning = false;
    }
}