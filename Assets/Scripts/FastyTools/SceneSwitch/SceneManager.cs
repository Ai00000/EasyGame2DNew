using System.Collections;
using FastyTools.Singleton;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/*
 * @Author:Fasty97
 * @UpdateTime: 2020年5月7日21:24:34
 */

namespace FastyTools.SceneSwitch
{
    /// <summary>
    /// 场景管理器
    /// </summary>
    public class SceneManager : SingletonManager<SceneManager>
    {
       public string CurrSceneName {
           get=>currName;
           private set => value = currName;
       }

       [SerializeField] private string currName;


      

       /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="scName">场景名</param>
        /// <param name="func"></param>
        public void LoadScene(string scName,UnityAction func)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(scName);
            CurrSceneName = scName;
            
            func();
        }
        
        /// <summary>
        /// 加载场景【异步】
        /// </summary>
        /// <param name="scName"></param>
        /// <param name="func"></param>
        public void LoadSceneAsync(string scName,UnityAction func)
        {
            CurrSceneName = scName;
            StartCoroutine(RealLoadSceneAsync(scName, func));
        }
        

        #region 私有
        
        /// <summary>
        /// 协程异步加载
        /// </summary>
        /// <param name="scName"></param>
        /// <param name="func"></param>
        /// <returns>进度（0-1）</returns>
        private IEnumerator RealLoadSceneAsync(string scName, UnityAction func)
        {
            var res = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scName);
            //是否完成加载
            while (res.isDone)
            {
                //向外分发进度条更新事件
                EventCenter.EventCenterManager.Instance.EventTrigger("loading",res.progress);
                yield return res.progress;    //返回进度（0-1）
            }
            func();
        }
        
        #endregion
        
    }
}