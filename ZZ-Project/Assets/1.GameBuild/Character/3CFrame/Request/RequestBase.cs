//****************** 代码文件申明 ************************
//* 文件：RequestBase                                       
//* 作者：Koo
//* 创建时间：2024/04/22 21:42:01 星期一
//* 功能：请求基类
//*****************************************************

using System;

namespace GameBuild
{
    public enum RequestState
    {
        back = 1,
        fore = 1 << 1,
        over = 1 << 2,
    }

    public abstract class RequestBase
    {
        public int actionID { get; protected set; }
        public int requestID { get; protected set; }

        /// <summary>
        /// 执行条件
        /// </summary>
        public Func<bool> exeCondition;

        protected RequestState requestState;

        protected BehaviourBase behaviour;

        protected int requestLifeSteps;

        protected int requestCount;

        public RequestBase(BehaviourBase behaviour, int actionID, int lifeSteps, Func<bool> exeCondition)
        {
            this.behaviour = behaviour;
            this.actionID = actionID;
            this.requestCount = lifeSteps;
            this.exeCondition = exeCondition;
        }

        protected virtual void OnRequestAction() { }

        protected virtual void OnRequestOver() { }

        public virtual bool RequestUpdate()
        {
            requestCount++;
            if (requestCount >= requestLifeSteps)
            {
                return true;
            }

            return false;
        }

        public void RequestAction()
        {
            requestCount = 0;
            OnRequestAction();
        }

        public void RequestOver()
        {
            requestCount = 0;
            OnRequestOver();
        }

        public virtual void ExternalInit(params object[] data)
        {
            
        }
    }
}