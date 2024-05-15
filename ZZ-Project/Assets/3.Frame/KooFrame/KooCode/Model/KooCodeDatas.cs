using System.Collections.Generic;
using UnityEngine;

namespace KooFrame
{
	[CreateAssetMenu(fileName = "CodeDatas", menuName = "SO/CodeDatas")]
	public class KooCodeDatas : ConfigBase_SO
	{
		/// <summary>
		/// 脚本笔记数据集合
		/// </summary>
		public List<CodeMarkData> CodeMarks = new();

		/// <summary>
		/// 脚本模板数据集合
		/// </summary>
		public List<CodeTemplateData> CodeTemplates = new();

		/// <summary>
		/// 目录提示集合
		/// </summary>
		public List<DirectoryTipData> DirectoryTips = new();


		public string GetCodeContentByDataName(string name)
		{
			string content = "";

			CodeTemplateData sameNameData = CodeTemplates.Find(data => data.Name.Value == name);

			//取第一个
			content = sameNameData.CodeContent;

			return content;
		}

	}
}
