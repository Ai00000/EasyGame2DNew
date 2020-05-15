using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        print("触发");
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.point += 10;
            GameObject.Find("GameCanvas").GetComponent<UIManager>().coinText.text= "Score: "+GameManager.Instance.point;

            // UIManager.Instance.coinText.text = "Score: "+GameManager.Instance.point;
            //结算游戏
            print("结算");
            // UIManager.Instance.ShowRes();
            GameObject.Find("GameCanvas").GetComponent<UIManager>().ShowRes();

            
        }
    }
}
