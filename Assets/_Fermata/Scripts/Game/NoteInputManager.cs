using UnityEngine;
using System.Collections;

public class NoteInputManager : MonoBehaviour
{
    [Header("Sequence System")]
    [SerializeField] private SequenceManager sequenceManager;

    [Header("Input Cooldown")]
    [Tooltip("Time (in seconds) before another note can be played after a CORRECT note.")]
    [SerializeField] private float noteInputCooldown = 1f;

    private bool inputLocked;

    private void Update()
    {
        if (inputLocked)
            return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
            PlayNote(NoteType.Do);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            PlayNote(NoteType.Re);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            PlayNote(NoteType.Mi);

        if (Input.GetKeyDown(KeyCode.Alpha4))
            PlayNote(NoteType.Fa);
    }

    private void PlayNote(NoteType playedNote)
    {
        Debug.Log("Played Note: " + playedNote);

        if (sequenceManager == null)
        {
            Debug.LogWarning("NoteInputManager: SequenceManager is not assigned.");
            return;
        }

        SequenceZone activeZone = sequenceManager.GetActiveZone();

        if (activeZone == null)
        {
            Debug.LogWarning("NoteInputManager: No active Sequence Zone.");
            return;
        }

        bool wasCorrect = activeZone.TrySubmitNote(playedNote);

        if (wasCorrect)
        {
            BackgroundMusicManager.Instance?.Duck();
            StartCoroutine(InputCooldown());
        }
    }

    private IEnumerator InputCooldown()
    {
        inputLocked = true;

        yield return new WaitForSeconds(noteInputCooldown);

        inputLocked = false;

        SequenceZone activeZone = sequenceManager?.GetActiveZone();
        if (activeZone == null || !activeZone.IsAttemptInProgress())
            BackgroundMusicManager.Instance?.Restore();
    }
}