using System;
using System.Collections.Generic;
using System.Linq;
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

	private CodeTemplateInspector inspector;

	private VisualElement rightDiv;

	private VisualElement leftDiv;

	#endregion


	[MenuItem("KooFrame/代码模板管理")]
	public static void ShowWindow()
	{
		CodeTemplateManagerWIndow wnd = GetWindow<CodeTemplateManagerWIndow>();
		wnd.titleContent = new GUIContent("代码模板管理");
	}



	public void CreateGUI()
	{
		VisualElement root = rootVisualElement;
		m_VisualTreeAsset.CloneTree(root);
		listView = root.Q<ListView>("CodeDataList");
		CreateCodeDatasListView();


		inspector = root.Q<CodeTemplateInspector>("CodeTemplateInspector");

		rightDiv = root.Q<VisualElement>("RightDiv");

		leftDiv = root.Q<VisualElement>("LeftDiv");
	}

	private void OnGUI()
	{
		if (Event.current.type == EventType.MouseDown &&
				Event.current.button == 1 && leftDiv.contentRect.Contains(Event.current.mousePosition))
		{
			GenericMenu menu = new GenericMenu();
			// 添加菜单项

			// 显示菜单
			menu.ShowAsContext();
		}
	}

	private void CreateTemplateListMenu(GenericMenu menu)
	{
		menu.AddItem(new GUIContent("添加脚本模板"), false, () =>
		{
			//打开创建模板窗口
		});

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

		listView.selectionChanged += OnCodeDataSelectChange;
	}

	private void OnCodeDataSelectChange(System.Collections.Generic.IEnumerable<object> enumerable)
	{
		List<CodeTemplateData> DataList = enumerable.OfType<CodeTemplateData>().ToList();
		// 如果有LevelData类型的元素
		if (DataList.Any())
		{
			inspector.UpdateInspector(DataList[0]);
		}
	}


}
