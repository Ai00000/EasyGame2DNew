using System.Threading;

/*
 * @Author:Fasty97
 * @UpdateTime: 2020年5月7日21:24:34
 */

namespace FastyTools.Singleton
{
    /// <summary>
    /// 基础单例（线程锁）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> where T : new()
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;
                try
                {
                    _instance = new T();
                    Monitor.Enter(_instance);
                }
                finally
                {
                    Monitor.Exit(_instance);
                }

                return _instance;
            }
        }

       
    }
}