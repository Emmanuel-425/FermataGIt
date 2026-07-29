using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VictoryScreen : MonoBehaviour
{
    [Header("Video")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private RawImage videoDisplay;

    [Header("UI")]
    [SerializeField] private GameObject videoPanel;
    [SerializeField] private GameObject congratsPanel;
    [SerializeField] private Button skipButton;

    private void Start()
    {
        videoPanel.SetActive(false);
        congratsPanel.SetActive(false);
    }

    public void Play()
    {
        videoPanel.SetActive(true);
        skipButton.onClick.AddListener(Skip);
        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Play();
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        ShowCongrats();
    }

    private void Skip()
    {
        videoPlayer.Stop();
        ShowCongrats();
    }

    private void ShowCongrats()
    {
        videoPanel.SetActive(false);
        congratsPanel.SetActive(true);
    }
}
