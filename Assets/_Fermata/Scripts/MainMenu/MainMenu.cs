using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField] private string gameScene = "Level_Prototype";
    [SerializeField] private string creditsScene = "Credits_Prototype";

    public void PlayGame()
    {
        SceneManager.LoadScene(gameScene);
    }

    public void OpenCredits()
    {
        SceneManager.LoadScene(creditsScene);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}