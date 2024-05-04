using KooFrame;
using System;
using UnityEditor;
/// <summary>
/// 代码模板工厂
/// </summary>
public class CodeTemplateFactory
{
	private CodeTemplateDatas codeTemplateDatas;

	public CodeTemplateDatas Datas => codeTemplateDatas;


	public Action OnCreateData;
	public Action OnDeleteData;


	public CodeTemplateFactory()
	{
		codeTemplateDatas = AssetDatabase.LoadAssetAtPath<CodeTemplateDatas>("Assets/3.Frame/KooFrame/1.FrameEditor/CodeTemplate/Data/CodeTemplateDatas.asset");

	}


	public void AddData(CodeTemplateData data)
	{
		codeTemplateDatas.CodeTemplates.Add(data);

		UpdateAndSave();
	}

	/// <summary>
	/// 创建脚本模板
	/// </summary>
	/// <returns>模板数据</returns>
	public CodeTemplateData CreateData(string name = "DefaultTemplate")
	{
		CodeTemplateData createDate = new CodeTemplateData(name);
		codeTemplateDatas.CodeTemplates.Add(createDate);

		OnCreateData?.Invoke();

		UpdateAndSave();
		return createDate;
	}




	public void DeleteData(CodeTemplateData data)
	{
		codeTemplateDatas.CodeTemplates.Remove(data);

		OnDeleteData?.Invoke();

		UpdateAndSave();
	}


	private void UpdateAndSave()
	{
		//更新MenuItem
		UpdateTemplateMenuItem();

		EditorUtility.SetDirty(codeTemplateDatas);
		AssetDatabase.SaveAssets();
	}


	private void UpdateTemplateMenuItem()
	{
		//遍历所有的codeTemplateDatas，根据名称生成对应的MenuItem
		string updateContent = "";
		foreach (CodeTemplateData data in Datas.CodeTemplates)
		{
			updateContent += "\t[MenuItem(\"Assets/KooFrame-脚本/" + data.Name + "\", false, 0)]\r\n\tpublic static void Create" + data.Name + "Scripts()\r\n\t{\r\n\t\tScriptsTemplatesCreater.CreateScriptByContent(\"" + data.Name + "\",\r\n\t\t\tDatas.GetCodeContentByDataName(\"" + data.Name + "\"));\r\n\t}\n";

		}

		//生成右键菜单选项
		KooTool.CodeGenerator_ByTag(updateContent, "Assets/3.Frame/KooFrame/1.FrameEditor/CodeTemplate/Editor/CodeTemplateMenuItem.cs");
	}

}
