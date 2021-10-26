using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;
using UnityEngine.EventSystems;

public class OptionButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
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
        GetVideo();
    }

    void GetVideo()
    {
        RenderTexture tex = new RenderTexture(480, 270, 16, RenderTextureFormat.ARGB32);
        tex.Create();
        textureHolder.texture = tex;
        videoPlayer.targetTexture = tex;

        StartCoroutine(SetThumbNail(m_edge.NextNode.Clip));
        //videoPlayer.clip = m_edge.NextNode.Clip;
        //videoPlayer.frame = 100;
        //videoPlayer.Play();

        //videoPlayer.Stop();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_clickCallBack == null)
        {
            Debug.LogError("Option call back is null");
            return;
        }

        m_clickCallBack.Invoke(m_edge);
    }

    IEnumerator SetThumbNail(VideoClip clip)
    {
        if (clip != null)
        {
            videoPlayer.clip = clip;
            videoPlayer.frame = 100;
            videoPlayer.Prepare();
            yield return new WaitUntil(() => videoPlayer.isPrepared);
            videoPlayer.Play();
            yield return new WaitUntil(() => videoPlayer.isPlaying);
            yield return new WaitForSeconds(0.3f);
            videoPlayer.Stop();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }
}
