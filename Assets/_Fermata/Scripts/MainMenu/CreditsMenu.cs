using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsMenu : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField] private string mainMenuScene = "MainMenu_Prototype";

    public void Back()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}