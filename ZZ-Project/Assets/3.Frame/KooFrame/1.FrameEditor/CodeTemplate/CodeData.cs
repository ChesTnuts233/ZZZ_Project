
using KooFrame;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace CodeTemplate
{
	[Serializable]
	public class CodeData
	{
		/// <summary>
		/// 模板名称
		/// </summary>
		public ModelValue<string> Name = new() { ValueWithoutAction = "DefaultTemplate" };

		[SerializeField]
		public TextAsset codeMD;

		[FilePath]
		public string MarkDownPath;



	}
}