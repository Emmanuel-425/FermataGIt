using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class BossProjectile : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 4f;
    [SerializeField] private float wrongNoteSpeed = 12f;
    [SerializeField] private float correctNoteSpeed = 10f;

    [Header("Audio")]
    [SerializeField] private AudioClip[] noteClips;
    [SerializeField] private float notePreviewDelay = 0.5f;

    public float PassingLineY { get; set; }
    public float PlayerY { get; set; }

    public NoteType[] Notes { get; private set; }
    public int NoteCount => Notes.Length;
    public int CurrentNoteIndex { get; private set; }
    public NoteType CurrentNote => Notes[CurrentNoteIndex];

    public UnityEvent<BossProjectile> onPassedPlayer;
    public UnityEvent<BossProjectile> onCrossedLine;
    public UnityEvent<BossProjectile> onHitPlayer;
    public UnityEvent<BossProjectile> onHitBoss;

    private AudioSource audioSource;
    private Vector2 moveDirection;
    private bool passedPlayer;
    private bool crossed;
    private bool redirected;
    private bool redirectedToPlayer;
    private int noteCount;

    private Transform bossTransform;
    private Transform playerTransform;
    private BossHealth bossHealth;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
    }

    public void Init(NoteType[] notes, Vector2 targetPosition, float projectileSpeed, Transform boss, Transform player, BossHealth health)
    {
        Notes = notes;
        noteCount = notes.Length;
        CurrentNoteIndex = 0;
        speed = projectileSpeed;
        bossTransform = boss;
        playerTransform = player;
        bossHealth = health;
        moveDirection = (targetPosition - (Vector2)transform.position).normalized;

        StartCoroutine(PlayNotePreview());
    }

    private IEnumerator PlayNotePreview()
    {
        for (int i = 0; i < Notes.Length; i++)
        {
            PlayNoteSound(i);
            yield return new WaitForSeconds(notePreviewDelay);
        }
    }

    public void AdvanceNote()
    {
        CurrentNoteIndex++;
    }

    public void DeflectToBoss()
    {
        redirected = true;
        StopAllCoroutines();
        speed = correctNoteSpeed;
        moveDirection = (bossTransform.position - transform.position).normalized;
    }

    public void DeflectToPlayer(Vector2 playerPosition)
    {
        redirected = true;
        redirectedToPlayer = true;
        StopAllCoroutines();
        speed = wrongNoteSpeed;
        moveDirection = (playerPosition - (Vector2)transform.position).normalized;
    }

    private void PlayNoteSound(int index)
    {
        int clipIndex = (int)Notes[index];
        if (noteClips != null && clipIndex < noteClips.Length && noteClips[clipIndex] != null)
            audioSource.PlayOneShot(noteClips[clipIndex]);
    }

    private void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);

        if (!redirected)
        {
            if (!passedPlayer && transform.position.y >= PlayerY)
            {
                passedPlayer = true;
                onPassedPlayer?.Invoke(this);
            }

            if (!crossed && transform.position.y >= PassingLineY)
            {
                crossed = true;
                onCrossedLine?.Invoke(this);
                Destroy(gameObject);
            }
        }
        else if (redirectedToPlayer && !crossed && transform.position.y >= PassingLineY)
        {
            crossed = true;
            onCrossedLine?.Invoke(this);
            Destroy(gameObject);
        }
    }

    private bool hasHit;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;

        if (redirected && other.CompareTag("Boss"))
        {
            hasHit = true;
            bossHealth?.TakeDamage(noteCount);
            onHitBoss?.Invoke(this);
            Destroy(gameObject);
            return;
        }

        if (other.CompareTag("Player"))
        {
            hasHit = true;
            other.GetComponent<PlayerBossHealth>()?.TakeWrongNoteDamage();
            onHitPlayer?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
