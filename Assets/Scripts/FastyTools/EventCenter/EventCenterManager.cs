using System.Collections.Generic;
using FastyTools.Singleton;
using UnityEngine.Events;


/*
 * @Author:Fasty97
 * @UpdateTime: 2020年5月8日16:47:58
 */

namespace FastyTools.EventCenter
{
    # region 解决Object装拆箱

    public interface IEventInfo
    {
    }

    public class EventInfo<T> : IEventInfo
    {
        public UnityAction<T> Actions { get; set; }

        public EventInfo(UnityAction<T> action)
        {
            Actions = action;
        }
    }

    public class EventInfo : IEventInfo
    {
        public UnityAction Actions { get; set; }

        public EventInfo(UnityAction action)
        {
            Actions = action;
        }
    }

    # endregion

    /// <summary>
    /// 事件中心管理器
    /// </summary>
    public class EventCenterManager : SingletonManager<EventCenterManager>
    {
        private readonly Dictionary<string, IEventInfo> eventDict =
            new Dictionary<string, IEventInfo>();
        
        #region 参数的

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="action">绑定action</param>
        public void AddEventLister<T>(string eventName, UnityAction<T> action)
        {
            if (eventDict.ContainsKey(eventName))
            {
                var unityAction = (eventDict[eventName] as EventInfo<T>)?.Actions;
                if (unityAction != null)
                    unityAction += action;
            }
            else
            {
                eventDict.Add(eventName, new EventInfo<T>(action));
            }
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="action">绑定action</param>
        public void RemoveEventLister<T>(string eventName, UnityAction<T> action)
        {
            if (eventDict.ContainsKey(eventName))
            {
                var unityAction = (eventDict[eventName] as EventInfo<T>)?.Actions;
                unityAction -= action;
            }
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="parameter">参数</param>
        public void EventTrigger<T>(string eventName, T parameter)
        {
            (eventDict[eventName] as EventInfo<T>)?.Actions?.Invoke(parameter);
        }

        #endregion


        #region 无参数的

        /// <summary>
        /// 添加事件监听【无参数】
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="action"></param>
        public void AddEventLister(string eventName, UnityAction action)
        {
            if (eventDict.ContainsKey(eventName))
            {
                var unityAction = (eventDict[eventName] as EventInfo)?.Actions;
                if (unityAction != null)
                    unityAction += action;
            }
            else
            {
                eventDict.Add(eventName, new EventInfo(action));
            }
        }

        /// <summary>
        /// 移除事件监听【无参数】
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="action">绑定action</param>
        public void RemoveEventLister(string eventName, UnityAction action)
        {
            if (eventDict.ContainsKey(eventName))
            {
                var unityAction = (eventDict[eventName] as EventInfo)?.Actions;
                unityAction -= action;
            }
        }

        /// <summary>
        /// 触发事件【无参数】
        /// </summary>
        /// <param name="eventName">事件名</param>
        public void EventTrigger(string eventName)
        {
            (eventDict[eventName] as EventInfo)?.Actions?.Invoke();
        }

        #endregion

        /// <summary>
        /// 清空（场景切换时）
        /// </summary>
        public void Clear()
        {
            eventDict.Clear();
        }
    }
}