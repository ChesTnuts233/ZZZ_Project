using KooFrame;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CodeTemplateDatas", menuName = "SO/CodeTemplateDatas")]
public class CodeTemplateDatas : ConfigBase_SO
{
	/// <summary>
	/// 脚本模板数据集合
	/// </summary>
	public List<CodeTemplateData> CodeTemplates = new();

}
