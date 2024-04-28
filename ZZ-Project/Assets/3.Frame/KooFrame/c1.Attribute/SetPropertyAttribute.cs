
using UnityEngine;

namespace KooFrame
{

	public class SetPropertyAttribute : PropertyAttribute
	{
		public string Name { get; private set; }

		/// <summary>
		/// 是否是脏数据
		/// </summary>
		public bool IsDirty { get; set; }

		public SetPropertyAttribute(string name)
		{
			this.Name = name;
		}
	}
}
