//****************** 代码文件申明 ************************
//* 文件：RequestNode                                       
//* 作者：Koo
//* 创建时间：2024/04/22 23:05:08 星期一
//* 功能：nothing
//*****************************************************

using System;

namespace GameBuild
{
    public class RequestNode : RequestBase
    {
        public RequestNode(BehaviourBase behaviour, int actionID, int lifeSteps, Func<bool> exeCondition) : base(
            behaviour, actionID, lifeSteps, exeCondition) { }

        protected override void OnRequestAction()
        {
            behaviour.Action(actionID, true);
        }

        protected override void OnRequestOver()
        {
            behaviour.Action(actionID, false);
        }
    }
}