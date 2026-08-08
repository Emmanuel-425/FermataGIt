using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class VictoryScreen : MonoBehaviour
{
    [Header("Video")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private RawImage videoDisplay;

    [Header("UI")]
    [SerializeField] private GameObject videoPanel;
    [SerializeField] private Button skipButton;

    [Header("After Video")]
    [SerializeField] private string nextScene;
    [SerializeField] private float playDelay = 5f;

    private void Start()
    {
        videoPanel.SetActive(false);
    }

    public void Play()
    {
        StartCoroutine(PlayAfterDelay());
    }

    private IEnumerator PlayAfterDelay()
    {
        yield return new WaitForSeconds(playDelay);
        videoPanel.SetActive(true);
        skipButton.onClick.AddListener(Skip);
        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Play();
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        Proceed();
    }

    private void Skip()
    {
        videoPlayer.Stop();
        Proceed();
    }

    private void Proceed()
    {
        BackgroundMusicManager.Instance?.Restore();
        SceneManager.LoadScene(nextScene);
    }
}
