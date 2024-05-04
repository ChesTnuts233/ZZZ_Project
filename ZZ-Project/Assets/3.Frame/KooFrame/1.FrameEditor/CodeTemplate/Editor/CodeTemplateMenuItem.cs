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

#endregion 代码生成结束标识
}
