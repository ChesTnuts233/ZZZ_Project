using KooFrame.BaseSystem;
using System;
using UnityEngine;

namespace KooFrame
{
	[Serializable]
	public class CodeTemplateData : CodeData
	{

		/// <summary>
		/// 模板内容
		/// </summary>
		public string CodeContent;

		[SerializeField]
		public TextAsset CodeTemplateFile;


		/// <summary>
		/// 根据TextAsset编辑
		/// </summary>
		public override void UpdateData()
		{
			if (CodeTemplateFile != null)
			{
				CodeContent = CodeTemplateFile.text;
			}

			if (Name.Value.IsNullOrWhitespace())
			{
				Name.Value = CodeTemplateFile.name;
			}

		}

		public CodeTemplateData()
		{

		}
		public CodeTemplateData(string name)
		{
			Name = new() { ValueWithoutAction = name };
		}

		public CodeTemplateData(string name, string content, TextAsset sourceFile)
		{
			this.Name.ValueWithoutAction = name;
			this.CodeContent = content;
			this.CodeTemplateFile = sourceFile;
		}

		/// <summary>
		/// 创建模版到对应的文件
		/// </summary>
		public void CreateFileToPath(string name, string path)
		{
			ScriptsTemplatesCreater.CreateScriptByContentAndPath(name, path, CodeContent);
		}

	}
}
