using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class BossProjectile : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 4f;

    [Header("Audio")]
    [SerializeField] private AudioClip[] noteClips; // Element 0=Do, 1=Re, 2=Mi, 3=Fa
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

    private AudioSource audioSource;
    private Vector2 moveDirection;
    private bool passedPlayer;
    private bool crossed;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
    }

    public void Init(NoteType[] notes, Vector2 targetPosition, float projectileSpeed = 4f)
    {
        Notes = notes;
        CurrentNoteIndex = 0;
        speed = projectileSpeed;
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

    private void PlayNoteSound(int index)
    {
        int clipIndex = (int)Notes[index];
        if (noteClips != null && clipIndex < noteClips.Length && noteClips[clipIndex] != null)
            audioSource.PlayOneShot(noteClips[clipIndex]);
    }

    private void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);

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

    public void Deflect()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        other.GetComponent<PlayerBossHealth>()?.TakeWrongNoteDamage();
        onHitPlayer?.Invoke(this);
        Destroy(gameObject);
    }
}
