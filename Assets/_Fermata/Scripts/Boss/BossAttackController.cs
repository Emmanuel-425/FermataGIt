using UnityEngine;

public class BossAttackController : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform spawnPoint;

    [Header("Target")]
    [SerializeField] private Transform player;
    [SerializeField] private float targetRandomOffset = 0.5f;

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

    private void Start()
    {
        // Automatically find the player if not assigned.
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
            else
            {
                Debug.LogError("BossAttackController: No GameObject with the Player tag was found.");
            }
        }
    }

    public void StartAttacking()
    {
        active = true;
        timer = 0f;
    }

    public void StopAttacking()
    {
        active = false;
    }

    public void EnterPhaseTwo()
    {
        currentInterval = phase2Interval;
    }

    private void Update()
    {
        if (!active)
            return;

        timer += Time.deltaTime;

        if (timer >= currentInterval)
        {
            timer = 0f;
            SpawnProjectile();
        }
    }

    private void SpawnProjectile()
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("BossAttackController: Projectile Prefab is missing.");
            return;
        }

        if (spawnPoint == null)
        {
            Debug.LogError("BossAttackController: Spawn Point is missing.");
            return;
        }

        if (player == null)
        {
            Debug.LogError("BossAttackController: Player reference is missing.");
            return;
        }

        GameObject projectileObject = Instantiate(
            projectilePrefab,
            spawnPoint.position,
            Quaternion.identity
        );

        BossProjectile projectile =
            projectileObject.GetComponent<BossProjectile>();

        if (projectile == null)
        {
            Debug.LogError("BossAttackController: Projectile prefab does not contain BossProjectile.");
            return;
        }

        NoteType note =
            allNotes[Random.Range(0, allNotes.Length)];

        Vector2 targetPosition = player.position;

        // Small random offset so every projectile isn't perfectly accurate.
        targetPosition += Random.insideUnitCircle * targetRandomOffset;

        projectile.PassingLineY = passingLineY;
        projectile.Init(note, targetPosition);

        projectile.onCrossedLine.AddListener(OnProjectileCrossed);
    }

    private void OnProjectileCrossed(BossProjectile projectile)
    {
        speedController?.ApplySlow();
    }
}