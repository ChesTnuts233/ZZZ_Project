//****************** 代码文件申明 ************************
//* 文件：BehaviourBase                                       
//* 作者：Koo
//* 创建时间：2024/04/22 21:44:56 星期一
//* 功能：行为基类
//*****************************************************

namespace GameBuild
{
    public abstract class BehaviourBase
    {
        public BehaviourBase() { }

        public abstract void Action(int actionID, bool execute = true);

        public abstract void ActionWithData(int actionID, params object[] data);
        
        
        
    }
}