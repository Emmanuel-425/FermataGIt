using UnityEngine;

public class MusicPlatform : MonoBehaviour
{
    [Header("Platform Settings")]
    [SerializeField] private NoteType note;

    [Header("Audio")]
    [SerializeField] private AudioClip noteClip;

    [Header("Listen Settings")]
    [SerializeField] private float listenLineWidth = 0.05f;

    private SpriteRenderer spriteRenderer;
    private Collider2D platformCollider;
    private LineRenderer listenOutline;
    private AudioSource audioSource;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        platformCollider = GetComponent<Collider2D>();

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;

        CreateListenOutline();
        Hide();
    }

    public NoteType GetNote()
    {
        return note;
    }

    public bool IsHidden()
    {
        return !spriteRenderer.enabled && !listenOutline.enabled;
    }

    public void RevealForSequence()
    {
        spriteRenderer.enabled = true;
        platformCollider.enabled = false;
        listenOutline.enabled = false;

        CancelInvoke(nameof(Hide));

        PlayNoteSound();
    }

    public void ActivateForCrossing(float duration)
    {
        spriteRenderer.enabled = true;
        platformCollider.enabled = true;

        listenOutline.enabled = false;

        CancelInvoke(nameof(Hide));

        if (duration > 0f)
        {
            Invoke(nameof(Hide), duration);
        }
    }

    public void Hide()
    {
        CancelInvoke(nameof(Hide));
        CancelInvoke(nameof(HideListenOutline));

        if (spriteRenderer != null) spriteRenderer.enabled = false;
        if (platformCollider != null) platformCollider.enabled = false;
        if (listenOutline != null) listenOutline.enabled = false;
    }

    public void ListenReveal(float duration)
    {
        spriteRenderer.enabled = false;
        platformCollider.enabled = false;

        UpdateListenOutline();

        listenOutline.enabled = true;

        PlayNoteSound();

        CancelInvoke(nameof(HideListenOutline));
        Invoke(nameof(HideListenOutline), duration);
    }

    private void PlayNoteSound()
    {
        if (noteClip != null)
            audioSource.PlayOneShot(noteClip);
    }

    private void HideListenOutline()
    {
        if (listenOutline != null)
        {
            listenOutline.enabled = false;
        }
    }

    private void CreateListenOutline()
    {
        GameObject outlineObject = new GameObject("Listen Outline");

        outlineObject.transform.SetParent(transform);
        outlineObject.transform.localPosition = Vector3.zero;
        outlineObject.transform.localRotation = Quaternion.identity;
        outlineObject.transform.localScale = Vector3.one;

        listenOutline = outlineObject.AddComponent<LineRenderer>();

        listenOutline.useWorldSpace = true;
        listenOutline.loop = true;

        listenOutline.startWidth = listenLineWidth;
        listenOutline.endWidth = listenLineWidth;

        listenOutline.positionCount = 4;

        listenOutline.material = new Material(Shader.Find("Sprites/Default"));

        listenOutline.startColor = Color.white;
        listenOutline.endColor = Color.white;

        listenOutline.enabled = false;
    }

    private void UpdateListenOutline()
    {
        if (listenOutline == null)
        {
            return;
        }

        Bounds bounds = spriteRenderer.bounds;

        Vector3 topLeft = new Vector3(
            bounds.min.x,
            bounds.max.y,
            0f
        );

        Vector3 topRight = new Vector3(
            bounds.max.x,
            bounds.max.y,
            0f
        );

        Vector3 bottomRight = new Vector3(
            bounds.max.x,
            bounds.min.y,
            0f
        );

        Vector3 bottomLeft = new Vector3(
            bounds.min.x,
            bounds.min.y,
            0f
        );

        listenOutline.SetPosition(0, topLeft);
        listenOutline.SetPosition(1, topRight);
        listenOutline.SetPosition(2, bottomRight);
        listenOutline.SetPosition(3, bottomLeft);
    }
}