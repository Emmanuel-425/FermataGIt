using UnityEngine;
using UnityEngine.Events;

public class PlayerBossHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHP = 100;
    [SerializeField] private int wrongNoteDamage = 15;

    public UnityEvent onDeath;

    private int currentHP;

    private void Awake() => currentHP = maxHP;

    public void TakeWrongNoteDamage()
    {
        currentHP -= wrongNoteDamage;
        Debug.Log($"Player HP: {currentHP}");

        if (currentHP <= 0)
            onDeath?.Invoke();
    }
}
