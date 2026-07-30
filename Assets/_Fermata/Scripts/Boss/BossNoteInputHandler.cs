using UnityEngine;
using System.Collections.Generic;

public class BossNoteInputHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BossAttackController attackController;
    [SerializeField] private PlayerBossHealth playerHealth;
    [SerializeField] private PlayerSpeedController speedController;

    [Header("Ripple")]
    [SerializeField] private GameObject smallRipplePrefab;
    [SerializeField] private GameObject bigRipplePrefab;
    [SerializeField] private Transform playerTransform;

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
        if (smallRipplePrefab != null && playerTransform != null)
            Instantiate(smallRipplePrefab, playerTransform.position, Quaternion.identity);

        if (projectileQueue.Count == 0) return;

        BossProjectile target = projectileQueue[0];

        if (played == target.CurrentNote)
        {
            speedController?.ApplyRecovery();

            bool isLastNote = target.CurrentNoteIndex >= target.NoteCount - 1;

            if (isLastNote)
            {
                projectileQueue.RemoveAt(0);
                target.DeflectToBoss();

                if (bigRipplePrefab != null && playerTransform != null)
                    Instantiate(bigRipplePrefab, playerTransform.position, Quaternion.identity);
            }
            else
            {
                target.AdvanceNote();
            }
        }
        else
        {
            projectileQueue.RemoveAt(0);
            if (smallRipplePrefab != null)
                Instantiate(smallRipplePrefab, target.transform.position, Quaternion.identity);
            target.DeflectToPlayer(playerTransform.position);
        }
    }
}
