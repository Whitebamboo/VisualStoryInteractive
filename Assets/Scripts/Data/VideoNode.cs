using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "Node", menuName = "VideoNode")]
public class VideoNode : ScriptableObject
{
    [SerializeField] VideoTypeEnum videoType;
    [SerializeField] VideoClip clip;
    [SerializeField] VideoEdge[] edges;
    [SerializeField] string url;

    public VideoTypeEnum VideoType => videoType;
    public VideoClip Clip => clip;
    public VideoEdge[] Edges => edges;
    public string URL => url;
}
