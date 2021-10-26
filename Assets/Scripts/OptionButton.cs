using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;
using UnityEngine.EventSystems;
using DG.Tweening;

public class OptionButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI title;
    public RawImage textureHolder;
    public VideoPlayer videoPlayer;

    Action<VideoEdge> m_clickCallBack;
    VideoEdge m_edge;
    CanvasGroup m_group;

    bool isPointerEnter;

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
        title.text = "";
    }

    void Start()
    {
        m_group = GetComponent<CanvasGroup>();
        m_group.alpha = 0;

        GetVideo();

        m_group.DOFade(1, 1.5f);
    }

    void Update()
    {
        if(videoPlayer.frame > 150)
        {
            videoPlayer.frame = 10;
        }
    }

    void GetVideo()
    {
        RenderTexture tex = new RenderTexture(480, 270, 16, RenderTextureFormat.ARGB32);
        tex.Create();
        textureHolder.texture = tex;
        videoPlayer.targetTexture = tex;

        StartCoroutine(SetThumbNail(m_edge.NextNode.Clip));
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
            videoPlayer.Prepare();
            yield return new WaitUntil(() => videoPlayer.isPrepared);
            videoPlayer.Play();
            yield return new WaitUntil(() => videoPlayer.isPlaying);
            yield return new WaitForSeconds(0.2f);
            videoPlayer.Pause();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerEnter = true;

        videoPlayer.Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerEnter = false;

        videoPlayer.Pause();
    }


}
