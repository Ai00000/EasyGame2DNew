using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIMenu : MonoBehaviour
{
    
    
    public GameObject menuPanel;
    public Button continueBtn;
    public Button restartBtn;
    public Button returnMenuBtn;
    
    // Start is called before the first frame update
    void Start()
    {
        
        //置空
        GameManager.Instance.point = 0;
        
        continueBtn.onClick.AddListener(() =>
        {
            menuPanel.SetActive(false);
            //
            GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().canInput = true;
        });
        
        restartBtn.onClick.AddListener(() => { SceneManager.LoadScene(GameManager.Instance.currentSceneIndex); });
        
        returnMenuBtn.onClick.AddListener(() => { SceneManager.LoadScene(0);});
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().canInput = menuPanel.activeSelf;
            menuPanel.SetActive(!menuPanel.activeSelf);

        }
    }
}
