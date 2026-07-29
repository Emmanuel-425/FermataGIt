using UnityEngine;

public class SequenceZone : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] private SequenceManager sequenceManager;

    [Header("Platforms")]
    [SerializeField] private MusicPlatform[] platforms;

    [Header("Completion")]
    [SerializeField] private float completedPlatformDuration = 2f;

    [SerializeField]
    private float completionDelay = 0.3f;

    [Header("Audio")]
    [SerializeField] private AudioClip victoryJingle;

    private AudioSource audioSource;

    private NoteType[] sequence;
    private int currentProgress;
    private bool isActive;
    private bool isBridgeActive;
    private bool hasSolvedOnce;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;

        sequence = new NoteType[platforms.Length];
        for (int i = 0; i < platforms.Length; i++)
            sequence[i] = platforms[i].GetNote();

        Shuffle(sequence);

        HideAllPlatforms();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        sequenceManager?.ActivateZone(this);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        sequenceManager?.ClearZone(this);
    }

    public void Activate()
    {
        isActive = true;
        currentProgress = 0;
        isBridgeActive = false;

        CancelInvoke(nameof(ResetSequence));
        CancelInvoke(nameof(FinishBridgeActivation));

        HideAllPlatforms();
    }

    public void Deactivate()
    {
        isActive = false;
        currentProgress = 0;
        isBridgeActive = false;
        hasSolvedOnce = false;

        CancelInvoke(nameof(ResetSequence));
        CancelInvoke(nameof(FinishBridgeActivation));
    }

    public bool HasSolvedOnce() => hasSolvedOnce;

    public bool IsAttemptInProgress()
        => isActive && currentProgress > 0 && !isBridgeActive;

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
            if (platform == null || !platform.IsHidden())
                continue;

            float distance =
                Vector2.Distance(playerPosition, platform.transform.position);

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
            matchingPlatform.RevealForSequence();

        currentProgress++;

        if (currentProgress >= platforms.Length)
        {
            hasSolvedOnce = true;

            Invoke(nameof(FinishBridgeActivation), completionDelay);
        }
    }

    private void FinishBridgeActivation()
    {
        if (victoryJingle != null)
            audioSource.PlayOneShot(victoryJingle);

        isBridgeActive = true;

        foreach (MusicPlatform platform in platforms)
        {
            if (platform != null)
                platform.ActivateForCrossing(completedPlatformDuration);
        }

        Invoke(nameof(ResetSequence), completedPlatformDuration);
    }

    private void HandleWrongNote()
    {
        ResetSequence();
    }

    private void ResetSequence()
    {
        CancelInvoke(nameof(ResetSequence));
        CancelInvoke(nameof(FinishBridgeActivation));

        currentProgress = 0;
        isBridgeActive = false;

        HideAllPlatforms();

        BackgroundMusicManager.Instance?.Restore();
    }

    private NoteType GetExpectedNote()
    {
        return sequence[currentProgress];
    }

    private MusicPlatform FindPlatformForNote(NoteType targetNote)
    {
        foreach (MusicPlatform platform in platforms)
        {
            if (platform != null &&
                platform.GetNote() == targetNote)
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