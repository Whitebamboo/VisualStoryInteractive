using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class MainCanvas : MonoBehaviour
{
    public OptionButton opitionButtonPrefab;
    public Transform optionContentHolder;
    public TextMeshProUGUI debugTitle;
    public GameObject blackBg;

    public void ShowBlackBg()
    {
        blackBg.SetActive(true);
    }

    public void AddOptionButton(VideoEdge edge, Action<VideoEdge> callback)
    {
        OptionButton button = Instantiate(opitionButtonPrefab, optionContentHolder);
        button.Init(edge, callback);
    }

    public void ClearOptions()
    {
        blackBg.SetActive(false);

        for (int i = optionContentHolder.childCount - 1; i >= 0; i--)
        {
            Destroy(optionContentHolder.GetChild(i).gameObject);
        }
    }

    public void ShowDebugWindow(string title)
    {
        debugTitle.transform.parent.gameObject.SetActive(true);
        debugTitle.text = title;
    }

}
