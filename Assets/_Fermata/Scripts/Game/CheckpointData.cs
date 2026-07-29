using UnityEngine;

public class CheckpointData : MonoBehaviour
{
    public static CheckpointData Instance { get; private set; }

    public Vector3 SavedPosition { get; private set; }
    public bool HasSaved { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Save(Vector3 position)
    {
        SavedPosition = position;
        HasSaved = true;
    }
}
