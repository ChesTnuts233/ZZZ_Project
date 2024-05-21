using System;
using System.Collections.Generic;

namespace KooFrame
{
    [Serializable]
    public class DirectoryData : UMLTree
    {
        /// <summary>
        /// 数据对应的目录路径
        /// </summary>
        public string DirectoryPath;

        /// <summary>
        /// 目录的标题
        /// </summary>
        public string Title;

        /// <summary>
        /// 目录的描述
        /// </summary>
        public string Description;

        /// <summary>
        /// 提示数据
        /// </summary>
        public List<DirectoryTipData> TipsDatas = new();

        public Serialized_Dic<CodeData, string> PathDic = new();
        
        
        
        
    }
}