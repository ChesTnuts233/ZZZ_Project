using System;
using UnityEngine;

[Serializable]
public class CodeTemplateData
{
	/// <summary>
	/// 模板名称
	/// </summary>
	public string Name;

	/// <summary>
	/// 模板ID
	/// </summary>
	public string ID;


	/// <summary>
	/// 模板内容
	/// </summary>
	[TextArea]
	public string CodeContent;







}
