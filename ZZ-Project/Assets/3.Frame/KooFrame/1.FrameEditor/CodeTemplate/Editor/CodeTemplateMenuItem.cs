using KooFrame.BaseSystem;
using UnityEditor;

public static class CodeTemplateMenuItem
{
	private static KooCodeDatas datas;

	public static KooCodeDatas Datas
	{
		get
		{
			if (datas == null)
			{
				datas = AssetDatabase.LoadAssetAtPath<KooCodeDatas>(CodeManagerWindow.CodeDatasPath);
			}
			return datas;
		}
	}


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

#endregion 代码生成结束标识
}
