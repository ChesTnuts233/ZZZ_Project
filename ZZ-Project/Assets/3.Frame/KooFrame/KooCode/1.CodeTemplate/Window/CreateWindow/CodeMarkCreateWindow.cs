using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


namespace KooFrame
{
	public class CodeMarkCreateWindow : CodeCreateWindowBase
	{
		private CodeMarkFactory factory;

		/// <summary>
		/// 被创建的Data
		/// </summary>
		private CodeMarkData curCreatedData;


		#region 元素

		private Button createNameBtn;

		private ObjectField createMarkDown;

		private TextField createDataName;

		#endregion

		public static CodeMarkCreateWindow ShowWindow()
		{
			CodeMarkCreateWindow wnd = GetWindow<CodeMarkCreateWindow>();
			wnd.titleContent = new GUIContent("创建代码笔记");
			return wnd;
		}

		private void OnEnable()
		{
			factory = new CodeMarkFactory();
			Datas = factory.Datas;
			curCreatedData = new();
			curCreatedData.Name.SetValueWithoutAction("DefaultMark");
		}

		protected override void CreateGUI()
		{
			base.CreateGUI();

			BindNameInput();
			BindMarkDownField();
			BindButton();
		}

		private void BindNameInput()
		{
			createDataName = Root.Q<TextField>("CreateDataName");
			createDataName.RegisterValueChangedCallback((value) =>
			{
				curCreatedData.Name.SetValueWithoutAction(value.newValue);
			});

		}

		private void BindButton()
		{
			createNameBtn = Root.Q<Button>("CreateNameBtn");
			createNameBtn.clicked += CreateData;
		}

		private void BindMarkDownField()
		{
			createMarkDown = Root.Q<ObjectField>("CreateMarkDown");
			createMarkDown.RegisterValueChangedCallback(value =>
			{
				curCreatedData.CodeMarkDown = value.newValue as TextAsset;
				curCreatedData.MarkDownPath = AssetDatabase.GetAssetPath(curCreatedData.CodeMarkDown);
			});
		}

		private void CreateData()
		{
			if (!CheckDataNameLedge(curCreatedData))
			{
				return;
			}


			factory.AddData(curCreatedData);
			CurEditorListView?.Rebuild();
			Close();
		}

	}
}
