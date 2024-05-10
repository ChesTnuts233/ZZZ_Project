
using Sirenix.OdinInspector;
using UnityEngine;

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