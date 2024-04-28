//****************** 代码文件申明 ************************
//* 文件：RequestHandleBase                                       
//* 作者：Koo
//* 创建时间：2024/04/22 22:58:54 星期一
//* 功能：nothing
//*****************************************************

using System.Collections.Generic;

namespace GameBuild
{
    public abstract class RequestHandleBase
    {
        protected Queue<RequestBase> foreRequestsQueue;
        protected Queue<RequestBase> backRequestQueue;
        protected BehaviourBase behaviour;

        public RequestHandleBase(BehaviourBase behaviour)
        {
            this.behaviour = behaviour;
            foreRequestsQueue = new Queue<RequestBase>();
            backRequestQueue = new Queue<RequestBase>();
        }

        public abstract void ReceiveRequest(RequestBase request);

        protected virtual void OnUpdate() { }

        protected virtual void OnFixedUpdate() { }

        protected void EnqueueFore(RequestBase request)
        {
            foreRequestsQueue.Enqueue(request);
        }

        protected RequestBase DequeueFore()
        {
            return foreRequestsQueue.Dequeue();
        }

        protected void EnqueueBack(RequestBase request)
        {
            backRequestQueue.Enqueue(request);
        }

        protected RequestBase DequeueBack()
        {
            return backRequestQueue.Dequeue();
        }

        public void Update()
        {
            OnUpdate();
        }

        public void FixedUpdate()
        {
            OnFixedUpdate();
        }
    }
}