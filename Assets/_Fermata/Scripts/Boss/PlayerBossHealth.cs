using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerBossHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHP = 10;
    [SerializeField] private int wrongNoteDamage = 1;

    [Header("Death")]
    [SerializeField] private float deathDelay = 1f;

    [Header("Audio")]
    [SerializeField] private AudioClip hurtClip;

    public UnityEvent onDeath;
    public UnityEvent<int> onDamaged;

    private int currentHP;
    private AudioSource audioSource;

    private void Awake()
    {
        currentHP = maxHP;
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
    }

    public void TakeWrongNoteDamage()
    {
        currentHP -= wrongNoteDamage;
        Debug.Log($"Player HP: {currentHP}");
        onDamaged?.Invoke(currentHP);

        if (hurtClip != null)
            audioSource.PlayOneShot(hurtClip);

        if (currentHP <= 0)
        {
            onDeath?.Invoke();
            Invoke(nameof(LoadScene), deathDelay);
        }
    }

    private void LoadScene()
    {
        SceneManager.LoadScene("World 1");
    }
}
