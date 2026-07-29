using UnityEngine;
using UnityEngine.Events;

public class BossProjectile : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 4f;

    [Header("Audio")]
    [SerializeField] private AudioClip[] noteClips; // Element 0=Do, 1=Re, 2=Mi, 3=Fa

    public NoteType Note { get; private set; }

    // Assigned by BossAttackController after spawn
    public float PassingLineY { get; set; }

    public UnityEvent<BossProjectile> onCrossedLine;

    private AudioSource audioSource;
    private bool crossed;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
    }

    public void Init(NoteType note)
    {
        Note = note;

        int index = (int)note;
        if (noteClips != null && index < noteClips.Length && noteClips[index] != null)
            audioSource.PlayOneShot(noteClips[index]);
    }

    private void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);

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
}
