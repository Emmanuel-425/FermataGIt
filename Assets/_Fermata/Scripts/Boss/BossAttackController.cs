using UnityEngine;

public class BossAttackController : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform spawnPoint;

    [Header("Passing Line")]
    [SerializeField] private float passingLineY = 3f;

    [Header("Phase Intervals")]
    [SerializeField] private float phase1Interval = 1.2f;
    [SerializeField] private float phase2Interval = 0.9f;

    [Header("References")]
    [SerializeField] private PlayerSpeedController speedController;

    private float timer;
    private float currentInterval;
    private bool active;

    private NoteType[] allNotes;

    private void Awake()
    {
        allNotes = (NoteType[])System.Enum.GetValues(typeof(NoteType));
        currentInterval = phase1Interval;
    }

    public void StartAttacking()
    {
        active = true;
        timer = 0f;
    }

    public void StopAttacking() => active = false;

    public void EnterPhaseTwo() => currentInterval = phase2Interval;

    private void Update()
    {
        if (!active) return;

        timer += Time.deltaTime;
        if (timer >= currentInterval)
        {
            timer = 0f;
            SpawnProjectile();
        }
    }

    private void SpawnProjectile()
    {
        GameObject go = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        BossProjectile proj = go.GetComponent<BossProjectile>();

        NoteType note = allNotes[Random.Range(0, allNotes.Length)];

        proj.PassingLineY = passingLineY;
        proj.Init(note);
        proj.onCrossedLine.AddListener(OnProjectileCrossed);
    }

    private void OnProjectileCrossed(BossProjectile proj)
    {
        speedController?.ApplySlow();
    }
}
