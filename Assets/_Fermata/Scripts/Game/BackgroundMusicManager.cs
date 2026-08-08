using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    public static BackgroundMusicManager Instance { get; private set; }

    [Header("Audio")]
    [SerializeField] private AudioSource musicSource;

    [Header("Ducking")]
    [SerializeField] private float normalVolume = 1f;
    [SerializeField] private float duckVolume = 0.1f;
    [SerializeField] private float duckSpeed = 3f;

    private bool isDucked;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        float target = isDucked ? duckVolume : normalVolume;
        musicSource.volume = Mathf.MoveTowards(musicSource.volume, target, duckSpeed * Time.deltaTime);
    }

    public void Duck() => isDucked = true;
    public void Restore() => isDucked = false;
}
