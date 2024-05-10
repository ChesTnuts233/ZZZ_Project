using KooFrame;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CodeCreateWindowBase : EditorWindow
{
	[SerializeField]
	private VisualTreeAsset visualTreeAsset = default;


	protected KooCodeDatas Datas;


	protected VisualElement Root;

	protected ListView CurEditorListView;


	public void BindListView(ListView listView)
	{
		CurEditorListView = listView;
	}

	protected virtual void CreateGUI()
	{
		Root = rootVisualElement;
		visualTreeAsset.CloneTree(Root);
	}



	protected bool CheckDataNameLedge(CodeData checkData)
	{
		bool isHasSameData = false;
		bool isHasLegalChar = false;
		//先看Datas里是否有重名的  重名的不能添加
		foreach (CodeData d in Datas.CodeMarks)
		{
			if (checkData.Name.Value == d.Name.Value)
			{
				isHasSameData = true;
			}
		}
		if (KooTool.IsContainsIllegalCharacters(checkData.Name.Value))
		{
			isHasLegalChar = true;
		}

		if (isHasSameData)
		{
			EditorUtility.DisplayDialog("警告", "已经有相同名称的成员", "确定");
			return false;
		}
		if (isHasLegalChar)
		{
			EditorUtility.DisplayDialog("警告", "名称中含有非法字符", "确定");
			return false;
		}

		return true;
	}

}
