// using System;
// using System.Collections.Generic;
// using UnityEngine;
//
// namespace KooFrame.FrameTools
// {
//     //基于TimeData Mono的计时器系统
//     public partial class KooGameTimeTool
//     {
//         /// <summary>
//         /// 所有正在计时的计时任务
//         /// </summary>
//         private List<TimeTaskBase> TimeTaskList = new();
//
//         /// <summary>
//         /// 总计时
//         /// </summary>
//         private float _timer;
//
//
//         //构造函数
//         private KooGameTimeTool(Action action, float timer, Func<bool> testDestroy,
//             string functionName, bool useUnscaledDeltaTime)
//         {
//             this.action = action;
//             this.timer = timer;
//             this.testDestroy = testDestroy;
//             this.functionName = functionName;
//             this.useUnscaledDeltaTime = useUnscaledDeltaTime;
//             baseTimer = timer;
//         }
//
//
//         private static GameObject initGameObject; // 全局游戏对象，用于初始化类，场景切换时销毁
//
//         private static void InitIfNeeded()
//         {
//             if (initGameObject == null)
//             {
//                 initGameObject = new GameObject("KooGameTimeTool_Global");
//                 MonoSystem.PeriodicFuncs = new List<KooGameTimeTool>();
//             }
//         }
//
//
//         /// <summary>
//         /// 创建在场景切换的时候保持存在的全局定时器
//         /// </summary>
//         /// <param name="action"></param>
//         /// <param name="testDestroy"></param>
//         /// <param name="timer"></param>
//         /// <returns></returns>
//         public static KooGameTimeTool Create_Global(Action action, Func<bool> testDestroy, float timer)
//         {
//             // 创建定时器，设置为全局，可传入一个条件函数用于销毁判断
//             KooGameTimeTool KooGameTimeTool = Create(action, testDestroy, timer, "", false, false, false);
//             return KooGameTimeTool;
//         }
//
//
//         /// <summary>
//         /// 创建一个定时器
//         /// action在每个timer后执行 testDestory会在action执行后触发 如果返回true则销毁停止
//         /// </summary>
//         /// <param name="action">延迟触发的Action</param>
//         /// <param name="testDestroy">传入用来判断销毁的函数</param>
//         /// <param name="timer">延迟多久执行</param>
//         /// <returns></returns>
//         public static KooGameTimeTool Create(Action action, Func<bool> testDestroy, float timer)
//         {
//             return Create(action, testDestroy, timer, "", false);
//         }
//
//         /// <summary>
//         /// 创建定时器
//         /// </summary>
//         /// <param name="action"></param>
//         /// <param name="timer"></param>
//         /// <returns></returns>
//         public static KooGameTimeTool Create(Action action, float timer)
//         {
//             return Create(action, null, timer, "", false, false, false);
//         }
//
//         public static KooGameTimeTool Create(Action action, float timer, string functionName)
//         {
//             return Create(action, null, timer, functionName, false, false, false);
//         }
//
//         public static KooGameTimeTool Create(Action callback, Func<bool> testDestroy, float timer, string functionName,
//             bool stopAllWithSameName)
//         {
//             return Create(callback, testDestroy, timer, functionName, false, false, stopAllWithSameName);
//         }
//
//         public static KooGameTimeTool Create(Action action, Func<bool> testDestroy, float timer, string functionName,
//             bool useUnscaledDeltaTime, bool triggerImmediately, bool stopAllWithSameName)
//         {
//             InitIfNeeded();
//
//             if (stopAllWithSameName)
//             {
//                 StopAllFunc(functionName);
//             }
//
//
//             KooGameTimeTool KooGameTimeTool = new KooGameTimeTool(action, timer, testDestroy,
//                 functionName, useUnscaledDeltaTime);
//             MonoSystem.PeriodicFuncs.Add(KooGameTimeTool);
//
//             MonoSystem.AddUpdateListener(action);
//
//             if (triggerImmediately) action();
//
//             return KooGameTimeTool;
//         }
//
//         public static void RemoveTimer(KooGameTimeTool funcTimer)
//         {
//             InitIfNeeded();
//             MonoSystem.PeriodicFuncs.Remove(funcTimer);
//         }
//
//         public static void StopTimer(string _name)
//         {
//             InitIfNeeded();
//             for (int i = 0; i < funcList.Count; i++)
//             {
//                 if (funcList[i].functionName == _name)
//                 {
//                     funcList[i].DestroySelf();
//                     return;
//                 }
//             }
//         }
//
//         public static void StopAllFunc(string _name)
//         {
//             InitIfNeeded();
//             for (int i = 0; i < funcList.Count; i++)
//             {
//                 if (funcList[i].functionName == _name)
//                 {
//                     funcList[i].DestroySelf();
//                     i--;
//                 }
//             }
//         }
//
//         public static bool IsFuncActive(string name)
//         {
//             InitIfNeeded();
//             for (int i = 0; i < funcList.Count; i++)
//             {
//                 if (funcList[i].functionName == name)
//                 {
//                     return true;
//                 }
//             }
//
//             return false;
//         }
//
//
//         public void SkipTimerTo(float timer)
//         {
//             this.timer = timer;
//         }
//
//         void Update()
//         {
//             if (useUnscaledDeltaTime)
//             {
//                 timer -= Time.unscaledDeltaTime;
//             }
//             else
//             {
//                 timer -= Time.deltaTime;
//             }
//
//             if (timer <= 0)
//             {
//                 action();
//                 if (testDestroy != null && testDestroy())
//                 {
//                     //Destroy
//                     DestroySelf();
//                 }
//                 else
//                 {
//                     //Repeat
//                     timer += baseTimer;
//                 }
//             }
//         }
//
//         public void DestroySelf()
//         {
//             RemoveTimer(this);
//             //将Mono中的延迟函数都移除
//             foreach (var kooGameTimeTool in MonoSystem.PeriodicFuncs)
//             {
//                 MonoSystem.RemoveUpdateListener(kooGameTimeTool.action);
//             }
//
//             MonoSystem.PeriodicFuncs.Clear();
//         }
//     }
// }