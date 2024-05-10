using KooFrame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
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

	private ListView codeMarkListView;

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

		if (Datas == null)
		{
			Datas = AssetDatabase.LoadAssetAtPath<KooCodeDatas>(CodeDatasPath);
		}
	}

	public void CreateGUI()
	{
		VisualElement root = rootVisualElement;
		selfVisualTreeAsset.CloneTree(root);

		BindDiv(root);

		BindListView(root);

		BindListShowBtn(root);

		BindInspector(root);

		CreateCodeDataListView();

		CreateCodeTemplateListView();

		RegisterDivDragAndDrop(codeTemplateListView);
	}

	private void OnGUI()
	{

		//if (Event.current.button == 1 && leftDiv.contentRect.Contains(Event.current.mousePosition))
		//{
		//	GenericMenu menu = new GenericMenu();
		//	// 添加菜单项
		//	CreateTemplateListMenu(menu);
		//	// 显示菜单
		//	menu.ShowAsContext();
		//}


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
		codeMarkListView = root.Q<ListView>("CodeDataList");
		BindListViewRightClick(codeMarkListView, CreateMarkListMenu);
		codeTemplateListView = root.Q<ListView>("CodeTemplateList");
		BindListViewRightClick(codeTemplateListView, CreateTemplateListMenu);
		methodDataListView = root.Q<ListView>("MethodTemplateList");
	}


	private void BindListViewRightClick(VisualElement element, Action clickAction)
	{
		element.RegisterCallback<PointerDownEvent>(evt =>
		{
			// 检查是否是右键点击
			if (evt.button == PointerEventData.InputButton.Right.ToInt())
			{
				// 获取点击的UI元素
				VisualElement clickedElement = evt.target as VisualElement;
				// 添加菜单项
				clickAction?.Invoke();
			}
		});
	}

	private void BindListShowBtn(VisualElement root)
	{
		codeDataShowBtn = root.Q<Button>("CodeDataShowBtn");
		codeTemplateShowBtn = root.Q<Button>("CodeTemplateShowBtn");
		methodShowBtn = root.Q<Button>("MethodShowBtn");

		codeDataShowBtn.clicked += () => ShowListView(codeMarkListView);
		codeTemplateShowBtn.clicked += () => ShowListView(codeTemplateListView);
		methodShowBtn.clicked += () => ShowListView(methodDataListView);

		void ShowListView(ListView showListView)
		{
			codeMarkListView.style.display = DisplayStyle.None;
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
	private void CreateTemplateListMenu()
	{
		GenericMenu menu = new GenericMenu();
		menu.AddItem(new GUIContent("添加脚本模板"), false, () =>
		{
			//打开创建模板窗口
			CodeTemplateCreateWindow.ShowWindow();

			//codeTemplateListView.Rebuild();
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
		// 显示菜单
		menu.ShowAsContext();
	}


	private void CreateMarkListMenu()
	{
		GenericMenu menu = new GenericMenu();
		menu.AddItem(new GUIContent("添加代码笔记"), false, () =>
		{
			////打开创建模板窗口
			//CodeTemplateCreateWindow.ShowWindow();
			//codeTemplateListView.Rebuild();
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
		// 显示菜单
		menu.ShowAsContext();
	}




	/// <summary>
	/// 获取所有的模板数据
	/// </summary>
	private void CreateCodeTemplateListView()
	{
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


	private void CreateCodeDataListView()
	{
		CreateListView(codeMarkListView, DataListItemVistalTreeAsset, Datas.Codes);
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
			inspectorManager.UpdateInspector(selectedListItems[0]);
		}
	}








}
