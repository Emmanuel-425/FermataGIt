using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour
{
    [SerializeField] private BossHealth bossHealth;
    [SerializeField] private Image heartPrefab;
    [SerializeField] private Transform container;

    [SerializeField] private float dimmedAlpha = 0.3f;

    private Image[] hearts;
    private const int heartsPerPhase = 15;
    private bool isPhaseTwo;

    private void Start()
    {
        hearts = new Image[heartsPerPhase];
        for (int i = 0; i < heartsPerPhase; i++)
            hearts[i] = Instantiate(heartPrefab, container);
    }

    // newHP goes from 30 down to 0
    public void OnBossDamaged(int newHP)
    {
        if (!isPhaseTwo)
        {
            // Phase 1: HP 30-16, dim hearts as they deplete
            // hitsInPhase = 15 - (newHP - 15) = 30 - newHP
            int lit = newHP - heartsPerPhase; // how many hearts are still full
            for (int i = 0; i < heartsPerPhase; i++)
            {
                Color c = hearts[i].color;
                c.a = i < lit ? 1f : dimmedAlpha;
                hearts[i].color = c;
            }
        }
        else
        {
            // Phase 2: HP 15-0, remove hearts
            for (int i = 0; i < heartsPerPhase; i++)
            {
                hearts[i].gameObject.SetActive(i < newHP);
            }
        }
    }

    public void OnPhaseTwo()
    {
        isPhaseTwo = true;
    }
}
