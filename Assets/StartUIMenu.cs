using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
    
    
    private void Start()
    {
        TitleAnimation();
        startBtn.onClick.AddListener(()=>SceneManager.LoadScene(1));
        moreBtn.onClick.AddListener(() => { });
        helpBtn.onClick.AddListener(() => { });
        quitBtn.onClick.AddListener(Application.Quit);
    }

    private void TitleAnimation()
    {
        title.DOMove(new Vector2(0, 0), 0.5f).From();
    }
}
