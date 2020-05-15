using System;
using FastyTools.EventCenter;
using FastyTools.Singleton;
using Item;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager :MonoBehaviour
{
    public Text coinText;

    public Text time;

    public Text timeUi;
    public Text scoreUi;


    public Text totalScore;

    public GameObject resPanel;

    public float timer;


    [Header("========Button=========")] public Button restartBtn;
    public Button nextBtn;
    public Button exitBtn;

    private void Start()
    {
        restartBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(GameManager.Instance.currentSceneIndex);
        });
        nextBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.currentSceneIndex += 1;
            SceneManager.LoadScene(GameManager.Instance.currentSceneIndex);
        });
        
        exitBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.currentSceneIndex = 0; SceneManager.LoadScene(0); });
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().canInput)
        {
            time.text = "Time： " + timer.ToString("#0.00");
        }
    }

    public void ShowRes()
    {
        resPanel.SetActive(true);
        GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().canInput = false;
        timeUi.text = "耗时: " + timer.ToString("#0.00");
        scoreUi.text = "分数: " + GameManager.Instance.point;

        float tal = (GameManager.Instance.point + (10000 / timer));
        var c = Math.Round(tal, 2);
        totalScore.text = "总分数" + (GameManager.Instance.point + (10000 / timer));


        if (c > PlayerPrefs.GetFloat("bestScore"))
        {
            PlayerPrefs.SetFloat("bestScore", (float) c);
        }
    }
}