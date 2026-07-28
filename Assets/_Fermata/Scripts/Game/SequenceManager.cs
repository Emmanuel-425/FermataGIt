using UnityEngine;

public class SequenceManager : MonoBehaviour
{
    private SequenceZone activeZone;

    private void Start()
    {
        foreach (SequenceZone zone in FindObjectsByType<SequenceZone>(FindObjectsSortMode.None))
            zone.Deactivate();
    }

    public SequenceZone GetActiveZone()
    {
        return activeZone;
    }

    public void ActivateZone(SequenceZone newZone)
    {
        if (newZone == null) return;

        if (activeZone != null)
            activeZone.Deactivate();

        activeZone = newZone;
        activeZone.Activate();
    }

    public void ResetActiveZoneForRespawn()
    {
        if (activeZone != null)
            activeZone.ResetForRespawn();
    }
}
