using System;
using System.Collections.Generic;

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
		/// 数据ID
		/// </summary>
		public string ID;

		/// <summary>
		/// 这个数据拥有的Tag
		/// </summary>
		public List<string> Tags;

		public virtual void UpdateData()
		{

		}

	}
}

