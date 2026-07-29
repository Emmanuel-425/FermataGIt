using UnityEngine;
using System.Collections.Generic;

public class BossNoteInputHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BossAttackController attackController;
    [SerializeField] private BossHealth bossHealth;
    [SerializeField] private PlayerBossHealth playerHealth;
    [SerializeField] private PlayerSpeedController speedController;

    private readonly List<BossProjectile> projectileQueue = new List<BossProjectile>();

    public void RegisterProjectile(BossProjectile projectile)
    {
        projectileQueue.Add(projectile);
    }

    public void UnregisterProjectile(BossProjectile projectile)
    {
        projectileQueue.Remove(projectile);
    }

    private void Update()
    {
        projectileQueue.RemoveAll(p => p == null);

        if (Input.GetKeyDown(KeyCode.Alpha1)) TryNote(NoteType.Do);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) TryNote(NoteType.Re);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) TryNote(NoteType.Mi);
        else if (Input.GetKeyDown(KeyCode.Alpha4)) TryNote(NoteType.Fa);
    }

    private void TryNote(NoteType played)
    {
        if (projectileQueue.Count == 0) return;

        BossProjectile target = projectileQueue[0];

        if (played == target.CurrentNote)
        {
            speedController?.ApplyRecovery();

            bool isLastNote = target.CurrentNoteIndex >= target.NoteCount - 1;

            if (isLastNote)
            {
                projectileQueue.RemoveAt(0);
                int damage = target.NoteCount;
                target.Deflect();
                bossHealth?.TakeDamage(damage);
            }
            else
            {
                target.AdvanceNote();
            }
        }
        else
        {
            projectileQueue.RemoveAt(0);
            target.Deflect();
            playerHealth?.TakeWrongNoteDamage();
        }
    }
}
