using UnityEngine;
using UnityEngine.Events;

public class BossProjectile : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 4f;

    [Header("Lifetime")]
    [SerializeField] private float maxLifeTime = 6f;

    [Header("Audio")]
    [SerializeField] private AudioClip[] noteClips;

    public NoteType Note { get; private set; }

    public float PassingLineY { get; set; }

    public UnityEvent<BossProjectile> onCrossedLine;

    private AudioSource audioSource;

    private Vector2 moveDirection;

    private bool crossed;
    private float timer;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
    }

    public void Init(NoteType note, Vector2 targetPosition)
    {
        Note = note;

        moveDirection =
            (targetPosition - (Vector2)transform.position).normalized;

        int index = (int)note;

        if (noteClips != null &&
            index >= 0 &&
            index < noteClips.Length &&
            noteClips[index] != null)
        {
            audioSource.PlayOneShot(noteClips[index]);
        }
    }

    private void Update()
    {
        transform.position +=
            (Vector3)(moveDirection * speed * Time.deltaTime);

        timer += Time.deltaTime;

        if (!crossed)
        {
            if (moveDirection.y > 0f)
            {
                if (transform.position.y >= PassingLineY)
                {
                    crossed = true;
                    onCrossedLine?.Invoke(this);
                    Destroy(gameObject);
                    return;
                }
            }
            else
            {
                if (transform.position.y <= PassingLineY)
                {
                    crossed = true;
                    onCrossedLine?.Invoke(this);
                    Destroy(gameObject);
                    return;
                }
            }
        }

        if (timer >= maxLifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerBossHealth playerHealth =
            other.GetComponent<PlayerBossHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeProjectileDamage();
        }

        Destroy(gameObject);
    }

    public void Deflect()
    {
        Destroy(gameObject);
    }
}