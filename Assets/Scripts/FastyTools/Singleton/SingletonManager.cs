using System;
using JetBrains.Annotations;
using UnityEngine;

/*
 * @Author:Fasty97
 * @UpdateTime: 2020年5月7日21:24:34
 */

namespace FastyTools.Singleton
{
    /// <summary>
    /// 单例管理器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingletonManager<T> : MonoBehaviour where T : SingletonManager<T>
    {
        public static T Instance { get; private set; }
     

        protected SingletonManager()
        {
        }

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
                Debug.Log($"创建 {typeof(T)} ");
            }
    
            DontDestroyOnLoad(gameObject);
        }
    }
}