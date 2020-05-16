using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public Button settingBtn;
    public Text bestScore;

    public RectTransform helpPanel;
    public RectTransform morePanel;
    public RectTransform settingPanel;
    public RectTransform switchPanel;
    public RectTransform contentPanel;
    
    

    public Slider sliderTotal;
    public Slider sliderBgm;
    public Slider sliderSound;
    
    private Vector2 helpStartPos;
    private Vector2 moreStartPos;

    private RectTransform currentPanel;
    

    private void Start()
    {
        
        
        helpStartPos = helpPanel.localPosition;
        moreStartPos = morePanel.localPosition;
        MusicManager.Instance.PlayBgm("EDM Music");
        TitleAnimation();

        #region 绑定事件
        
        sliderTotal.onValueChanged.AddListener((v) =>
        {
            MusicManager.Instance.SetTotalVolume(v);
        });
        
        sliderBgm.onValueChanged.AddListener((v) =>
        {
            MusicManager.Instance.SetBgmVolume(v);
        });
        
        sliderSound.onValueChanged.AddListener((v) =>
        {
            MusicManager.Instance.SetSoundVolume(v);
        });
      
        //点击开始按钮
        startBtn.onClick.AddListener(() =>
            {
                // PlayerPrefs.DeleteKey("oldLevels");
                // PlayerPrefs.SetString("oldLevels",PlayerPrefs.GetString("oldLevels")+"0_1_2");

                //初次启动游戏
                var data= PlayerPrefs.GetString("oldLevels");
                if (data==" "||data==string.Empty||data==null)
                {
                    PlayerPrefs.SetString("oldLevels","0");
                }

                // MusicManager.Instance.StopBgm();
                // GameManager.Instance.currentSceneIndex = 1;
                // SceneManager.LoadScene(1);
                MusicManager.Instance.PlaySound("click");
                switchPanel.gameObject.SetActive(true);
                switchPanel.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f).SetEase(Ease.OutBack).From();
                AddLevel();
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
        
        settingBtn.onClick.AddListener(() =>
        {
            MusicManager.Instance.PlaySound("click");
            settingPanel.DOLocalMoveX(-505, 1f).SetEase(Ease.OutBack);
            if (settingPanel.localPosition.x>-600)
            {
                settingPanel.DOLocalMoveX(-1200, 1f).SetEase(Ease.OutBack);
            }
        });
        
        #endregion
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

    //向选关容器动态设置关卡是否显示
    private void AddLevel()
    {
        var oldLevels = GetOldLevels();
        var allChild = contentPanel.GetComponentsInChildren<Image>();
        for (var i = 0; i < oldLevels.Length; i++)
        {
            var c = int.Parse(oldLevels[i]);
            allChild[c].color=new Color(1,1,1);
        }

        AddLevelLoadBtn();
    }

    //获得所有已经通关的关卡
    private string[] GetOldLevels()
    {
        return PlayerPrefs.GetString("oldLevels").Split('_');
    }

    //添加关卡按钮监听
    private void AddLevelLoadBtn()
    {
        var allChild = contentPanel.GetComponentsInChildren<Button>();
        
        print("子图像:"+allChild.Length);
        print(PlayerPrefs.GetString("oldLevels"));
        for (int i = 0; i < allChild.Length; i++)
        {
            print("Bt:"+allChild[i].name);
            allChild[i].onClick.AddListener(() => {print("i="+i); SceneManager.LoadScene(i-4);});
        }
    }
}