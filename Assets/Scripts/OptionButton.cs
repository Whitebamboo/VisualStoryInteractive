using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;

public class OptionButton : MonoBehaviour
{
    public TextMeshProUGUI title;
    public RawImage textureHolder;
    public VideoPlayer videoPlayer;

    Action<VideoEdge> m_clickCallBack;
    VideoEdge m_edge;

    public void Init(VideoEdge edge, Action<VideoEdge> callBack)
    {
        m_clickCallBack = callBack;
        m_edge = edge;

        if(string.IsNullOrEmpty(edge.OptionTitle))
        {
            title.text = edge.name;
        }
        else
        {
            title.text = edge.OptionTitle;
        }
    }

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => 
        {
            if(m_clickCallBack == null)
            {
                Debug.LogError("Option call back is null");
                return;
            }

            m_clickCallBack.Invoke(m_edge);
        });

        GetVideo();
    }

    void GetVideo()
    {
        RenderTexture tex = new RenderTexture(480, 270, 16, RenderTextureFormat.ARGB32);
        tex.Create();
        textureHolder.texture = tex;
        videoPlayer.targetTexture = tex;

        videoPlayer.clip = m_edge.NextNode.Clip;
        videoPlayer.Play();
    }
}
