using UnityEngine;

public class SequenceZone : MonoBehaviour
{
    [Header("Sequence")]
    [SerializeField] private NoteType sequenceNote1 = NoteType.Do;
    [SerializeField] private NoteType sequenceNote2 = NoteType.Re;
    [SerializeField] private NoteType sequenceNote3 = NoteType.Mi;
    [SerializeField] private NoteType sequenceNote4 = NoteType.Fa;

    [Header("Platforms")]
    [SerializeField] private MusicPlatform platform1;
    [SerializeField] private MusicPlatform platform2;
    [SerializeField] private MusicPlatform platform3;
    [SerializeField] private MusicPlatform platform4;

    [Header("Completion")]
    [SerializeField] private float completedPlatformDuration = 2f;

    private int currentProgress;
    private bool isActive;
    private bool isComplete;

    private MusicPlatform[] platforms;

    private void Awake()
    {
        platforms = new MusicPlatform[]
        {
            platform1,
            platform2,
            platform3,
            platform4
        };

        HideAllPlatforms();
    }

    public void Activate()
    {
        isActive = true;
        isComplete = false;
        currentProgress = 0;

        HideAllPlatforms();
    }

    public void Deactivate()
    {
        isActive = false;
        isComplete = false;
        currentProgress = 0;

        HideAllPlatforms();
    }

    public bool IsActive()
    {
        return isActive;
    }

    public bool IsComplete()
    {
        return isComplete;
    }

    public bool TrySubmitNote(NoteType playedNote)
    {
        if (!isActive)
        {
            return false;
        }

        if (isComplete)
        {
            return false;
        }

        NoteType expectedNote = GetExpectedNote();

        Debug.Log(
            "Sequence expected: " + expectedNote +
            " | Player played: " + playedNote
        );

        if (playedNote == expectedNote)
        {
            HandleCorrectNote(playedNote);
            return true;
        }

        HandleWrongNote();

        return false;
    }

    public MusicPlatform GetNearestHiddenPlatform(Vector2 playerPosition)
    {
        MusicPlatform nearestPlatform = null;
        float closestDistance = Mathf.Infinity;

        foreach (MusicPlatform platform in platforms)
        {
            if (platform == null)
            {
                continue;
            }

            if (!platform.IsHidden())
            {
                continue;
            }

            float distance = Vector2.Distance(
                playerPosition,
                platform.transform.position
            );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearestPlatform = platform;
            }
        }

        return nearestPlatform;
    }

    public void ResetForRespawn()
    {
        if (!isActive)
        {
            return;
        }

        if (isComplete)
        {
            return;
        }

        currentProgress = 0;

        HideAllPlatforms();

        Debug.Log("Sequence reset because the player respawned.");
    }

    private void HandleCorrectNote(NoteType playedNote)
    {
        MusicPlatform matchingPlatform =
            FindPlatformForNote(playedNote);

        if (matchingPlatform != null)
        {
            matchingPlatform.RevealForSequence();
        }
        else
        {
            Debug.LogWarning(
                "No platform assigned for note: " + playedNote
            );
        }

        currentProgress++;

        Debug.Log(
            "Correct note! Progress: "
            + currentProgress + " / 4"
        );

        if (currentProgress >= 4)
        {
            CompleteSequence();
        }
    }

    private void HandleWrongNote()
    {
        Debug.Log("Wrong note! Sequence reset.");

        currentProgress = 0;

        HideAllPlatforms();
    }

    private void CompleteSequence()
    {
        isComplete = true;

        Debug.Log("SEQUENCE COMPLETE!");

        foreach (MusicPlatform platform in platforms)
        {
            if (platform == null)
            {
                continue;
            }

            platform.ActivateForCrossing(
                completedPlatformDuration
            );
        }
    }

    private NoteType GetExpectedNote()
    {
        switch (currentProgress)
        {
            case 0:
                return sequenceNote1;

            case 1:
                return sequenceNote2;

            case 2:
                return sequenceNote3;

            case 3:
                return sequenceNote4;

            default:
                return sequenceNote1;
        }
    }

    private MusicPlatform FindPlatformForNote(NoteType targetNote)
    {
        foreach (MusicPlatform platform in platforms)
        {
            if (platform == null)
            {
                continue;
            }

            if (platform.GetNote() == targetNote)
            {
                return platform;
            }
        }

        return null;
    }

    private void HideAllPlatforms()
    {
        foreach (MusicPlatform platform in platforms)
        {
            if (platform != null)
            {
                platform.Hide();
            }
        }
    }
}