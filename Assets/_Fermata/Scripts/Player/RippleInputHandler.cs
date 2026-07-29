using UnityEngine;

public class RippleInputHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NoteInputManager noteInputManager;
    [SerializeField] private SequenceManager sequenceManager;

    [Header("Ripple Prefabs")]
    [SerializeField] private GameObject normalRipplePrefab;
    [SerializeField] private GameObject largeRipplePrefab;

    private void OnEnable()
    {
        noteInputManager.OnCorrectNote += OnCorrectNote;
        noteInputManager.OnWrongNote += OnWrongNote;
    }

    private void OnDisable()
    {
        noteInputManager.OnCorrectNote -= OnCorrectNote;
        noteInputManager.OnWrongNote -= OnWrongNote;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            SpawnRipple(largeRipplePrefab, transform.position);
    }

    private void OnCorrectNote()
    {
        SpawnRipple(normalRipplePrefab, transform.position);

        SequenceZone zone = sequenceManager.GetActiveZone();
        if (zone == null) return;

        foreach (MusicPlatform platform in zone.GetPlatforms())
        {
            if (platform != null && !platform.IsHidden())
                SpawnRipple(normalRipplePrefab, platform.transform.position);
        }

        zone.OnSequenceCompleted -= OnSequenceCompleted;
        zone.OnSequenceCompleted += OnSequenceCompleted;
    }

    private void OnWrongNote()
    {
        SpawnRipple(normalRipplePrefab, transform.position);
    }

    private void OnSequenceCompleted()
    {
        SequenceZone zone = sequenceManager.GetActiveZone();
        if (zone == null) return;

        zone.OnSequenceCompleted -= OnSequenceCompleted;

        foreach (MusicPlatform platform in zone.GetPlatforms())
        {
            if (platform != null)
                SpawnRipple(normalRipplePrefab, platform.transform.position);
        }
    }

    private void SpawnRipple(GameObject prefab, Vector3 position)
    {
        if (prefab == null) return;
        Instantiate(prefab, position, Quaternion.identity);
    }
}
