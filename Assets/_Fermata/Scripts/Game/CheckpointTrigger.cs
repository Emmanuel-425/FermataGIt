using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    [Header("Checkpoint")]
    [SerializeField] private Transform checkpointPoint;

    [Header("Manager")]
    [SerializeField] private CheckpointManager checkpointManager;

    private bool activated;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activated)
        {
            return;
        }

        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (checkpointManager == null)
        {
            Debug.LogWarning(
                "CheckpointTrigger: CheckpointManager is not assigned."
            );

            return;
        }

        checkpointManager.SetCheckpoint(
            checkpointPoint
        );

        activated = true;

        Debug.Log("CHECKPOINT ACTIVATED!");
    }
}