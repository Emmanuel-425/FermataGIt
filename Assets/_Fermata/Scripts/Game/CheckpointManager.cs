using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform player;

    [Header("Starting Checkpoint")]
    [SerializeField] private Transform startingCheckpoint;

    private Vector3 currentCheckpointPosition;

    private Rigidbody2D playerRigidbody;

    private void Start()
    {
        if (player == null)
        {
            Debug.LogError(
                "CheckpointManager: Player is not assigned."
            );

            return;
        }

        playerRigidbody =
            player.GetComponent<Rigidbody2D>();

        if (startingCheckpoint != null)
        {
            currentCheckpointPosition =
                startingCheckpoint.position;
        }
        else
        {
            currentCheckpointPosition =
                player.position;
        }

        if (CheckpointData.Instance != null && CheckpointData.Instance.HasSaved)
        {
            currentCheckpointPosition = CheckpointData.Instance.SavedPosition;
            RespawnPlayer();
        }
    }

    public void SetCheckpoint(Transform checkpoint)
    {
        if (checkpoint == null)
        {
            return;
        }

        currentCheckpointPosition =
            checkpoint.position;

        CheckpointData.Instance?.Save(currentCheckpointPosition);

        Debug.Log(
            "Checkpoint updated: "
            + currentCheckpointPosition
        );
    }

    public void RespawnPlayer()
    {
        if (player == null)
        {
            return;
        }

        if (playerRigidbody != null)
        {
            playerRigidbody.linearVelocity = Vector2.zero;

            playerRigidbody.position =
                currentCheckpointPosition;
        }
        else
        {
            player.position =
                currentCheckpointPosition;
        }

        SequenceManager sequenceManager =
            FindFirstObjectByType<SequenceManager>();

        if (sequenceManager != null)
        {
            sequenceManager.ResetActiveZoneForRespawn();
        }

        Debug.Log("Player respawned at checkpoint.");
    }

}