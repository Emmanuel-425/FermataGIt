using UnityEngine;

public class BossBattleCoordinator : MonoBehaviour
{
    [SerializeField] private BossHealth bossHealth;
    [SerializeField] private BossAttackController attackController;

    private void Start()
    {
        bossHealth.onPhaseTwo.AddListener(attackController.EnterPhaseTwoAttack);
        bossHealth.onDeath.AddListener(attackController.StopAttacking);
        attackController.StartAttacking();
    }
}
