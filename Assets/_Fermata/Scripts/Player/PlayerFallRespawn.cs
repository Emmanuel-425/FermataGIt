using UnityEngine;

public class PlayerFallRespawn : MonoBehaviour
{
    [Header("Fall Settings")]
    [SerializeField] private float fallYLimit = -20f;

    [Header("Checkpoint")]
    [SerializeField] private CheckpointManager checkpointManager;

    private bool respawning;

    private void Update()
    {
        if (respawning)
        {
            return;
        }

        if (transform.position.y <= fallYLimit)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        respawning = true;

        if (checkpointManager == null)
        {
            checkpointManager =
                FindFirstObjectByType<CheckpointManager>();
        }

        if (checkpointManager != null)
        {
            checkpointManager.RespawnPlayer();
        }
        else
        {
            Debug.LogError(
                "PlayerFallRespawn: No CheckpointManager found."
            );
        }

        Invoke(nameof(AllowRespawn), 0.2f);
    }

    private void AllowRespawn()
    {
        respawning = false;
    }
}