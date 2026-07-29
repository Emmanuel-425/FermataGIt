using UnityEngine;
using UnityEngine.Events;

public class BossProjectile : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 4f;

    [Header("Audio")]
    [SerializeField] private AudioClip[] noteClips; // Element 0=Do, 1=Re, 2=Mi, 3=Fa

    public NoteType Note { get; private set; }
    public float PassingLineY { get; set; }
    public float PlayerY { get; set; }

    private Vector2 moveDirection;

    public UnityEvent<BossProjectile> onPassedPlayer;
    public UnityEvent<BossProjectile> onCrossedLine;
    public UnityEvent<BossProjectile> onHitPlayer;

    private AudioSource audioSource;
    private bool passedPlayer;
    private bool crossed;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
    }

    public void Init(NoteType note, Vector2 targetPosition)
    {
        Note = note;
        moveDirection = (targetPosition - (Vector2)transform.position).normalized;

        int index = (int)note;
        if (noteClips != null && index < noteClips.Length && noteClips[index] != null)
            audioSource.PlayOneShot(noteClips[index]);
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
