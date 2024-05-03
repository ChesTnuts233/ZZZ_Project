using UnityEditor;
/// <summary>
/// 代码模板工厂
/// </summary>
public class CodeTemplateFactory
{
	private CodeTemplateDatas codeTemplateDatas;

	public CodeTemplateFactory()
	{
		codeTemplateDatas = AssetDatabase.LoadAssetAtPath<CodeTemplateDatas>("Assets/3.Frame/KooFrame/1.FrameEditor/CodeTemplate/Data/CodeTemplateDatas.asset");
	}

	/// <summary>
	/// 创建脚本模板
	/// </summary>
	/// <returns>模板数据</returns>
	public CodeTemplateData CreateData()
	{
		CodeTemplateData createDate = new CodeTemplateData();
		codeTemplateDatas.CodeTemplates.Add(createDate);

		UpdateTemplateMenuItem();


		EditorUtility.SetDirty(codeTemplateDatas);
		AssetDatabase.SaveAssets();
		return createDate;
	}

	private void UpdateTemplateMenuItem()
	{
		//遍历所有的codeTemplateDatas，根据名称生成对应的MenuItem
		string updateContent = "";
		foreach (CodeTemplateData data in codeTemplateDatas.CodeTemplates)
		{
			updateContent += "\t\t[MenuItem(\"Assets/KooFrame/C# DefaultMonoScripts 完整架构模板\", false, 0)]\r\n\t\tpublic static void CreateDefaultMonoScripts()\r\n\t\t{\r\n\t\t\tKooScriptsTemplates.CreateMyScript(\"DefaultMonoScripts.cs\",\r\n\t\t\t\tTemplatesPath + \"/04-KooFrame__MonoBehaviour 完整架构模板.cs.txt\");\r\n\t\t}";
		}





		////生成右键菜单选项
		//KooTool.CodeGenerator_ByTag<CodeTemplateMenuItem>();
	}


	public void DeleteData(CodeTemplateData data)
	{
		codeTemplateDatas.CodeTemplates.Remove(data);

		EditorUtility.SetDirty(codeTemplateDatas);
		AssetDatabase.SaveAssets();
	}

}
