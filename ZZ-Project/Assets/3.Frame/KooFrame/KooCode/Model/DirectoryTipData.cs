using System;
using System.Collections.Generic;

namespace KooFrame
{
	[Serializable]
	public class URLData
	{
		public string URLTitle; //超链接标题
		public string URL; //超链接信息
	}

	[Serializable]
	public class DirectoryTipData : CodeData
	{
		public string TipTitle;

		/// <summary>
		/// 标签内容
		/// </summary>
		public string TipContent;

		/// <summary>
		/// 所属的目录路径
		/// </summary>
		public string DirectoryPath;

		public List<URLData> uRLDatas = new List<URLData>();

		public DirectoryTipData(string title, string content)
		{
			TipTitle = title;
			TipContent = content;
		}

	}
}