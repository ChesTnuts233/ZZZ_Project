// using System;
//
// namespace KooFrame.FrameTools
// {
//     public class TimeTask<T1, T2, T3, T4, T5> : TimeTaskBase
//     {
//         /// <summary>
//         /// 时间回调函数注册类
//         /// </summary>
//         /// <param name="FunName">带参数类型的回调函数地址</param>
//         /// <param name="fSeconds">设置的时间间隔1f=1秒 0.01f = 0.01秒 被除数=10000000</param>
//         /// <param name="isTriggerCallBackOnClear">当计时器任务被清理掉的时候，是否立即执行回调函数</param>
//         /// <param name="needFewTimes">需要回调的次数 1 = 1次, 0 =不需要执行</param>
//         public TimeTask(Action<T1, T2, T3, T4, T5> FunName, T1 d1, T2 d2, T3 d3, T4 d4, T5 d5,
//             float fSeconds = 1f, bool isTriggerCallBackOnClear = false, int needFewTimes = 1)
//         {
//             this.IsTriggerCallBackOnClear = isTriggerCallBackOnClear;
//             Callback = FunName;
//             this.d1 = d1;
//             this.d2 = d2;
//             this.d3 = d3;
//             this.d4 = d4;
//             this.d5 = d5;
//             SetEndTime(fSeconds);
//             SetCallNumber(needFewTimes);
//         }
//
//         /// <summary>
//         /// 调用数据变量
//         /// </summary>
//         private T1 d1;
//         private T2 d2;
//         private T3 d3;
//         private T4 d4;
//         private T5 d5;
//
//         /// <summary>
//         /// 调用数据变量
//         /// </summary>
//         private Action<T1, T2, T3, T4, T5> Callback = null;
//
//         /// <summary>
//         /// 唤醒时间到期之后的函数
//         /// </summary>
//         public override void launchEndTimeFun()
//         {
//             if (Callback != null)
//             {
//                 Callback(d1, d2, d3, d4, d5);
//             }
//         }
//     }
//
//     public class TimeTaskBaseBackFun<T1, T2, T3, T4> : TimeTaskBase
//     {
//         /// <summary>
//         /// 时间回调函数注册类
//         /// </summary>
//         /// <param name="FunName">带参数类型的回调函数地址</param>
//         /// <param name="fSeconds">设置的时间间隔1f=1秒 0.01f = 0.01秒 被除数=10000000</param>
//         /// <param name="isTriggerCallBackOnClear">当计时器任务被清理掉的时候，是否立即执行回调函数</param>
//         /// <param name="needFewTimes">需要回调的次数 1 = 1次, 0 =不需要执行</param>
//         public TimeTaskBaseBackFun(Action<T1, T2, T3, T4> FunName, T1 d1, T2 d2, T3 d3, T4 d4, float fSeconds = 1f,
//             bool isTriggerCallBackOnClear = false, int needFewTimes = 1)
//         {
//             this.IsTriggerCallBackOnClear = isTriggerCallBackOnClear;
//             Callback = FunName;
//             this.d1 = d1;
//             this.d2 = d2;
//             this.d3 = d3;
//             this.d4 = d4;
//             SetEndTime(fSeconds);
//             SetCallNumber(needFewTimes);
//         }
//
//         /// <summary>
//         /// 调用数据变量
//         /// </summary>
//         private T1 d1;
//         private T2 d2;
//         private T3 d3;
//         private T4 d4;
//
//         /// <summary>
//         /// 调用数据变量
//         /// </summary>
//         private Action<T1, T2, T3, T4> Callback = null;
//
//         /// <summary>
//         /// 唤醒时间到期之后的函数
//         /// </summary>
//         public override void launchEndTimeFun()
//         {
//             if (Callback != null)
//             {
//                 Callback(d1, d2, d3, d4);
//             }
//         }
//     }
//
//     public class TimeTaskBaseBackFun<T1, T2, T3> : TimeTaskBase
//     {
//         /// <summary>
//         /// 时间回调函数注册类
//         /// </summary>
//         /// <param name="FunName">带参数类型的回调函数地址</param>
//         /// <param name="fSeconds">设置的时间间隔1f=1秒 0.01f = 0.01秒 被除数=10000000</param>
//         /// <param name="isTriggerCallBackOnClear">当计时器任务被清理掉的时候，是否立即执行回调函数</param>
//         /// <param name="needFewTimes">需要回调的次数 1 = 1次, 0 =不需要执行</param>
//         public TimeTaskBaseBackFun(Action<T1, T2, T3> FunName, T1 d1, T2 d2, T3 d3, float fSeconds = 1f,
//             bool isTriggerCallBackOnClear = false, int needFewTimes = 1)
//         {
//             this.IsTriggerCallBackOnClear = isTriggerCallBackOnClear;
//             Callback = FunName;
//             this.d1 = d1;
//             this.d2 = d2;
//             this.d3 = d3;
//             SetEndTime(fSeconds);
//             SetCallNumber(needFewTimes);
//         }
//
//         /// <summary>
//         /// 调用数据变量
//         /// </summary>
//         private T1 d1;
//         private T2 d2;
//         private T3 d3;
//
//         /// <summary>
//         /// 调用数据变量
//         /// </summary>
//         private Action<T1, T2, T3> Callback = null;
//
//         /// <summary>
//         /// 唤醒时间到期之后的函数
//         /// </summary>
//         public override void launchEndTimeFun()
//         {
//             if (Callback != null)
//             {
//                 Callback(d1, d2, d3);
//             }
//         }
//     }
//
//     public class TimeTaskBaseBackFun<T1, T2> : TimeTaskBase
//     {
//         /// <summary>
//         /// 时间回调函数注册类
//         /// </summary>
//         /// <param name="FunName">带参数类型的回调函数地址</param>
//         /// <param name="fSeconds">设置的时间间隔1f=1秒 0.01f = 0.01秒 被除数=10000000</param>
//         /// <param name="isTriggerCallBackOnClear">当计时器任务被清理掉的时候，是否立即执行回调函数</param>
//         /// <param name="needFewTimes">需要回调的次数 1 = 1次, 0 =不需要执行</param>
//         public TimeTaskBaseBackFun(Action<T1, T2> FunName, T1 d1, T2 d2, float fSeconds = 1f,
//             bool isTriggerCallBackOnClear = false, int needFewTimes = 1)
//         {
//             this.IsTriggerCallBackOnClear = isTriggerCallBackOnClear;
//             Callback = FunName;
//             this.d1 = d1;
//             this.d2 = d2;
//             SetEndTime(fSeconds);
//             SetCallNumber(needFewTimes);
//         }
//
//         /// <summary>
//         /// 调用数据变量
//         /// </summary>
//         private T1 d1;
//         private T2 d2;
//
//         /// <summary>
//         /// 调用数据变量
//         /// </summary>
//         private Action<T1, T2> Callback = null;
//
//         /// <summary>
//         /// 唤醒时间到期之后的函数
//         /// </summary>
//         public override void launchEndTimeFun()
//         {
//             if (Callback != null)
//             {
//                 Callback(d1, d2);
//             }
//         }
//     }
//
//     /// <summary>
//     /// 时间回调函数式
//     /// </summary>
//     /// <typeparam name="T1"></typeparam>
//     public class TimeTaskBaseBackFun<T1> : TimeTaskBase
//     {
//         /// <summary>
//         /// 时间回调函数注册类
//         /// </summary>
//         /// <param name="FunName">带参数类型的回调函数地址</param>
//         /// <param name="fSeconds">设置的时间间隔1f=1秒 0.01f = 0.01秒 被除数=10000000</param>
//         /// <param name="isTriggerCallBackOnClear">当计时器任务被清理掉的时候，是否立即执行回调函数</param>
//         /// <param name="needFewTimes">需要回调的次数 1 = 1次, 0 =不需要执行</param>
//         public TimeTaskBaseBackFun(Action<T1> FunName, T1 d1, float fSeconds = 1f, bool isTriggerCallBackOnClear = false,
//             int needFewTimes = 1)
//         {
//             this.IsTriggerCallBackOnClear = isTriggerCallBackOnClear;
//             Callback = FunName;
//             this.d1 = d1;
//             SetEndTime(fSeconds);
//             SetCallNumber(needFewTimes);
//         }
//
//         /// <summary>
//         /// 调用数据变量
//         /// </summary>
//         private T1 d1;
//
//         /// <summary>
//         /// 调用数据变量
//         /// </summary>
//         private Action<T1> Callback = null;
//
//         /// <summary>
//         /// 唤醒时间到期之后的函数
//         /// </summary>
//         public override void launchEndTimeFun()
//         {
//             if (Callback != null)
//             {
//                 Callback(d1);
//             }
//         }
//     }
//
//     /// <summary>
//     /// 时间回调函数式
//     /// </summary>
//     /// <typeparam name="T"></typeparam>
//     public class TimeTaskBaseBackFun : TimeTaskBase
//     {
//         /// <summary>
//         /// 时间回调函数注册类
//         /// </summary>
//         /// <param name="FunName">带参数类型的回调函数地址</param>
//         /// <param name="fSeconds">设置的时间间隔1f=1秒 0.01f = 0.01秒 被除数=10000000</param>
//         /// <param name="isTriggerCallBackOnClear">当计时器任务被清理掉的时候，是否立即执行回调函数</param>
//         /// <param name="needFewTimes">需要回调的次数 1 = 1次, 0 =不需要执行</param>
//         public TimeTaskBaseBackFun(Action FunName, float fSeconds = 1f, bool isTriggerCallBackOnClear = false,
//             int needFewTimes = 1)
//         {
//             this.IsTriggerCallBackOnClear = isTriggerCallBackOnClear;
//             Callback = FunName;
//             SetEndTime(fSeconds);
//             SetCallNumber(needFewTimes);
//         }
//         
//         
//
//         /// <summary>
//         /// 调用数据变量
//         /// </summary>
//         private Action Callback = null;
//
//         /// <summary>
//         /// 唤醒时间到期之后的函数
//         /// </summary>
//         public override void launchEndTimeFun()
//         {
//             if (Callback != null)
//             {
//                 Callback();
//             }
//         }
//     }
// }