using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace KooFrame
{

	public class CodeDataCreateWindow : CodeCreateWindowBase
	{


		private CodeData curCreateData;

		public CodeData CurCreateTemplateData
		{
			get => curCreateData;

			set
			{
				curCreateData = value;
				nameField?.SetValueWithoutNotify(value.Name);
				codeTemplateFile?.SetValueWithoutNotify(value.CodeFile);
			}
		}

		private CodeDataFactory factory;




		#region 页面元素

		private TextField nameField;

		private ObjectField codeTemplateFile;

		private Button createBtn;

		#endregion

		public static CodeDataCreateWindow ShowWindow()
		{
			CodeDataCreateWindow wnd = GetWindow<CodeDataCreateWindow>();
			wnd.titleContent = new GUIContent("创建脚本模板");
			return wnd;
		}



		private void OnEnable()
		{
			factory = new();
			Datas = factory.Datas;
		}


		protected override void CreateGUI()
		{
			base.CreateGUI();

			BindNameField(Root);

			BindTextAssetField(Root);

			BindCreateBtn(Root);

			nameField.Focus();

			curCreateData = new();
		}


		private void BindTextAssetField(VisualElement root)
		{
			codeTemplateFile = root.Q<ObjectField>("CodeTemplateFile");

			codeTemplateFile.RegisterValueChangedCallback((value) =>
			{
				curCreateData.CodeFile = value.newValue as TextAsset;

				nameField.text.IsNullOrWhitespace().Log();

				if (nameField.text.IsNullOrWhitespace())
				{
					nameField.SetValueWithoutNotify(value.newValue.name);
				}

				curCreateData.UpdateData();
			});
		}


		private void BindNameField(VisualElement root)
		{
			nameField = root.Q<TextField>("NameInput");
			nameField.SetValueWithoutNotify("DefaultTemplate");
			nameField.RegisterValueChangedCallback((value) =>
			{
				curCreateData.Name.ValueWithoutAction = value.newValue;
			});
		}

		private void BindCreateBtn(VisualElement root)
		{
			createBtn = root.Q<Button>("CreateBtn");

			createBtn.clicked += CreateCodeData;
		}

		private void CreateCodeData()
		{
			//检查名称中是否有非法符号
			if (!CheckDataNameLedge(curCreateData))
			{
				return;
			}

			//创建模板数据
			factory.AddData(curCreateData);

			if (CurEditorListView != null)
			{
				CurEditorListView.Rebuild();
			}

			////创建模板文件
			//CreateTXTTemplateFile();
			//UpdateTemplateMenuItem();

			Close();
		}





		/// <summary>
		/// 创建模板文件
		/// </summary>
		private void CreateTXTTemplateFile()
		{

		}



	}
}