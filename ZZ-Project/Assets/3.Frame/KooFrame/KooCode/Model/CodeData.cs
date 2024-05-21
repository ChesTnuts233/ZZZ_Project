using System;
using System.Collections.Generic;
using KooFrame.BaseSystem;
using UnityEngine;

namespace KooFrame
{
    [Serializable]
    public class CodeData
    {
        /// <summary>
        /// 数据名称
        /// </summary>
        public ModelValue<string> Name = new() { ValueWithoutAction = "DefaultData" };

        /// <summary>
        /// 源码文件
        /// </summary>
        public TextAsset CodeFile;

        public string Content;

        /// <summary>
        /// 数据ID
        /// </summary>
        public string ID;

        /// <summary>
        /// 这个数据拥有的Tag
        /// </summary>
        public List<string> Tags;

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