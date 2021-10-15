using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionButton : MonoBehaviour
{
    public TextMeshProUGUI title;

    Action<VideoEdge> m_clickCallBack;
    VideoEdge m_edge;

    public void Init(VideoEdge edge, Action<VideoEdge> callBack)
    {
        m_clickCallBack = callBack;
        m_edge = edge;

        title.text = edge.OptionTitle;
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
    }
}
