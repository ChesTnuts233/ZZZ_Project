//****************** 代码文件申明 ************************
//* 文件：KooActivatorTool                                       
//* 作者：Koo
//* 创建时间：2024/04/22 23:16:56 星期一
//* 功能：nothing
//*****************************************************

using System;

namespace KooFrame
{
    public partial class KooTool
    {
        public static T CreateInstance<T>(Type type)
        {
            T instance = (T)Activator.CreateInstance(type ?? throw new InvalidCastException());
            return instance;
        }

        public static T CreateInstance<T>(Type type, params object[] args)
        {
			T instance = (T)Activator.CreateInstance(type ?? throw new InvalidCastException(), args);
			return instance;
		}
    }
}