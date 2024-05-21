using KooFrame;
using KooFrame.BaseSystem;
using UnityEditor;

public static class CodeTemplateMenuItem
{
	private static KooCodeDatas Datas => KooCode.Datas;


	#region 代码生成开始标识
	[MenuItem("Assets/KooFrame-脚本/UIPanel", false, 0)]
	public static void CreateUIPanelScripts()
	{
		ScriptsTemplatesCreater.CreateScriptByContent("UIPanel",
			Datas.GetTemplateContentByDataName("UIPanel"));
	}
	[MenuItem("Assets/KooFrame-脚本/CSharp_Class", false, 0)]
	public static void CreateCSharp_ClassScripts()
	{
		ScriptsTemplatesCreater.CreateScriptByContent("CSharp_Class",
			Datas.GetTemplateContentByDataName("CSharp_Class"));
	}
	[MenuItem("Assets/KooFrame-脚本/CustomVisualElement", false, 0)]
	public static void CreateCustomVisualElementScripts()
	{
		ScriptsTemplatesCreater.CreateScriptByContent("CustomVisualElement",
			Datas.GetTemplateContentByDataName("CustomVisualElement"));
	}
	[MenuItem("Assets/KooFrame-脚本/ScriptableObject", false, 0)]
	public static void CreateScriptableObjectScripts()
	{
		ScriptsTemplatesCreater.CreateScriptByContent("ScriptableObject",
			Datas.GetTemplateContentByDataName("ScriptableObject"));
	}
	[MenuItem("Assets/KooFrame-脚本/TestScripts", false, 0)]
	public static void CreateTestScriptsScripts()
	{
		ScriptsTemplatesCreater.CreateScriptByContent("TestScripts",
			Datas.GetTemplateContentByDataName("TestScripts"));
	}
	[MenuItem("Assets/KooFrame-脚本/AnieState", false, 0)]
	public static void CreateAnieStateScripts()
	{
		ScriptsTemplatesCreater.CreateScriptByContent("AnieState",
			Datas.GetTemplateContentByDataName("AnieState"));
	}
	[MenuItem("Assets/KooFrame-脚本/MonoBehaviour", false, 0)]
	public static void CreateMonoBehaviourScripts()
	{
		ScriptsTemplatesCreater.CreateScriptByContent("MonoBehaviour",
			Datas.GetTemplateContentByDataName("MonoBehaviour"));
	}
	[MenuItem("Assets/KooFrame-脚本/testData", false, 0)]
	public static void CreatetestDataScripts()
	{
		ScriptsTemplatesCreater.CreateScriptByContent("testData",
			Datas.GetTemplateContentByDataName("testData"));
	}

#endregion 代码生成结束标识
}
