using UnityEngine;

public class SequenceTrigger : MonoBehaviour
{
    [Header("Sequence Transition")]
    [SerializeField] private SequenceZone requiredZone;
    [SerializeField] private SequenceZone nextZone;

    [Header("Manager")]
    [SerializeField] private SequenceManager sequenceManager;

    private bool hasTriggered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered)
        {
            return;
        }

        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (sequenceManager == null)
        {
            Debug.LogWarning(
                "SequenceTrigger: SequenceManager is not assigned."
            );

            return;
        }

        bool advanced =
            sequenceManager.TryAdvanceToZone(
                requiredZone,
                nextZone
            );

        if (advanced)
        {
            hasTriggered = true;

            Debug.Log(
                "Sequence Trigger activated. Moving to next zone."
            );
        }
    }
}