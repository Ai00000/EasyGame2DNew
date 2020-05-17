using System;
using FastyTools.EventCenter;
using FastyTools.Singleton;
using Item;
using UnityEngine;

public class GameManager: Singleton<GameManager>
{
        public int point;        //当前得分

        public int currentSceneIndex=1;
        //1YZXQ
        public string[] levelName =
        {
            "绿地",
            "通往冰霜之巅",
            "沙漠奇异植物",
            "暗黑火山",
            "绿色陷阱"
        };

        //添加关卡
        public void AddLevel()
        {
            if (currentSceneIndex==1)
            {
                return;
            }
            PlayerPrefs.SetString("oldLevels",PlayerPrefs.GetString("oldLevels")+"_"+ (currentSceneIndex-1));
        }

}