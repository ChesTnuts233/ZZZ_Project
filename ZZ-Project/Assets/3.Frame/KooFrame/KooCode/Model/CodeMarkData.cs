using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace KooFrame
{
	[Serializable]
	public class CodeMarkData : CodeData
	{
		/// <summary>
		/// MarkDown
		/// </summary>
		[SerializeField]
		public TextAsset codeMD;

		[FilePath]
		public string MarkDownPath;


		public string MarkDownContent
		{
			get => Content;
			set => Content = value;
		}


	}
}