using UnityEditor;
/// <summary>
/// 代码模板工厂
/// </summary>
public class CodeTemplateFactory
{
	private CodeTemplateDatas codeTemplateDatas;

	public CodeTemplateDatas Datas => codeTemplateDatas;

	public CodeTemplateFactory()
	{
		codeTemplateDatas = AssetDatabase.LoadAssetAtPath<CodeTemplateDatas>("Assets/3.Frame/KooFrame/1.FrameEditor/CodeTemplate/Data/CodeTemplateDatas.asset");

	}

	/// <summary>
	/// 创建脚本模板
	/// </summary>
	/// <returns>模板数据</returns>
	public CodeTemplateData CreateData(string name = "DefaultTemplate")
	{
		CodeTemplateData createDate = new CodeTemplateData(name);
		codeTemplateDatas.CodeTemplates.Add(createDate);



		EditorUtility.SetDirty(codeTemplateDatas);
		AssetDatabase.SaveAssets();
		return createDate;
	}

	


	public void DeleteData(CodeTemplateData data)
	{
		codeTemplateDatas.CodeTemplates.Remove(data);

		EditorUtility.SetDirty(codeTemplateDatas);
		AssetDatabase.SaveAssets();
	}

}
