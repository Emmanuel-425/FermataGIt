using UnityEngine;

public class SequenceZone : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] private SequenceManager sequenceManager;

    [Header("Platforms")]
    [SerializeField] private MusicPlatform[] platforms;

    [Header("Completion")]
    [SerializeField] private float completedPlatformDuration = 2f;

    private NoteType[] sequence;
    private int currentProgress;
    private bool isActive;
    private bool isBridgeActive;
    private bool hasSolvedOnce;

    private void Awake()
    {
        NoteType[] allNotes = (NoteType[])System.Enum.GetValues(typeof(NoteType));
        sequence = new NoteType[platforms.Length];

        for (int i = 0; i < platforms.Length; i++)
            sequence[i] = allNotes[i];

        Shuffle(sequence);

        HideAllPlatforms();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (sequenceManager != null)
            sequenceManager.ActivateZone(this);
    }

    public void Activate()
    {
        isActive = true;
        currentProgress = 0;
        isBridgeActive = false;

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

        if (playedNote == GetExpectedNote())
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
            if (platform == null || !platform.IsHidden()) continue;

            float distance = Vector2.Distance(playerPosition, platform.transform.position);

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
        if (!isActive) return;
        ResetSequence();
    }

    private void HandleCorrectNote(NoteType playedNote)
    {
        MusicPlatform matchingPlatform = FindPlatformForNote(playedNote);

        if (matchingPlatform != null)
            matchingPlatform.RevealForSequence();

        currentProgress++;

        if (currentProgress >= platforms.Length)
            ActivateBridge();
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
                platform.ActivateForCrossing(completedPlatformDuration);
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
        return sequence[currentProgress];
    }

    private MusicPlatform FindPlatformForNote(NoteType targetNote)
    {
        foreach (MusicPlatform platform in platforms)
        {
            if (platform != null && platform.GetNote() == targetNote)
                return platform;
        }

        return null;
    }

    private void HideAllPlatforms()
    {
        foreach (MusicPlatform platform in platforms)
        {
            if (platform != null)
                platform.Hide();
        }
    }

    private void Shuffle(NoteType[] arr)
    {
        for (int i = arr.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (arr[i], arr[j]) = (arr[j], arr[i]);
        }
    }
}
