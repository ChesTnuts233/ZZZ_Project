using System;
using System.Collections.Generic;
using KooFrame.BaseSystem;
using UnityEngine;

namespace KooFrame
{
    [Serializable]
    public class CodeData : AbstractCodeData
    {
        /// <summary>
        /// 源码文件
        /// </summary>
        public TextAsset CodeFile;
        
       

        /// <summary>
        /// 使用了这个模板的路径
        /// </summary>
        public List<string> usePath;


        public CodeData() { }

        public CodeData(string name)
        {
            Name.SetValueWithoutAction(name);
        }

        public CodeData(string name, string content, TextAsset sourceFile)
        {
            Name.SetValueWithoutAction(name);
            Content = content;
            CodeFile = sourceFile;
        }

        public virtual void UpdateData() { }

        /// <summary>
        /// 创建模版到对应的文件
        /// </summary>
        public void CreateFileToPath(string name, string path)
        {
            ScriptsTemplatesCreater.CreateScriptByContentAndPath(name, path, Content);
            string targetFilePath = path + "/" + name + ".cs";

            ScriptsTemplatesCreater.AddDicInDirectoryData(this, targetFilePath);
        }
    }
}