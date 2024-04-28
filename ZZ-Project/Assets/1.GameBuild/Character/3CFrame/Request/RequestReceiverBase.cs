//****************** 代码文件申明 ************************
//* 文件：RequestReceiverBase                                       
//* 作者：Koo
//* 创建时间：2024/04/22 23:08:01 星期一
//* 功能：nothing
//*****************************************************

using System.Collections.Generic;
using UnityEngine;

namespace GameBuild
{
    interface IRequestReceiver
    {
        public void ReceiverRequest(int requestID);
    }

    public class RequestReceiverBase : IRequestReceiver
    {
        protected Dictionary<int, RequestBase> requestDic;
        protected RequestHandleBase requestHandle;
        protected BehaviourBase behaviour;

        public void ReceiverRequest(int requestID)
        {
            if (requestDic.ContainsKey(requestID))
            {
                //接受请求
            }
            else
            {
                Debug.LogWarning("没有对应ID的请求，ID为：" + requestID.ToString());
            }
        }

        public virtual void ReceiverRequestWithData(int requestID, params object[] data) { }

        public void RegisterRequest(int requestID, RequestBase request)
        {
            requestDic[requestID] = request;
        }

        public void RemoveRequest(int requestID)
        {
            if (requestDic.ContainsKey(requestID))
            {
                requestDic.Remove(requestID);
            }
        }
    }
}