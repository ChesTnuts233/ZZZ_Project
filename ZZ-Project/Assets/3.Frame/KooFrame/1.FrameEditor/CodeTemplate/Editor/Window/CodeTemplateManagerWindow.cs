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


	public static string CodeDatasPath = "Assets/3.Frame/KooFrame/1.FrameEditor/CodeTemplate/Data/CodeTemplateDatas.asset";




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

		BindDiv(root);

		BindInspector(root);

		CreateCodeDatasListView(root);
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
		inspector?.OnClose();
	}

	private void BindDiv(VisualElement root)
	{
		upDiv = root.Q<VisualElement>("UpDiv");
		downDiv = root.Q<VisualElement>("DownDiv");
		rightDiv = root.Q<VisualElement>("RightDiv");
		leftDiv = root.Q<VisualElement>("LeftDiv");
	}


	private void BindInspector(VisualElement root)
	{
		inspector = root.Q<CodeTemplateInspector>("CodeTemplateInspector");
		inspector.OnBindToManagerWindow(this);
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
	private void CreateCodeDatasListView(VisualElement root)
	{
		listView = root.Q<ListView>("CodeDataList");
		if (Datas == null)
		{
			Datas = AssetDatabase.LoadAssetAtPath<CodeTemplateDatas>(CodeDatasPath);
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

		//当创建数据的时候 刷新ListView
		factory.OnCreateData += () =>
		{
			UpdateListView();
		};

		factory.OnDeleteData += () =>
		{
			UpdateListView();
		};

		UpdateListView();
	}


	private void UpdateListView()
	{
		if (listView != null)
		{
			listView.Rebuild();
			//列表空了
			if (listView.itemsSource.Count == 0)
			{
				rightDiv.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
			}
			else
			{
				rightDiv.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
			}
		}
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
