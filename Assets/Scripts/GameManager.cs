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
            Debug.Log("Cannot find Clip");
            mainCanvas.ShowDebugWindow(currentNode.name);
            CheckNextStage(null);
        }
        else
        {
            videoPlayer.clip = clip;
            videoPlayer.Prepare();
            yield return new WaitUntil(() => videoPlayer.isPrepared);
            videoPlayer.Play();
        }
    }

    void CheckNextStage(VideoPlayer source)
    {
        if (currentNode.Edges.Length >= 1)
        {
            mainCanvas.ShowBlackBg();

            foreach (VideoEdge edge in currentNode.Edges)
            {
                mainCanvas.AddOptionButton(edge, OnOptionClicked);                
            }
        }
        else
        {
            Debug.Log("No Edge");
        }
    }

    void OnOptionClicked(VideoEdge edge)
    {
        mainCanvas.ClearOptions();
        currentNode = edge.NextNode;
        StartCoroutine(PlayNodeClip(currentNode.Clip));
    }


    public void SkipToEnd()
    {
        ulong frameCount = videoPlayer.frameCount;

        videoPlayer.frame = (long)frameCount - 1;
    }
}
