using UnityEngine;

public class SequenceManager : MonoBehaviour
{
    [Header("Starting Zone")]
    [SerializeField] private SequenceZone startingZone;

    private SequenceZone activeZone;

    private void Start()
    {
        SequenceZone[] allZones =
            FindObjectsByType<SequenceZone>(
                FindObjectsSortMode.None
            );

        foreach (SequenceZone zone in allZones)
        {
            zone.Deactivate();
        }

        if (startingZone != null)
        {
            ActivateZone(startingZone);
        }
        else
        {
            Debug.LogError(
                "SequenceManager: No starting zone assigned."
            );
        }
    }

    public SequenceZone GetActiveZone()
    {
        return activeZone;
    }

    public void ActivateZone(SequenceZone newZone)
    {
        if (newZone == null)
        {
            Debug.LogWarning(
                "SequenceManager: Tried to activate a null zone."
            );

            return;
        }

        if (activeZone != null)
        {
            activeZone.Deactivate();
        }

        activeZone = newZone;

        activeZone.Activate();

        Debug.Log(
            "Activated Sequence Zone: "
            + activeZone.gameObject.name
        );
    }

    public bool TryAdvanceToZone(
        SequenceZone requiredZone,
        SequenceZone nextZone
    )
    {
        if (activeZone != requiredZone)
        {
            return false;
        }

        if (!requiredZone.IsComplete())
        {
            Debug.Log(
                "Cannot advance yet. Complete the sequence first."
            );

            return false;
        }

        ActivateZone(nextZone);

        return true;
    }

    public void ResetActiveZoneForRespawn()
    {
        if (activeZone == null)
        {
            return;
        }

        activeZone.ResetForRespawn();
    }
}