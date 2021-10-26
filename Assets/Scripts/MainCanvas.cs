using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MainCanvas : MonoBehaviour
{
    public OptionButton opitionButtonPrefab;
    public Transform optionContentHolder;
    public TextMeshProUGUI debugTitle;
    public GameObject blackBg;
    public GameObject endingGroup;
    public Button restart;
    public Button quit;
    public TextMeshProUGUI endingText;

    public void ShowBlackBg()
    {
        blackBg.SetActive(true);
        blackBg.GetComponent<Image>().DOFade(0, 0.5f).From();
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

    public void ShowEnding(VideoTypeEnum type)
    {
        endingGroup.SetActive(true);
        endingGroup.GetComponent<CanvasGroup>().DOFade(0, 1f).From();
        restart.onClick.AddListener(() => { SceneManager.LoadScene("Main"); });
        quit.onClick.AddListener(() => { Application.Quit(); });

        switch (type)
        {
            case VideoTypeEnum.Boring:
                endingText.text = "Ending: From Vlogging to Starting C++";
                break;
            case VideoTypeEnum.TikTok:
                endingText.text = "Ending: The Best Seller";
                break;
            case VideoTypeEnum.Prank:
                endingText.text = "Ending: The one who prank finally get pranked";
                break;
        }
    }
}
