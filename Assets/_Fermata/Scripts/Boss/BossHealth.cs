using UnityEngine;
using UnityEngine.Events;

public class BossHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHP = 30;

    public UnityEvent onPhaseTwo;
    public UnityEvent onDeath;

    private int currentHP;
    private bool phaseTwoTriggered;

    public int CurrentHP => currentHP;

    private void Awake() => currentHP = maxHP;

    public void TakeDamage()
    {
        currentHP--;
        Debug.Log($"Boss HP: {currentHP}");

        if (!phaseTwoTriggered && currentHP <= maxHP / 2)
        {
            phaseTwoTriggered = true;
            onPhaseTwo?.Invoke();
        }

        if (currentHP <= 0)
            onDeath?.Invoke();
    }
}
