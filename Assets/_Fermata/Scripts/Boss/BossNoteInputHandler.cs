using UnityEngine;

public class BossNoteInputHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BossAttackController attackController;
    [SerializeField] private BossHealth bossHealth;
    [SerializeField] private PlayerBossHealth playerHealth;
    [SerializeField] private PlayerSpeedController speedController;

    [Header("Audio")]
    [SerializeField] private AudioClip wrongNoteClip;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) TryNote(NoteType.Do);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) TryNote(NoteType.Re);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) TryNote(NoteType.Mi);
        else if (Input.GetKeyDown(KeyCode.Alpha4)) TryNote(NoteType.Fa);
    }

    private void TryNote(NoteType played)
    {
        BossProjectile target = FindOldestProjectile();

        if (target == null) return;

        if (played == target.Note)
        {
            target.Deflect();
            bossHealth?.TakeDamage();
            speedController?.ApplyRecovery();
        }
        else
        {
            playerHealth?.TakeWrongNoteDamage();
            if (wrongNoteClip != null)
                audioSource.PlayOneShot(wrongNoteClip);
        }
    }

    // Targets the projectile closest to the passing line (highest Y)
    private BossProjectile FindOldestProjectile()
    {
        BossProjectile best = null;
        float highestY = float.NegativeInfinity;

        foreach (BossProjectile p in FindObjectsByType<BossProjectile>(FindObjectsSortMode.None))
        {
            if (p.transform.position.y > highestY)
            {
                highestY = p.transform.position.y;
                best = p;
            }
        }

        return best;
    }
}
