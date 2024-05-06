using KooFrame.BaseSystem;
using UnityEditor;

public static class CodeTemplateMenuItem
{
	private static CodeDatas datas;

	public static CodeDatas Datas
	{
		get
		{
			if (datas == null)
			{
				datas = AssetDatabase.LoadAssetAtPath<CodeDatas>(CodeManagerWindow.CodeDatasPath);
			}
			return datas;
		}
	}


	#region 代码生成开始标识
	[MenuItem("Assets/KooFrame-脚本/MonoBahaviour", false, 0)]
	public static void CreateMonoBahaviourScripts()
	{
		ScriptsTemplatesCreater.CreateScriptByContent("MonoBahaviour",
			Datas.GetCodeContentByDataName("MonoBahaviour"));
	}

#endregion 代码生成结束标识
}
