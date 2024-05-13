//****************** 代码文件申明 ************************
//* 文件：CodeMethodData                                       
//* 作者：Koo
//* 创建时间：2024/05/13 20:57:23 星期一
//* 描述：方法数据
//*****************************************************

using System;
using System.Collections.Generic;

namespace KooFrame
{
	[Serializable]
	public class CodeMethodData : CodeData
	{
		/// <summary>
		/// 被使用的代码文件路径
		/// </summary>
		public List<string> UsedPath = new();


		/// <summary>
		/// 方法内容
		/// </summary>
		public string Content;
	}
}
