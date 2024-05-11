
using MG.MDV;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

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


}