using UnityEngine;
using UnityEngine.Events;

public class PlayerBossHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHP = 100;

    [Header("Damage")]
    [SerializeField] private int wrongNoteDamage = 15;
    [SerializeField] private int projectileDamage = 20;

    public UnityEvent onDeath;

    private int currentHP;

    private void Awake()
    {
        currentHP = maxHP;
    }

    public void TakeWrongNoteDamage()
    {
        TakeDamage(wrongNoteDamage);
    }

    public void TakeProjectileDamage()
    {
        TakeDamage(projectileDamage);
    }

    private void TakeDamage(int amount)
    {
        currentHP -= amount;

        Debug.Log($"Player HP: {currentHP}");

        if (currentHP <= 0)
        {
            currentHP = 0;
            onDeath?.Invoke();
        }
    }
}