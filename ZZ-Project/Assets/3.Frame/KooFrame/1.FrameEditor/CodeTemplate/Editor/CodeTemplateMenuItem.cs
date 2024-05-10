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
	[MenuItem("Assets/KooFrame-脚本/Test1", false, 0)]
	public static void CreateTest1Scripts()
	{
		ScriptsTemplatesCreater.CreateScriptByContent("Test1",
			Datas.GetCodeContentByDataName("Test1"));
	}
	[MenuItem("Assets/KooFrame-脚本/Test2", false, 0)]
	public static void CreateTest2Scripts()
	{
		ScriptsTemplatesCreater.CreateScriptByContent("Test2",
			Datas.GetCodeContentByDataName("Test2"));
	}
	[MenuItem("Assets/KooFrame-脚本/Test3", false, 0)]
	public static void CreateTest3Scripts()
	{
		ScriptsTemplatesCreater.CreateScriptByContent("Test3",
			Datas.GetCodeContentByDataName("Test3"));
	}

#endregion 代码生成结束标识
}
