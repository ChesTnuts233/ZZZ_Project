using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace KooFrame
{

	public class KooCodeWindow : EditorWindow
	{
		#region 数据

		private KooCodeDatas Datas => KooCode.Datas;
		private CodeAssetsData settingsData => KooCode.AssetsData;

		private CodeDataFactory templateFactory;
		private CodeMarkFactory markFactory;


		private int selectListViewInstance = -1;

		private int selectListView
		{
			get
			{
				selectListViewInstance = EditorPrefs.GetInt("selectList");
				return selectListViewInstance;
			}

			set
			{
				//保存选中的列表实例
				EditorPrefs.SetInt("selectList", value);
				selectListViewInstance = value;
			}
		}

		#endregion

		#region 面板元素

		private ListView codeMarkListView;

		private ListView codeDataListView;

		private ListView methodDataListView;

		private CodeInspectorManager inspectorManager;

		private VisualElement upDiv;

		private VisualElement downDiv;

		private VisualElement rightDiv;

		private VisualElement leftDiv;

		private Button codeDataShowBtn;

		private Button codeTemplateShowBtn;

		private Button methodShowBtn;

		private Button settingsBtn;

		private VisualElement border;

		#endregion

		#region 其他

		private List<CodeData> selectedTemplateListItems = new();

		private List<CodeMarkData> selectedMarkListItems = new();


		private Action onGUICallBack;

		#endregion

		#region 面板生命周期
		[MenuItem("KooFrame/代码模板管理")]
		public static void ShowWindow()
		{
			KooCodeWindow wnd = GetWindow<KooCodeWindow>();
			wnd.titleContent = new GUIContent("代码模板管理");
		}

		private void OnEnable()
		{
			templateFactory = new CodeDataFactory();
			markFactory = new CodeMarkFactory();
		}

		public void CreateGUI()
		{
			VisualElement root = rootVisualElement;
			settingsData.ManagerVisualTreeAsset.CloneTree(root);

			ReadHexoBlog(); //尝试读取Hexo

			//BindBorder(root);

			BindDiv(root);

			BindListView(root);

			BindListShowBtn(root);

			BindInspector(root);

			CreateCodeMarkListView();

			CreateCodeDataListView();

			RegisterDivDragAndDrop(codeDataListView);

			BindSettingsBtn(root);

			//根据保存打开页面
			switch (selectListView)
			{
				case 0:
					ShowListView(codeMarkListView);
					if (KooCode.Datas.CodeMarks.Count > 0)
					{
						inspectorManager.UpdateInspectors(KooCode.Datas.CodeMarks.First());
					}
					break;
				case 1:
					ShowListView(codeDataListView);
					if (KooCode.Datas.CodeTemplates.Count > 0)
					{
						inspectorManager.UpdateInspectors(KooCode.Datas.CodeTemplates.First());
					}
					break;
				case 2:
					ShowListView(methodDataListView);
					//inspectorManager.UpdateInspectors(KooCode.Datas..First());
					break;
				default:
					break;
			}
		}


		private void ReadHexoBlog()
		{
			string path = KooCode.SettingsData.HexoPath;
			if (!path.IsNullOrEmpty())
			{
				//检查路径中的文件
				if (!Directory.Exists(path))
				{
					return;
				}

				//读取目录下所有的md文件
				string[] files = Directory.GetFiles(path, "*.md");

				foreach (string filePath in files)
				{
					//读取文件内容
					string fileContent = File.ReadAllText(filePath);

					string name = Path.GetFileNameWithoutExtension(filePath);

					//如果包含相同名称的数据
					if (KooCode.Datas.CodeMarks.Find(data => data.Name.Value == name) != null)
					{
						continue;
					}

					CodeMarkData newData = new CodeMarkData();

					//创建 TextAsset 对象
					TextAsset textAsset = new TextAsset(fileContent);

					newData.Name.SetValueWithoutAction(name);
					newData.CodeMarkDown = textAsset;
					newData.MarkDownPath = filePath;

					KooCode.Datas.CodeMarks.Add(newData);
				}
			}
		}


		private void BindBorder(VisualElement root)
		{
			border = root.Q<VisualElement>("Border");

			border.RegisterCallback<PointerEnterEvent>((evt) =>
			{
				//Todo 
			});

		}

		private void BindSettingsBtn(VisualElement root)
		{
			settingsBtn = root.Q<Button>("SettingsBtn");
			settingsBtn.clicked += OpenSettingsWindow;
		}

		private void OpenSettingsWindow()
		{
			SettingsWindow.ShowWindow();
		}

		private void OnGUI()
		{
			onGUICallBack?.Invoke();
		}



		private void OnDestroy()
		{
			inspectorManager?.Close();
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
			codeDataListView = root.Q<ListView>("CodeTemplateList");
			BindListViewRightClick(codeDataListView, CreateTemplateListMenu);
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
			codeDataShowBtn.clicked += () => selectListView = 0;

			codeTemplateShowBtn.clicked += () => ShowListView(codeDataListView);
			codeTemplateShowBtn.clicked += () => selectListView = 1;

			methodShowBtn.clicked += () => ShowListView(methodDataListView);
			methodShowBtn.clicked += () => selectListView = 2;
		}

		private void ShowListView(ListView showListView)
		{
			codeMarkListView.style.display = DisplayStyle.None;
			codeDataListView.style.display = DisplayStyle.None;
			methodDataListView.style.display = DisplayStyle.None;
			showListView.style.display = DisplayStyle.Flex;
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
						var window = CodeDataCreateWindow.ShowWindow();
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
				CodeDataCreateWindow window = CodeDataCreateWindow.ShowWindow();
				window.BindListView(codeDataListView);
			});

			menu.AddItem(new GUIContent("删除"), false, () =>
			{
				foreach (var selectedItem in selectedTemplateListItems)
				{
					templateFactory.DeleteData(selectedItem);
				}
				selectedTemplateListItems.Clear(); // 删除后清空选中项列表
				codeDataListView.Rebuild();
			});
			// 显示菜单
			menu.ShowAsContext();
		}


		private void CreateMarkListMenu()
		{
			GenericMenu menu = new GenericMenu();
			menu.AddItem(new GUIContent("添加代码笔记"), false, () =>
			{
				//打开创建模板窗口
				var window = CodeMarkCreateWindow.ShowWindow();
				window.BindListView(codeMarkListView);
			});

			menu.AddItem(new GUIContent("删除"), false, () =>
			{
				foreach (var selectedItem in selectedMarkListItems)
				{
					markFactory.DeleteData(selectedItem);
				}
				selectedTemplateListItems.Clear(); // 删除后清空选中项列表
				codeDataListView.Rebuild();
			});
			// 显示菜单
			menu.ShowAsContext();
		}




		/// <summary>
		/// 获取所有的模板数据
		/// </summary>
		private void CreateCodeDataListView()
		{
			CreateListView(codeDataListView, settingsData.CodeDataListItemVistalTreeAsset, Datas.CodeDatas);

			codeDataListView.bindItem += BindTemplateItem;

			void BindTemplateItem(VisualElement element, int index)
			{
				Label nameLabel = element.Q<Label>("Name");
				if (nameLabel != null)
				{
					nameLabel.text = KooCode.Datas.CodeDatas[index].Name.Value;
					//当名称变化时 列表名称也变化
					KooCode.Datas.CodeDatas[index].Name.OnValueChange += (value) =>
					{
						nameLabel.text = value;
					};
				}


			}


			codeDataListView.selectionChanged += OnCodeTemplateDataSelectChange;


			//当创建数据的时候 刷新ListView
			templateFactory.OnCreateOrAddData += () =>
			{
				UpdateListView(codeDataListView);
			};

			templateFactory.OnDeleteData += () =>
			{
				UpdateListView(codeDataListView);
			};

			UpdateListView(codeDataListView);
		}


		private void CreateCodeMarkListView()
		{
			CreateListView(codeMarkListView, settingsData.MarkDataListItemVistalTreeAsset, Datas.CodeMarks);

			codeMarkListView.bindItem += BindTemplateItem;

			void BindTemplateItem(VisualElement element, int index)
			{
				Label nameLabel = element.Q<Label>("Name");
				if (nameLabel != null)
				{
					nameLabel.text = Datas.CodeMarks[index].Name.Value;
				}

				//当名称变化时 列表名称也变化
				Datas.CodeMarks[index].Name.OnValueChange += (value) =>
				{
					element.Q<Label>("Name").text = value;
				};
			}

			codeMarkListView.selectionChanged += OnCodeMarkDataSelectChange;

			//当创建数据的时候 刷新ListView
			markFactory.OnCreateOrAddData += () =>
			{
				UpdateListView(codeMarkListView);
			};

			markFactory.OnDeleteData += () =>
			{
				UpdateListView(codeMarkListView);
			};

			UpdateListView(codeMarkListView);

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

			createdListView.selectionType = SelectionType.Multiple;
			createdListView.itemsSource = itemsSource;
		}




		#endregion

		private void UpdateListView(ListView view)
		{
			if (view != null)
			{
				view.Rebuild();
				// TODO 列表空了 
				//if (codeTemplateListView.itemsSource.Count == 0)
				//{
				//	rightDiv.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
				//}
				//else
				//{
				//	rightDiv.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
				//}
			}
		}

		private void OnCodeTemplateDataSelectChange(IEnumerable<object> enumerable)
		{
			selectedTemplateListItems = enumerable.OfType<CodeData>().ToList();


			// 如果有LevelData类型的元素
			if (selectedTemplateListItems.Any())
			{
				inspectorManager.UpdateInspectors(selectedTemplateListItems[0]);
			}
		}

		private void OnCodeMarkDataSelectChange(IEnumerable<object> enumerable)
		{
			selectedMarkListItems = enumerable.OfType<CodeMarkData>().ToList();

			// 如果有LevelData类型的元素
			if (selectedMarkListItems.Any())
			{
				inspectorManager.UpdateInspectors(selectedMarkListItems[0]);
			}
		}


	}
}
