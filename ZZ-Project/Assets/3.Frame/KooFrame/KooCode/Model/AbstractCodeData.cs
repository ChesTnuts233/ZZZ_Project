//****************** 代码文件申明 ************************
//* 文件：AbstractCodeData                                       
//* 作者：Koo
//* 创建时间：2024/06/09 18:30:17 星期日
//* 功能：nothing
//*****************************************************

using System.Collections.Generic;

namespace KooFrame
{
    public abstract class AbstractCodeData
    {
        /// <summary>
        /// 数据名称
        /// </summary>
        public ModelValue<string> Name = new() { ValueWithoutAction = "DefaultData" };

        public string Content;
        
        /// <summary>
        /// 数据ID
        /// </summary>
        public string ID;

        /// <summary>
        /// 这个数据拥有的Tag
        /// </summary>
        public List<string> Tags;
    }
}