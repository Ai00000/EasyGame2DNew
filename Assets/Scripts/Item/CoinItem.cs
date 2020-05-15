using System;
using FastyTools.EventCenter;
using UnityEngine;

namespace Item
{
    /// <summary>
    /// 金币道具（现在以牌子代替)
    /// </summary>
    public class CoinItem :  MonoBehaviour
    {
        public int points = 5;

        public bool first;


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player")&& !first)
            {
                GameManager.Instance.point += points;
                UIManager.Instance.coinText.text = "Score: "+GameManager.Instance.point;
                first = true;
            }
        }
    }
    
    
     
     
}