using UnityEngine;

public class NoteInputManager : MonoBehaviour
{
    [Header("Sequence System")]
    [SerializeField] private SequenceManager sequenceManager;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayNote(NoteType.Do);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayNote(NoteType.Re);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlayNote(NoteType.Mi);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PlayNote(NoteType.Fa);
        }
    }

    private void PlayNote(NoteType playedNote)
    {
        Debug.Log("Played Note: " + playedNote);

        if (sequenceManager == null)
        {
            Debug.LogWarning(
                "NoteInputManager: SequenceManager is not assigned."
            );

            return;
        }

        SequenceZone activeZone =
            sequenceManager.GetActiveZone();

        if (activeZone == null)
        {
            Debug.LogWarning(
                "NoteInputManager: No active Sequence Zone."
            );

            return;
        }

        activeZone.TrySubmitNote(playedNote);
    }
}