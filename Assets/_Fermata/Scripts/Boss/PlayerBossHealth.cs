using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerBossHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHP = 10;
    [SerializeField] private int wrongNoteDamage = 1;

    public UnityEvent onDeath;
    public UnityEvent<int> onDamaged;

    private int currentHP;

    private void Awake() => currentHP = maxHP;

    public void TakeWrongNoteDamage()
    {
        currentHP -= wrongNoteDamage;
        Debug.Log($"Player HP: {currentHP}");
        onDamaged?.Invoke(currentHP);

        if (currentHP <= 0)
        {
            onDeath?.Invoke();
            SceneManager.LoadScene("Level_Prototype");
        }
    }
}
