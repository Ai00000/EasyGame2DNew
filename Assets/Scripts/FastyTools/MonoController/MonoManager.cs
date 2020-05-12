using System.Collections;
using System.ComponentModel;
using FastyTools.Singleton;
using UnityEngine;
using UnityEngine.Events;

/*
 * @Author:Fasty97
 * @UpdateTime: 2020年5月7日21:24:34
 */

namespace FastyTools.MonoController
{
    /// <summary>
    /// 全局MONO管理器(提供使用mono的生命周期，使用协程的支持[解决反射造成的开销])
    /// </summary>
    public class MonoManager : Singleton<MonoManager>
    {
        private MonoController controller;

        public MonoManager()
        {
            var  go=new GameObject("MonoController");
            go.AddComponent<MonoController>();
            controller = go.GetComponent<MonoController>();
        }
        
        // protected override void Awake()
        // {
        //     base.Awake();
        //     var  go=new GameObject("MonoController");
        //     go.AddComponent<MonoController>();
        // }
        
        
        /// <summary>
        /// 添加update更新
        /// </summary>
        /// <param name="fun"></param>
        public void AddUpdateLister(UnityAction fun)
        {
            controller.AddUpdateLister(fun); 
        }
        /// <summary>
        /// 移除管理器
        /// </summary>
        /// <param name="fun"></param>
        public void RemoveUpdateLister(UnityAction fun)
        {
            controller.RemoveUpdateLister(fun);
        }
    
        /// <summary>
        /// 启动协程
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public Coroutine StartCoroutine(string methodName)
        {
            return controller.StartCoroutine(methodName);
        }

        
        public Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value)
        {
            return controller.StartCoroutine(methodName, value);
        }

        public Coroutine StartCoroutine(IEnumerator routine)
        {
            return controller.StartCoroutine(routine);
        }



    }
}