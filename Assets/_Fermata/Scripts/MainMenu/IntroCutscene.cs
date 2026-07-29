using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroCutscene : MonoBehaviour
{
    [Header("Video")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject videoPanel;
    [SerializeField] private Button skipButton;

    [Header("Scene")]
    [SerializeField] private string gameScene = "Level_Prototype";

    private void Start()
    {
        videoPanel.SetActive(false);
    }

    public void Play()
    {
        videoPanel.SetActive(true);
        skipButton.onClick.AddListener(Skip);
        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Play();
    }

    private void OnVideoFinished(VideoPlayer vp) => LoadGame();

    private void Skip()
    {
        videoPlayer.Stop();
        LoadGame();
    }

    private void LoadGame()
    {
        SceneManager.LoadScene(gameScene);
    }
}
