using KooFrame;
using System;
using UnityEngine;

[Serializable]
public class CodeTemplateData
{
	/// <summary>
	/// 模板名称
	/// </summary>
	public ModelValue<string> Name = new() { ValueWithoutAction = "DefaultTemplate" };


	/// <summary>
	/// 模板ID
	/// </summary>
	public string ID;


	/// <summary>
	/// 模板内容
	/// </summary>
	public string CodeContent;

	[SerializeField]
	public TextAsset CodeTemplateFile;


	/// <summary>
	/// 根据TextAsset编辑
	/// </summary>
	public void UpdateCodeContent()
	{
		if (CodeTemplateFile != null)
		{
			CodeContent = CodeTemplateFile.text;
		}

		Name.Value = CodeTemplateFile.name;

	}




}
