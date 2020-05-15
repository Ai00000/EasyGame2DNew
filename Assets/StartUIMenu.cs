using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using FastyTools.Music;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartUIMenu : MonoBehaviour
{
    public RectTransform title;
    public Button startBtn;
    public Button moreBtn;
    public Button helpBtn;
    public Button quitBtn;
    public Text bestScore;

    public RectTransform helpPanel;
    public RectTransform morePanel;

    private Vector2 helpStartPos;
    private Vector2 moreStartPos;

    private RectTransform currentPanel;

    private void Start()
    {
        helpStartPos = helpPanel.localPosition;
        moreStartPos = morePanel.localPosition;
        MusicManager.Instance.PlayBgm("EDM Music");
        TitleAnimation();
        startBtn.onClick.AddListener(() =>
            {
                MusicManager.Instance.StopBgm();
                SceneManager.LoadScene(1);
                MusicManager.Instance.PlaySound("click");
            }
        );
        moreBtn.onClick.AddListener(() =>
        {
            MusicManager.Instance.PlaySound("click");

            if (currentPanel != null)
            {
                //消失
                currentPanel.DOLocalMoveX(helpStartPos.x, 0.5f).SetDelay(0.5f);
                morePanel.DOLocalMoveX(500, 1f).SetEase(Ease.OutBack).onComplete+=()=>currentPanel=morePanel;
            }
            else
            {
                morePanel.DOLocalMoveX(500, 1f).SetEase(Ease.OutBack).onComplete += () => currentPanel = morePanel;
                ;
            }

            // StartCoroutine(PanelClose());
        });

        helpBtn.onClick.AddListener(() =>
        {
            MusicManager.Instance.PlaySound("click");


            if (currentPanel != null)
            {
                //消失
                currentPanel.DOLocalMoveX(helpStartPos.x, 0.5f).SetDelay(0.5f);
                helpPanel.DOLocalMoveX(500, 1f).SetEase(Ease.OutBack).onComplete+=()=>currentPanel=helpPanel;
            }
            else
            {
                helpPanel.DOLocalMoveX(500, 1f).SetEase(Ease.OutBack).onComplete += () => currentPanel = helpPanel;
            }

            // StartCoroutine(PanelClose());
        });
        quitBtn.onClick.AddListener(Application.Quit);
        bestScore.text = "Best Score:" + PlayerPrefs.GetFloat("bestScore") + "";
    }


    IEnumerator PanelClose()
    {
        yield return new WaitForSeconds(20f);
        if (currentPanel == helpPanel)
        {
            currentPanel.DOLocalMoveX(helpStartPos.x, 0.5f).SetEase(Ease.OutBack).onComplete += () =>
            {
                currentPanel = null;
            };
        }
        else
        {
            currentPanel.DOLocalMoveX(moreStartPos.x, 0.5f).SetEase(Ease.OutBack).onComplete += () =>
            {
                currentPanel = null;
            };
        }
    }


    private void TitleAnimation()
    {
        title.DOMoveY(-10, 1f).From();
        title.DOScale(Vector3.one * 1.2f, 2f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.OutBack);
        startBtn.GetComponent<RectTransform>().DOLocalMoveX(1000, 0.5f).From().SetDelay(0.2f).SetEase(Ease.OutBack);
        moreBtn.GetComponent<RectTransform>().DOLocalMoveX(-1000, 0.5f).From().SetDelay(0.5f).SetEase(Ease.OutBack);
        helpBtn.GetComponent<RectTransform>().DOLocalMoveX(1000, 0.5f).From().SetDelay(0.8f).SetEase(Ease.OutBack);
        quitBtn.GetComponent<RectTransform>().DOLocalMoveX(-1000, 0.5f).From().SetDelay(1f).SetEase(Ease.OutBack);
    }


    private void SwitchPanel()
    {
        if (helpPanel.gameObject.activeSelf)
        {
        }
        else
        {
            morePanel.DOLocalMoveX(1000, 0.5f).From();
            morePanel.gameObject.SetActive(false);
        }
    }
}