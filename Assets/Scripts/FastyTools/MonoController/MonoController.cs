using UnityEngine;
using UnityEngine.Events;

/*
 * @Author:Fasty97
 * @UpdateTime: 2020年5月7日21:24:34
 */

namespace FastyTools.MonoController
{
    /// <summary>
    /// 公共Mono 
    /// </summary>
    public class MonoController : MonoBehaviour
    {

        private event UnityAction UpdateAction;
        
        /// <summary>
        /// 添加update更新
        /// </summary>
        /// <param name="fun"></param>
        public void AddUpdateLister(UnityAction fun)
        {
            UpdateAction += fun;
        }
        /// <summary>
        /// 移除管理器
        /// </summary>
        /// <param name="fun"></param>
        public void RemoveUpdateLister(UnityAction fun)
        {
            UpdateAction -= fun;
        }
        
        private void Update()
        {
            UpdateAction?.Invoke();
        }
    }
}