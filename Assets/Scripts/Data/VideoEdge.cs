using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Edge", menuName = "VideoEdge")]
public class VideoEdge : ScriptableObject
{
    [SerializeField] VideoNode nextNode;
    [SerializeField]  string optionTitle;

    public VideoNode NextNode => nextNode;
    public string OptionTitle => optionTitle;
}
