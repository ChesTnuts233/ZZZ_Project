using UnityEngine;


namespace KooFrame.FrameTools
{
    /// <summary>
    /// 时间函数回调基础定义类
    /// </summary>
    public class TimeTaskBase
    {
        #region 添加TUID 标记了该时间任务

        //private int _tuid = -1; //默认的guid为-1

        #endregion

        /// <summary>
        /// 任务状态
        /// </summary>
        public enum State
        {
            Running,  //运行中的
            Pause,    //暂停执行
            Stop,     //被标记为停止的 等待回调任务触发完毕后会标记为Finished
            Finished, //已经结束运行的
        }

        /// <summary>
        /// 这个时间任务的状态
        /// </summary>
        public State TimeTaskState;


        #region 任务时间相关内容

        /// <summary>
        /// 延迟多久触发回调任务
        /// </summary>
        public float TriggerDelayTime;

        /// <summary>
        /// 这个任务开始的时间
        /// </summary>
        public Time TaskStartTime;

        /// <summary>
        /// 这个任务已经执行的时长
        /// </summary>
        public float TaskExecutedDuration;

        /// <summary>
        /// 这个任务会执行的总时间
        /// </summary>
        public float TaskMaxTime;

        #endregion


        #region 回调任务相关

        /// <summary>
        /// 是否重复触发回调函数
        /// </summary>
        public bool IsLoopTriggerCallBack;

        /// <summary>
        /// 这个时间任务需要触发多少次回调函数
        /// 默认为1次 参数为-1的时候 函数无限次数触发
        /// <see cref="TriggerDelayTime"/>为回调函数的触发间隔
        /// </summary>
        public int CallBackNeedTriggeredNum;

        /// <summary>
        /// 这个时间任务中的回调函数已经触发的次数
        /// </summary>
        public int CallBackTriggerNums;

        /// <summary>
        /// 当计时器任务被清理掉的时候，是否立即执行回调函数
        /// </summary>
        public bool IsTriggerCallBackOnClear = false;

        #endregion


        /// <summary>
        ///  等待下一次回调函数执行完毕后停止计时任务
        /// 这里考虑的情况有
        /// 1.停止的时候 是否等待下一次延迟回调触发完毕后再停止(考虑一些计时任务是至少执行完毕再回收的)
        /// 2.立即停止 不再触发回调任务(考虑这种情况应该是最常用的)
        /// 3.立即停止 直接触发回调任务(考虑回调任务是一种延迟的结束处理，如果提前结束任务,那就要)
        /// </summary>
        public void StopTimeTaskWaitCallBack()
        {
            //标记时间任务为停止状态
            TimeTaskState = State.Stop;
        }

        /// <summary>
        /// 立刻停止时间任务
        /// </summary>
        public void ImmediateStopTimeTask()
        {
            //标记时间任务为结束状态
            TimeTaskState = State.Finished;
        }
    }
}