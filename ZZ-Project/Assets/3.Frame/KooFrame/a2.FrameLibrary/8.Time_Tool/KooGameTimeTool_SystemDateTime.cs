// using System.Collections.Generic;
// using System;
// using UnityEngine;
//
// namespace KooFrame.FrameTools
// {
//     /// <summary>
//     /// 事件回调回调函数 API层
//     /// </summary>
//     public partial class KooGameTimeTool
//     {
//        
//
//         /// <summary>
//         /// 存储
//         /// </summary>
//         private List<TimeTaskBase> lTimeFunData = new List<TimeTaskBase>();
//
//         /// <summary>
//         /// 添加线程安全能力
//         /// </summary>
//         private SafeQueue<TimeTaskBase> safeQueue = new SafeQueue<TimeTaskBase>();
//
//         /// <summary>
//         /// 存放所有的计时器任务返回的Guid
//         /// </summary>
//         public List<int> allTimeGuid = new List<int>();
//
//         public static int AddTimeCallbackFunction(TimeTaskBase taskBaseBackData)
//         {
//             TimeProgress().safeQueue.Enqueue(taskBaseBackData);
//             return taskBaseBackData.GetGuid();
//         }
//
//         /// <summary>
//         /// 添加计时器回调任务
//         /// </summary>
//         /// <param name="timeCallBack">过多少时间后执行的回调函数</param>
//         /// <param name="fSeconds">延迟时间</param>
//         /// <param name="onClearProcessFun"></param>
//         /// <param name="needFewTimes"></param>
//         /// <returns></returns>
//         public static int AddTimeCallbackFunction(Action timeCallBack, float fSeconds = 1f,
//             bool onClearProcessFun = false,
//             int needFewTimes = 1)
//         {
//             TimeTaskBase timeTaskBase = new TimeTaskBaseBackFun(timeCallBack, fSeconds, onClearProcessFun, needFewTimes);
//             TimeProgress().safeQueue.Enqueue(timeTaskBase);
//             return timeTaskBase.GetGuid();
//         }
//
//         public static int AddTimeCallbackFunction<T1>(Action<T1> FunName, T1 d1, float fSeconds = 1f,
//             bool onClearProcessFun = false, int needFewTimes = 1)
//         {
//             TimeTaskBase timeTaskBase = new TimeTaskBaseBackFun<T1>(FunName, d1, fSeconds, onClearProcessFun, needFewTimes);
//             TimeProgress().safeQueue.Enqueue(timeTaskBase);
//             return timeTaskBase.GetGuid();
//         }
//
//         public static int AddTimeCallbackFunction<T1, T2>(Action<T1, T2> FunName, T1 d1, T2 d2, float fSeconds = 1f,
//             bool onClearProcessFun = false, int needFewTimes = 1)
//         {
//             TimeTaskBase timeTaskBase =
//                 new TimeTaskBaseBackFun<T1, T2>(FunName, d1, d2, fSeconds, onClearProcessFun, needFewTimes);
//             TimeProgress().safeQueue.Enqueue(timeTaskBase);
//             return timeTaskBase.GetGuid();
//         }
//
//         public static int AddTimeCallbackFunction<T1, T2, T3>(Action<T1, T2, T3> FunName, T1 d1, T2 d2, T3 d3,
//             float fSeconds = 1f, bool onClearProcessFun = false, int needFewTimes = 1)
//         {
//             TimeTaskBase timeTaskBase =
//                 new TimeTaskBaseBackFun<T1, T2, T3>(FunName, d1, d2, d3, fSeconds, onClearProcessFun, needFewTimes);
//             TimeProgress().safeQueue.Enqueue(timeTaskBase);
//             return timeTaskBase.GetGuid();
//         }
//
//         public static int AddTimeCallbackFunction<T1, T2, T3, T4>(Action<T1, T2, T3, T4> FunName, T1 d1, T2 d2, T3 d3,
//             T4 d4, float fSeconds = 1f, bool onClearProcessFun = false, int needFewTimes = 1)
//         {
//             TimeTaskBase timeTaskBase =
//                 new TimeTaskBaseBackFun<T1, T2, T3, T4>(FunName, d1, d2, d3, d4, fSeconds, onClearProcessFun, needFewTimes);
//             TimeProgress().safeQueue.Enqueue(timeTaskBase);
//             return timeTaskBase.GetGuid();
//         }
//
//         public static int AddTimeCallbackFunction<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> FunName, T1 d1, T2 d2,
//             T3 d3, T4 d4, T5 d5, float fSeconds = 1f, bool onClearProcessFun = false, int needFewTimes = 1)
//         {
//             TimeTaskBase timeTaskBase = new TimeTask<T1, T2, T3, T4, T5>(FunName, d1, d2, d3, d4, d5, fSeconds,
//                 onClearProcessFun, needFewTimes);
//             TimeProgress().safeQueue.Enqueue(timeTaskBase);
//             return timeTaskBase.GetGuid();
//         }
//
//         /// <summary>
//         /// 循环遍历逻辑
//         /// </summary>
//         private void UpdataLogic()
//         {
//             #region 线程安全添加时间轴回调函数
//
//             try
//             {
//                 if (TimeProgress().safeQueue.Count > 0)
//                 {
//                     for (int i = 0; i < TimeProgress().safeQueue.Count; i++)
//                     {
//                         TimeTaskBase timeTaskBase = TimeProgress().safeQueue.Dequeue();
//                         if (timeTaskBase != null)
//                         {
//                             TimeProgress().lTimeFunData.Add(timeTaskBase);
//                         }
//                     }
//                 }
//             }
//             catch (Exception ex)
//             {
//                 Debug.LogError(ex.ToString());
//             }
//
//             #endregion
//
//             #region 处理事件回调函数
//
//             try
//             {
//                 if (lTimeFunData.Count == 0)
//                     return;
//
//                 TimeTaskBase.TimeNowToBinary = System.DateTime.Now.ToBinary();
//
//                 #region 清理标记为已经执行过的回到函数类
//
//                 for (int i = lTimeFunData.Count - 1; i >= 0; i--)
//                 {
//                     if (lTimeFunData[i].GetClean())
//                     {
//                         allTimeGuid.Remove(lTimeFunData[i].nGuid);
//                         lTimeFunData.Remove(lTimeFunData[i]);
//                     }
//                 }
//
//                 #endregion
//
//                 if (lTimeFunData.Count == 0)
//                     return;
//
//                 for (int i = 0; i < lTimeFunData.Count; i++)
//                 {
//                     TimeTaskBase Iter = lTimeFunData[i];
//                     if (Iter != null)
//                     {
//                         if (Iter.GetEndTime()) //设定的时间到期
//                         {
//                             Iter.launchEndTimeFun();
//                         }
//                     }
//                 }
//             }
//             catch (Exception ex)
//             {
//                 Debug.LogError(ex.ToString());
//             }
//
//             #endregion
//         }
//
//         /// <summary>
//         /// 主进程更新实例
//         /// 已经在MonoSystem里不断调用了 不需要再调用
//         /// </summary>
//         public static void Logic()
//         {
//             TimeProgress().UpdataLogic();
//         }
//
//         /// <summary>
//         /// 清理不需要的时间回调函数
//         /// </summary>
//         /// <param name="_Guid"></param>
//         /// <returns></returns>
//         private bool Clear(int _Guid, bool isUse)
//         {
//             if (this.lTimeFunData.Count == 0)
//                 return false;
//
//             #region 清理标记为已经执行过的回到函数类
//
//             for (int i = this.lTimeFunData.Count - 1; i >= 0; i--)
//             {
//                 if (this.lTimeFunData[i].nGuid == _Guid)
//                 {
//                     if (lTimeFunData[i].IsTriggerCallBackOnClear && isUse)
//                     {
//                         lTimeFunData[i].launchEndTimeFun();
//                     }
//
//                     this.lTimeFunData.RemoveAt(i);
//                     return true;
//                 }
//             }
//
//             #endregion
//
//             return false;
//         }
//
//         /// <summary>
//         /// 清理计时器任务
//         /// </summary>
//         /// <param name="Guid"></param>
//         /// <returns></returns>
//         public static bool ClearWaitTimeFuncByGuid(int Guid)
//         {
//             return TimeProgress().Clear(Guid, false);
//         }
//
//         /// <summary>
//         /// 清理计时器任务
//         /// 注意：之前注册计时器任务时设置的 onClearProcessFun 会生效
//         /// </summary>
//         /// <param name="Guid"></param>
//         /// <returns></returns>
//         public static bool ClearWaitTimeFuncByGuidWithFun(int Guid)
//         {
//             return TimeProgress().Clear(Guid, true);
//         }
//
//         /// <summary>
//         /// 清除掉所有的计时器任务
//         /// 注意：之前注册计时器任务时设置的 onClearProcessFun 会生效
//         /// </summary>
//         public static void ClearAllGuid()
//         {
//             if (TimeProgress().lTimeFunData.Count == 0)
//                 return;
//
//             for (int i = TimeProgress().lTimeFunData.Count - 1; i >= 0; i--)
//             {
//                 if (TimeProgress().allTimeGuid.Contains(TimeProgress().lTimeFunData[i].nGuid))
//                 {
//                     if (TimeProgress().lTimeFunData[i].IsTriggerCallBackOnClear)
//                     {
//                         TimeProgress().lTimeFunData[i].launchEndTimeFun();
//                     }
//
//                     TimeProgress().lTimeFunData.RemoveAt(i);
//                 }
//             }
//         }
//     }
// }