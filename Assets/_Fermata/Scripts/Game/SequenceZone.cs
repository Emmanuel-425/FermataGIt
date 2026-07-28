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
    private bool isBridgeActive;
    private bool hasSolvedOnce;

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
        currentProgress = 0;
        isBridgeActive = false;
        hasSolvedOnce = false;

        CancelInvoke(nameof(ResetSequence));

        HideAllPlatforms();
    }

    public void Deactivate()
    {
        isActive = false;
        currentProgress = 0;
        isBridgeActive = false;
        hasSolvedOnce = false;

        CancelInvoke(nameof(ResetSequence));

        HideAllPlatforms();
    }

    public bool IsActive()
    {
        return isActive;
    }

    public bool IsBridgeActive()
    {
        return isBridgeActive;
    }

    // NEW
    public bool HasSolvedOnce()
    {
        return hasSolvedOnce;
    }

    public bool TrySubmitNote(NoteType playedNote)
    {
        if (!isActive)
            return false;

        if (isBridgeActive)
            return false;

        NoteType expectedNote = GetExpectedNote();

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
                continue;

            if (!platform.IsHidden())
                continue;

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
            return;

        ResetSequence();
    }

    private void HandleCorrectNote(NoteType playedNote)
    {
        MusicPlatform matchingPlatform =
            FindPlatformForNote(playedNote);

        if (matchingPlatform != null)
        {
            matchingPlatform.RevealForSequence();
        }

        currentProgress++;

        if (currentProgress >= 4)
        {
            ActivateBridge();
        }
    }

    private void HandleWrongNote()
    {
        ResetSequence();
    }

    private void ActivateBridge()
    {
        isBridgeActive = true;
        hasSolvedOnce = true;

        foreach (MusicPlatform platform in platforms)
        {
            if (platform != null)
            {
                platform.ActivateForCrossing(completedPlatformDuration);
            }
        }

        Invoke(nameof(ResetSequence), completedPlatformDuration);
    }

    private void ResetSequence()
    {
        CancelInvoke(nameof(ResetSequence));

        currentProgress = 0;
        isBridgeActive = false;

        HideAllPlatforms();
    }

    private NoteType GetExpectedNote()
    {
        switch (currentProgress)
        {
            case 0: return sequenceNote1;
            case 1: return sequenceNote2;
            case 2: return sequenceNote3;
            case 3: return sequenceNote4;
            default: return sequenceNote1;
        }
    }

    private MusicPlatform FindPlatformForNote(NoteType targetNote)
    {
        foreach (MusicPlatform platform in platforms)
        {
            if (platform == null)
                continue;

            if (platform.GetNote() == targetNote)
                return platform;
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