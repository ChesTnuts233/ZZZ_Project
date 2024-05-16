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
		public string Title;


		/// <summary>
		/// 所属的目录路径
		/// </summary>
		public string DirectoryPath;

		/// <summary>
		/// 所属的目录数据
		/// </summary>
		public DirectoryData DirectoryData;

		public List<URLData> uRLDatas = new List<URLData>();

		public DirectoryTipData(string title, string content)
		{
			Title = title;
			Content = content;
		}

	}
}