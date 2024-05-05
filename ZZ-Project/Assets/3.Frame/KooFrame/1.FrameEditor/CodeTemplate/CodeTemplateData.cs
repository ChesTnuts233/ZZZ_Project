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

		if (Name.Value.IsNullOrWhitespace())
		{
			Name.Value = CodeTemplateFile.name;
		}
	}

	public CodeTemplateData()
	{

	}
	public CodeTemplateData(string name)
	{
		Name = new() { ValueWithoutAction = name };
	}

	public CodeTemplateData(string name, string content, TextAsset sourceFile)
	{
		this.Name.ValueWithoutAction = name;
		this.CodeContent = content;
		this.CodeTemplateFile = sourceFile;


	}

}
