using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


public class CodeTemplateManagerWindow : EditorWindow
{
	[SerializeField]
	private VisualTreeAsset m_VisualTreeAsset = default;

	[SerializeField]
	public VisualTreeAsset InspectorVisualTreeAsset = default;

	[SerializeField]
	public VisualTreeAsset ListItemVistalTreeAsset = default;


	#region 数据

	[SerializeField]
	public CodeTemplateDatas Datas;

	private CodeTemplateFactory factory;

	#endregion

	#region 面板元素

	private ListView listView;

	private CodeTemplateInspector inspector;

	private VisualElement upDiv;

	private VisualElement downDiv;

	private VisualElement rightDiv;

	private VisualElement leftDiv;

	#endregion

	private List<CodeTemplateData> selectedListItems = new();




	[MenuItem("KooFrame/代码模板管理")]
	public static void ShowWindow()
	{
		CodeTemplateManagerWindow wnd = GetWindow<CodeTemplateManagerWindow>();
		wnd.titleContent = new GUIContent("代码模板管理");
	}

	private void OnEnable()
	{
		factory = new CodeTemplateFactory();
	}

	public void CreateGUI()
	{
		VisualElement root = rootVisualElement;
		m_VisualTreeAsset.CloneTree(root);
		listView = root.Q<ListView>("CodeDataList");

		CreateCodeDatasListView();


		inspector = root.Q<CodeTemplateInspector>("CodeTemplateInspector");
		inspector.OnBindToManagerWindow(this);


		rightDiv = root.Q<VisualElement>("RightDiv");

		leftDiv = root.Q<VisualElement>("LeftDiv");

	}

	private void OnGUI()
	{

		if (Event.current.button == 1 && leftDiv.contentRect.Contains(Event.current.mousePosition))
		{
			GenericMenu menu = new GenericMenu();
			// 添加菜单项
			CreateTemplateListMenu(menu);
			// 显示菜单
			menu.ShowAsContext();
		}
	}

	private void OnDestroy()
	{
		inspector.OnClose();
	}


	private void CreateTemplateListMenu(GenericMenu menu)
	{
		menu.AddItem(new GUIContent("添加脚本模板"), false, () =>
		{
			//打开创建模板窗口
			CodeTemplateCreateWindow.ShowWindow();

			listView.Rebuild();
		});

		menu.AddItem(new GUIContent("删除"), false, () =>
		{
			foreach (var selectedItem in selectedListItems)
			{
				factory.DeleteData(selectedItem);
			}
			selectedListItems.Clear(); // 删除后清空选中项列表
			listView.Rebuild();
		});

	}

	/// <summary>
	/// 获取所有的模板数据
	/// </summary>
	private void CreateCodeDatasListView()
	{
		if (Datas == null)
		{
			Datas = AssetDatabase.LoadAssetAtPath<CodeTemplateDatas>("Assets/3.Frame/KooFrame/1.FrameEditor/CodeTemplate/Data/CodeTemplateDatas.asset");
		}


		var item = ListItemVistalTreeAsset;
		Func<VisualElement> makeItem = () => item.Instantiate();

		Action<VisualElement, int> bindItem = (visualElement, index) =>
		{
			if (index >= Datas.CodeTemplates.Count || Datas.CodeTemplates[index] == null)
			{
				listView.Remove(visualElement);
				return;
			}

			visualElement.Q<Label>("Name").text = Datas.CodeTemplates[index].Name.Value;

			//当名称变化时 列表名称也变化
			Datas.CodeTemplates[index].Name.OnValueChange += (value) =>
			{
				visualElement.Q<Label>("Name").text = value;
			};
		};



		listView.makeItem = makeItem;
		listView.bindItem = bindItem;
		listView.itemsSource = Datas.CodeTemplates;
		listView.selectionType = SelectionType.Multiple;



		listView.selectionChanged += OnCodeDataSelectChange;

	}

	private void OnCodeDataSelectChange(IEnumerable<object> enumerable)
	{
		selectedListItems = enumerable.OfType<CodeTemplateData>().ToList();


		// 如果有LevelData类型的元素
		if (selectedListItems.Any())
		{
			inspector.UpdateInspector(selectedListItems[0]);
		}
	}








}
