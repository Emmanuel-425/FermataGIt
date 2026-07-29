using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private PlayerBossHealth playerHealth;
    [SerializeField] private Image heartPrefab;
    [SerializeField] private Transform container;

    private Image[] hearts;
    private int currentHP;
    private int maxHP = 10;

    private void Start()
    {
        hearts = new Image[maxHP];
        for (int i = 0; i < maxHP; i++)
        {
            hearts[i] = Instantiate(heartPrefab, container);
        }

        currentHP = maxHP;
    }

    public void OnPlayerDamaged(int newHP)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].gameObject.SetActive(i < newHP);
        }
    }
}
