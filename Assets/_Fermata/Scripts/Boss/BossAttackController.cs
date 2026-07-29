using UnityEngine;
using System.Collections;

public class BossAttackController : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform spawnPoint;

    [Header("Passing Line")]
    [SerializeField] private float passingLineY = 3f;

    [Header("Phase 1")]
    [SerializeField] private float phase1Interval = 1.2f;

    [Header("Phase 2")]
    [SerializeField] private float phase2ComboInterval = 2f;
    [SerializeField] private float[] noteCountSpeeds = { 5f, 4.5f, 3.5f, 2.5f };

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform boss;
    [SerializeField] private PlayerSpeedController speedController;
    [SerializeField] private BossNoteInputHandler noteInputHandler;
    [SerializeField] private BossHealth bossHealth;

    private float timer;
    private bool active;
    private bool isPhaseTwo;

    private NoteType[] allNotes;

    private void Awake()
    {
        allNotes = (NoteType[])System.Enum.GetValues(typeof(NoteType));
    }

    public void StartAttacking()
    {
        active = true;
        timer = 0f;
    }

    public void StopAttacking()
    {
        active = false;
        StopAllCoroutines();
    }

    public void EnterPhaseTwoAttack()
    {
        isPhaseTwo = true;
        StartCoroutine(PhaseTwoLoop());
    }

    private void Update()
    {
        if (!active || isPhaseTwo) return;

        timer += Time.deltaTime;
        if (timer >= phase1Interval)
        {
            timer = 0f;
            SpawnProjectile(1);
        }
    }

    private IEnumerator PhaseTwoLoop()
    {
        while (active)
        {
            int comboSize = Random.Range(1, 5);
            SpawnProjectile(comboSize);
            yield return new WaitForSeconds(phase2ComboInterval);
        }
    }

    private void SpawnProjectile(int noteCount)
    {
        GameObject go = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        BossProjectile proj = go.GetComponent<BossProjectile>();

        NoteType[] notes = GetUniqueNotes(noteCount);
        float speed = noteCountSpeeds[Mathf.Clamp(noteCount - 1, 0, noteCountSpeeds.Length - 1)];

        proj.PassingLineY = passingLineY;
        proj.PlayerY = player.position.y;
        proj.Init(notes, player.position, speed, boss, player, bossHealth);
        proj.onPassedPlayer.AddListener(OnProjectilePassedPlayer);
        proj.onHitPlayer.AddListener(OnProjectileHitPlayer);
        proj.onCrossedLine.AddListener(OnProjectileCrossedLine);
        proj.onHitBoss.AddListener(OnProjectileHitBoss);

        noteInputHandler?.RegisterProjectile(proj);
    }

    private NoteType[] GetUniqueNotes(int count)
    {
        NoteType[] shuffled = (NoteType[])allNotes.Clone();
        for (int i = shuffled.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (shuffled[i], shuffled[j]) = (shuffled[j], shuffled[i]);
        }

        NoteType[] result = new NoteType[count];
        for (int i = 0; i < count; i++)
            result[i] = shuffled[i];

        return result;
    }

    private void OnProjectilePassedPlayer(BossProjectile proj)
    {
        noteInputHandler?.UnregisterProjectile(proj);
    }

    private void OnProjectileHitPlayer(BossProjectile proj)
    {
        noteInputHandler?.UnregisterProjectile(proj);
    }

    private void OnProjectileCrossedLine(BossProjectile proj)
    {
        speedController?.ApplySlow();
    }

    private void OnProjectileHitBoss(BossProjectile proj)
    {
        // damage is handled by BossNoteInputHandler when deflecting
    }
}
