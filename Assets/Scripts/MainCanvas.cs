using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class MainCanvas : MonoBehaviour
{
    public OptionButton opitionButtonPrefab;
    public Transform optionContentHolder;

    public void AddOptionButton(VideoEdge edge, Action<VideoEdge> callback)
    {
        OptionButton button = Instantiate(opitionButtonPrefab, optionContentHolder);
        button.Init(edge, callback);
    }

    public void ClearOptions()
    {
        for (int i = optionContentHolder.childCount - 1; i >= 0; i--)
        {
            Destroy(optionContentHolder.GetChild(i).gameObject);
        }
    }
}
