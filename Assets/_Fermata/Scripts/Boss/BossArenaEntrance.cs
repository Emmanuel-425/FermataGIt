using UnityEngine;
using UnityEngine.SceneManagement;

public class BossArenaEntrance : MonoBehaviour
{
    [SerializeField] private string bossSceneName = "Boss_Prototype";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        BackgroundMusicManager.Instance?.Duck();
        SceneManager.LoadScene(bossSceneName);
    }
}
