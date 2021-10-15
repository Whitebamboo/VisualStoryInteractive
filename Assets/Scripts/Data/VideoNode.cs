using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "Node", menuName = "VideoNode")]
public class VideoNode : ScriptableObject
{
    [SerializeField] VideoClip clip;
    [SerializeField] VideoEdge[] edges;

    public VideoClip Clip => clip;
    public VideoEdge[] Edges => edges;
}
