using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public VideoNode startVideo;
    public VideoPlayer videoPlayer;
    public MainCanvas mainCanvas;

    VideoNode currentNode;

    VideoNode[] normalVideoList;
    VideoNode[] businessVideoList;
    VideoNode[] devilVideoList;

    bool isPlayingVlogVideo;

    private void Start()
    {
        currentNode = startVideo;
        StartCoroutine(PlayNodeClip(currentNode.Clip));

        videoPlayer.loopPointReached += CheckNextStage;

        normalVideoList = Resources.LoadAll("BusinessVideo", typeof(VideoNode)).Cast<VideoNode>().ToArray();
        businessVideoList = Resources.LoadAll("DevilVideo", typeof(VideoNode)).Cast<VideoNode>().ToArray();
        devilVideoList = Resources.LoadAll("NormalVideo", typeof(VideoNode)).Cast<VideoNode>().ToArray();
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
        if(!isPlayingVlogVideo)
        {
            VideoEdge normalOption = new VideoEdge(normalVideoList[Random.Range(0, normalVideoList.Length)], "normal");
            mainCanvas.AddOptionButton(normalOption, OnOptionClicked);

            VideoEdge businessOption = new VideoEdge(businessVideoList[Random.Range(0, normalVideoList.Length)], "business");
            mainCanvas.AddOptionButton(businessOption, OnOptionClicked);

            VideoEdge devilOption = new VideoEdge(devilVideoList[Random.Range(0, normalVideoList.Length)], "devil");
            mainCanvas.AddOptionButton(devilOption, OnOptionClicked);

            isPlayingVlogVideo = true;
        }
        else
        {
            currentNode = startVideo;
            StartCoroutine(PlayNodeClip(currentNode.Clip));

            isPlayingVlogVideo = false;
        }


        //No Edge transition for now
        //if(currentNode.Edges.Length == 1)
        //{
        //    Debug.Log("Next video");
        //}
        //else if(currentNode.Edges.Length >= 2)
        //{
        //    foreach(VideoEdge edge in currentNode.Edges)
        //    {
        //        mainCanvas.AddOptionButton(edge, OnOptionClicked);
        //    }
        //}
        //else
        //{
        //    Debug.Log("No Edge");
        //}
    }

    void OnOptionClicked(VideoEdge edge)
    {
        mainCanvas.ClearOptions();
        currentNode = edge.NextNode;
        StartCoroutine(PlayNodeClip(currentNode.Clip));
    }
}
