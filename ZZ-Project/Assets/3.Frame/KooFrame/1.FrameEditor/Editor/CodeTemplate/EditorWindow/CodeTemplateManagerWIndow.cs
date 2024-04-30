using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CodeTemplateManagerWIndow : EditorWindow
{
	[SerializeField]
	private VisualTreeAsset m_VisualTreeAsset = default;


	#region 数据

	private CodeTemplateDatas datas;

	#endregion

	#region 面板元素

	private ListView listView;

	#endregion


	[MenuItem("KooFrame/代码模板管理")]
	public static void ShowWindow()
	{
		CodeTemplateManagerWIndow wnd = GetWindow<CodeTemplateManagerWIndow>();
		wnd.titleContent = new GUIContent("代码模板管理");
	}



	public void CreateGUI()
	{
		// Each editor window contains a root VisualElement object
		VisualElement root = rootVisualElement;

		m_VisualTreeAsset.CloneTree(root);
		listView = root.Q<ListView>("CodeDataList");
		CreateCodeDatasListView();
	}


	/// <summary>
	/// 获取所有的模板数据
	/// </summary>
	private void CreateCodeDatasListView()
	{
		//datas = ResSystem.LoadAsset<CodeTemplateDatas>("CodeTemplateDatas");
		datas = AssetDatabase.LoadAssetAtPath<CodeTemplateDatas>("Assets/3.Frame/KooFrame/1.FrameEditor/Editor/CodeTemplate/Data/CodeTemplateDatas.asset");

		var item = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
			"Assets/3.Frame/KooFrame/1.FrameEditor/Editor/CodeTemplate/EditorWindow/CodeTemplateListItem.uxml");
		Func<VisualElement> makeItem = () => item.Instantiate();

		Action<VisualElement, int> bindItem = (visualElement, index) =>
		{
			if (index >= datas.CodeTemplates.Count || datas.CodeTemplates[index] == null)
			{
				listView.Remove(visualElement);
				return;
			}


			visualElement.Q<Label>("Name").text = datas.CodeTemplates[index].Name;
		};



		listView.makeItem = makeItem;
		listView.bindItem = bindItem;
		listView.itemsSource = datas.CodeTemplates;
		listView.selectionType = SelectionType.Multiple;
	}


}
