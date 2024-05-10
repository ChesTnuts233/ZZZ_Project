using KooFrame;
using System;
using UnityEditor;
using UnityEngine;
/// <summary>
/// 代码模板工厂
/// </summary>
public class CodeTemplateFactory : BaseCodeFactory<CodeTemplateData>
{

	public Action OnCreateOrAddData;
	public Action OnDeleteData;

	public override void AddData(CodeTemplateData data)
	{
		Datas.CodeTemplates.Add(data);
		OnCreateOrAddData?.Invoke();
		UpdateAndSave();
	}


	public override CodeTemplateData CreateData()
	{
		return CreateData("DefaultTemplate");
	}

	/// <summary>
	/// 创建脚本模板
	/// </summary>
	/// <returns>模板数据</returns>
	public CodeTemplateData CreateData(string name)
	{
		CodeTemplateData createDate = new CodeTemplateData(name);
		Datas.CodeTemplates.Add(createDate);

		OnCreateOrAddData?.Invoke();

		UpdateAndSave();
		return createDate;
	}

	/// <summary>
	/// 创建脚本模板
	/// </summary>
	/// <param name="content"></param>
	/// <param name="sourcefile"></param>
	/// <param name="name"></param>
	public CodeTemplateData CreateData(string content, TextAsset sourcefile, string name = "DefaultTemplate")
	{
		CodeTemplateData createDate = new CodeTemplateData(name, content, sourcefile);
		Datas.CodeTemplates.Add(createDate);

		OnCreateOrAddData?.Invoke();

		UpdateAndSave();
		return createDate;
	}



	public override void DeleteData(CodeTemplateData data)
	{
		Datas.CodeTemplates.Remove(data);

		OnDeleteData?.Invoke();

		UpdateAndSave();
	}

	private void UpdateAndSave()
	{
		//更新MenuItem
		UpdateTemplateMenuItem();

		EditorUtility.SetDirty(Datas);
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
