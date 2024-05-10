using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


public class CodeManagerWindow : EditorWindow
{
	#region 排布资源
	[SerializeField]
	private VisualTreeAsset selfVisualTreeAsset = default;

	[SerializeField]
	public VisualTreeAsset TemplateInspectorVisualTreeAsset = default;

	[SerializeField]
	public VisualTreeAsset DataInspectorVisualTreeAsset = default;

	[SerializeField]
	public VisualTreeAsset TemplateListItemVistalTreeAsset = default;

	[SerializeField]
	public VisualTreeAsset DataListItemVistalTreeAsset = default;

	[SerializeField]
	public GUISkin DarkSkin;

	#endregion

	#region 数据

	[SerializeField]
	public KooCodeDatas Datas;

	private CodeTemplateFactory factory;

	#endregion

	#region 面板元素

	private ListView codeDataListView;

	private ListView codeTemplateListView;

	private ListView methodDataListView;

	private CodeInspectorManager inspectorManager;

	private VisualElement upDiv;

	private VisualElement downDiv;

	private VisualElement rightDiv;

	private VisualElement leftDiv;

	private Button codeDataShowBtn;

	private Button codeTemplateShowBtn;

	private Button methodShowBtn;

	#endregion

	#region 其他

	private List<CodeTemplateData> selectedListItems = new();

	public static string CodeDatasPath = "Assets/3.Frame/KooFrame/1.FrameEditor/CodeTemplate/Data/CodeDatas.asset";

	private Action onGUICallBack;

	#endregion

	#region 面板生命周期
	[MenuItem("KooFrame/代码模板管理")]
	public static void ShowWindow()
	{
		CodeManagerWindow wnd = GetWindow<CodeManagerWindow>();
		wnd.titleContent = new GUIContent("代码模板管理");
	}

	private void OnEnable()
	{
		factory = new CodeTemplateFactory();
	}

	public void CreateGUI()
	{
		VisualElement root = rootVisualElement;
		selfVisualTreeAsset.CloneTree(root);

		BindDiv(root);

		BindListView(root);

		BindListShowBtn(root);

		BindInspector(root);

		CreateCodeTemplateListView();

		RegisterDivDragAndDrop(codeTemplateListView);
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


		onGUICallBack?.Invoke();
	}

	private void OnDestroy()
	{
		inspectorManager?.OnClose();
	}

	#endregion

	#region 元素绑定
	private void BindDiv(VisualElement root)
	{
		upDiv = root.Q<VisualElement>("UpDiv");
		downDiv = root.Q<VisualElement>("DownDiv");
		rightDiv = root.Q<VisualElement>("RightDiv");
		leftDiv = root.Q<VisualElement>("LeftDiv");
	}

	private void BindListView(VisualElement root)
	{
		codeDataListView = root.Q<ListView>("CodeDataList");
		codeTemplateListView = root.Q<ListView>("CodeTemplateList");
		methodDataListView = root.Q<ListView>("MethodTemplateList");
	}

	private void BindListShowBtn(VisualElement root)
	{
		codeDataShowBtn = root.Q<Button>("CodeDataShowBtn");
		codeTemplateShowBtn = root.Q<Button>("CodeTemplateShowBtn");
		methodShowBtn = root.Q<Button>("MethodShowBtn");

		codeDataShowBtn.clicked += () => ShowListView(codeDataListView);
		codeTemplateShowBtn.clicked += () => ShowListView(codeTemplateListView);
		methodShowBtn.clicked += () => ShowListView(methodDataListView);

		void ShowListView(ListView showListView)
		{
			codeDataListView.style.display = DisplayStyle.None;
			codeTemplateListView.style.display = DisplayStyle.None;
			methodDataListView.style.display = DisplayStyle.None;
			showListView.style.display = DisplayStyle.Flex;
		}
	}


	private void RegisterDivDragAndDrop(VisualElement element)
	{

		element.RegisterCallback<DragUpdatedEvent>(evt =>
		{
			Rect dropArea = element.contentRect;

			//把鼠标的显示改为Copy的样子
			DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

		});
		element.RegisterCallback<DragExitedEvent>(evt =>
		{
			Rect dropArea = element.contentRect;
			//如果鼠标不在区域内就直接结束
			if (!dropArea.Contains(evt.mousePosition))
			{
				return;
			}

			DragAndDrop.AcceptDrag();

			foreach (object draggedObject in DragAndDrop.objectReferences)
			{
				if (draggedObject is TextAsset textAsset)
				{
					var window = CodeTemplateCreateWindow.ShowWindow();
					window.CurCreateTemplateData = new CodeTemplateData("DefaultTemplate", textAsset.text, textAsset);

				}
			}
		});
	}



	private void BindInspector(VisualElement root)
	{
		inspectorManager = root.Q<CodeInspectorManager>("CodeInspectorManager");
		inspectorManager.BindToManagerWindow(this);
	}

	/// <summary>
	/// 创建右键面板
	/// </summary>
	/// <param name="menu"></param>
	private void CreateTemplateListMenu(GenericMenu menu)
	{
		menu.AddItem(new GUIContent("添加脚本模板"), false, () =>
		{
			//打开创建模板窗口
			CodeTemplateCreateWindow.ShowWindow();

			codeTemplateListView.Rebuild();
		});

		menu.AddItem(new GUIContent("删除"), false, () =>
		{
			foreach (var selectedItem in selectedListItems)
			{
				factory.DeleteData(selectedItem);
			}
			selectedListItems.Clear(); // 删除后清空选中项列表
			codeTemplateListView.Rebuild();
		});

	}

	/// <summary>
	/// 获取所有的模板数据
	/// </summary>
	private void CreateCodeTemplateListView()
	{

		if (Datas == null)
		{
			Datas = AssetDatabase.LoadAssetAtPath<KooCodeDatas>(CodeDatasPath);
		}

		CreateListView(codeTemplateListView, TemplateListItemVistalTreeAsset, Datas.CodeTemplates);

		codeTemplateListView.bindItem += BindTemplateItem;

		void BindTemplateItem(VisualElement element, int index)
		{
			Label nameLabel = element.Q<Label>("Name");
			if (nameLabel != null)
			{
				nameLabel.text = Datas.CodeTemplates[index].Name.Value;
			}

			//当名称变化时 列表名称也变化
			Datas.CodeTemplates[index].Name.OnValueChange += (value) =>
			{
				element.Q<Label>("Name").text = value;
			};
		}


		codeTemplateListView.selectionChanged += OnCodeDataSelectChange;


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


	private void CreateCodeDataListView(VisualElement root)
	{
		if (Datas == null)
		{
			Datas = AssetDatabase.LoadAssetAtPath<KooCodeDatas>(CodeDatasPath);
		}

		CreateListView(codeDataListView, DataListItemVistalTreeAsset, Datas.Codes);


	}


	private void CreateListView(ListView createdListView, VisualTreeAsset item, IList itemsSource)
	{
		Func<VisualElement> makeItem = () => item.Instantiate();

		Action<VisualElement, int> bindItem = (visualElement, index) =>
		{
			if (index >= itemsSource.Count || itemsSource[index] == null)
			{
				createdListView.Remove(visualElement);
				return;
			}

		};

		createdListView.makeItem = makeItem;
		createdListView.bindItem = bindItem;

		codeTemplateListView.selectionType = SelectionType.Multiple;
		codeTemplateListView.itemsSource = itemsSource;
	}




	#endregion

	private void UpdateListView()
	{
		if (codeTemplateListView != null)
		{
			codeTemplateListView.Rebuild();
			//列表空了
			if (codeTemplateListView.itemsSource.Count == 0)
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
			//




			inspectorManager.UpdateInspector(selectedListItems[0]);
		}
	}








}
