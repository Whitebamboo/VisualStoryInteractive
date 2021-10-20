using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Edge", menuName = "VideoEdge")]
public class VideoEdge
{
    VideoNode nextNode;
    string optionTitle;

    public VideoNode NextNode => nextNode;
    public string OptionTitle => optionTitle;

    public VideoEdge(VideoNode nextNode, string optionTitle)
    {
        this.nextNode = nextNode;
        this.optionTitle = optionTitle;
    }
}
