using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartCanvas : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public VideoClip startClip;
    public GameObject startPage;
    public Button startButton;

    private void Start()
    {
        startPage.SetActive(false);
        videoPlayer.clip = startClip;
        videoPlayer.Play();
        videoPlayer.loopPointReached += OnClipEnds;
        startButton.onClick.AddListener(() => { SceneManager.LoadScene("Main"); });
    }

    private void OnClipEnds(VideoPlayer source)
    {
        startPage.SetActive(true);
    }
}
