using KooFrame;
using KooFrame.BaseSystem;
using UnityEditor;

public static class CodeTemplateMenuItem
{
	private static KooCodeDatas Datas => KooCode.Datas;


	#region 代码生成开始标识
	[MenuItem("Assets/KooFrame-脚本/MonoBehaviour", false, 0)]
	public static void CreateMonoBehaviourScripts()
	{
		ScriptsTemplatesCreater.CreateScriptByContent("MonoBehaviour",
			Datas.GetCodeContentByDataName("MonoBehaviour"));
	}
	[MenuItem("Assets/KooFrame-脚本/UIPanel", false, 0)]
	public static void CreateUIPanelScripts()
	{
		ScriptsTemplatesCreater.CreateScriptByContent("UIPanel",
			Datas.GetCodeContentByDataName("UIPanel"));
	}
	[MenuItem("Assets/KooFrame-脚本/TestScripts", false, 0)]
	public static void CreateTestScriptsScripts()
	{
		ScriptsTemplatesCreater.CreateScriptByContent("TestScripts",
			Datas.GetCodeContentByDataName("TestScripts"));
	}
	[MenuItem("Assets/KooFrame-脚本/CSharp_Class", false, 0)]
	public static void CreateCSharp_ClassScripts()
	{
		ScriptsTemplatesCreater.CreateScriptByContent("CSharp_Class",
			Datas.GetCodeContentByDataName("CSharp_Class"));
	}

	[MenuItem("Assets/KooFrame-脚本/CustomVisualElement", false, 0)]
	public static void CreateCustomVisualElementScripts()
	{
		ScriptsTemplatesCreater.CreateScriptByContent("CustomVisualElement",
			Datas.GetCodeContentByDataName("CustomVisualElement"));
	}

	#endregion 代码生成结束标识
}
