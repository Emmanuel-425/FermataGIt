using UnityEngine;
using UnityEngine.Events;

public class BossHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHP = 30;

    [Header("Audio")]
    [SerializeField] private AudioClip hurtClip;

    public UnityEvent onPhaseTwo;
    public UnityEvent onDeath;
    public UnityEvent<int> onDamaged;

    private int currentHP;
    private bool phaseTwoTriggered;
    private AudioSource audioSource;

    public int CurrentHP => currentHP;

    private void Awake()
    {
        currentHP = maxHP;
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
    }

    public void TakeDamage(int amount = 1)
    {
        currentHP -= amount;
        Debug.Log($"Boss HP: {currentHP}");
        onDamaged?.Invoke(currentHP);

        if (hurtClip != null)
            audioSource.PlayOneShot(hurtClip);

        if (!phaseTwoTriggered && currentHP <= maxHP / 2)
        {
            phaseTwoTriggered = true;
            onPhaseTwo?.Invoke();
        }

        if (currentHP <= 0)
            onDeath?.Invoke();
    }
}
