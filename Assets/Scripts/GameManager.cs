using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    public VideoNode startVideo;
    public VideoPlayer videoPlayer;
    public MainCanvas mainCanvas;

    VideoNode currentNode;

    private void Start()
    {
        currentNode = startVideo;
        StartCoroutine(PlayNodeClip(currentNode.Clip));

        videoPlayer.loopPointReached += CheckNextStage;
    }

    IEnumerator PlayNodeClip(VideoClip clip)
    {
        if (currentNode == null || currentNode.Clip == null)
        {
            Debug.LogError("Cannot find Clip");
        }

        videoPlayer.clip = clip;
        yield return new WaitForEndOfFrame();
        videoPlayer.Play();
    }

    void CheckNextStage(VideoPlayer source)
    {
        if(currentNode.Edges.Length == 1)
        {
            Debug.Log("Next video");
        }
        else if(currentNode.Edges.Length >= 2)
        {
            foreach(VideoEdge edge in currentNode.Edges)
            {
                mainCanvas.AddOptionButton(edge, OnOptionClicked);
            }
        }
    }

    void OnOptionClicked(VideoEdge edge)
    {
        mainCanvas.ClearOptions();
        currentNode = edge.NextNode;
        StartCoroutine(PlayNodeClip(currentNode.Clip));
    }
}
