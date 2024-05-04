using KooFrame.BaseSystem;
using UnityEditor;

public static class CodeTemplateMenuItem
{
	private static CodeTemplateDatas datas;

	public static CodeTemplateDatas Datas
	{
		get
		{
			if (datas == null)
			{
				datas = AssetDatabase.LoadAssetAtPath<CodeTemplateDatas>(CodeTemplateManagerWindow.CodeDatasPath);
			}
			return datas;
		}
	}


	#region 代码生成开始标识
	[MenuItem("Assets/KooFrame-脚本/CodeTemplateFactory", false, 0)]
	public static void CreateCodeTemplateFactoryScripts()
	{
		ScriptsTemplatesCreater.CreateScriptByContent("CodeTemplateFactory",
			Datas.GetCodeContentByDataName("CodeTemplateFactory"));
	}
	[MenuItem("Assets/KooFrame-脚本/AEAudioTrack", false, 0)]
	public static void CreateAEAudioTrackScripts()
	{
		ScriptsTemplatesCreater.CreateScriptByContent("AEAudioTrack",
			Datas.GetCodeContentByDataName("AEAudioTrack"));
	}
	[MenuItem("Assets/KooFrame-脚本/CodeTemplateInspector", false, 0)]
	public static void CreateCodeTemplateInspectorScripts()
	{
		ScriptsTemplatesCreater.CreateScriptByContent("CodeTemplateInspector",
			Datas.GetCodeContentByDataName("CodeTemplateInspector"));
	}
	[MenuItem("Assets/KooFrame-脚本/Test", false, 0)]
	public static void CreateTestScripts()
	{
		ScriptsTemplatesCreater.CreateScriptByContent("Test",
			Datas.GetCodeContentByDataName("Test"));
	}

#endregion 代码生成结束标识
}
