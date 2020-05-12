using System.Collections;
using System.Collections.Generic;
using System.IO;
using FastyTools.MonoController;
using FastyTools.Singleton;
using UnityEngine;
using UnityEngine.Events;

/*
 * @Author:Fasty97
 * @UpdateTime:2020年5月8日22:23:19
 */


namespace FastyTools.ResLoad
{
    /// <summary>
    /// 资源加载管理器
    /// </summary>
    public class ResManager : Singleton<ResManager>
    {
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Load<T>(string path) where T : Object
        {
            if (!AssetIsExist(path))
            {
                return null ;
            }
            var res = Resources.Load<T>(path);
            return res is GameObject ? Object.Instantiate(res) : res;
        }


        /// <summary>
        /// 加载资源【异步】
        /// </summary>
        /// <param name="path"></param>
        /// <param name="callBack"></param>
        /// <typeparam name="T"></typeparam>
        public void LoadAsync<T>(string path, UnityAction<T> callBack) where T : Object
        {
            if (!AssetIsExist(path))
            {
                 return ;
            }
            MonoManager.Instance.StartCoroutine(RealLoadAsync<T>(path, callBack));
        }


        private IEnumerator RealLoadAsync<T>(string path, UnityAction<T> callBack) where T : Object
        {
            if (!AssetIsExist(path))
            {
                yield return null;
            }
            var r = Resources.LoadAsync<T>(path);
            yield return r;

            if (r.asset is GameObject)
            {
                callBack(Object.Instantiate(r.asset) as T);
            }
            else
            {
                callBack(r.asset as T);
            }
        }

        
        /// <summary>
        /// 判断资源是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool AssetIsExist(string path)
        {
            // if (!File.Exists(path+".*"))
            // {
            //     Debug.LogError("资源："+path+"不存在");
            // }
            //
            // return File.Exists(path+".*");
            // var files= Directory.GetFiles(path + ".*");
            // return files.Length > 0;
            return true;
        }
    }
}