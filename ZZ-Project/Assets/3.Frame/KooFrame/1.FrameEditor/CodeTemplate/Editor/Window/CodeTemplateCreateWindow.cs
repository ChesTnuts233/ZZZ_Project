using KooFrame;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class CodeTemplateCreateWindow : EditorWindow
{
	[SerializeField]
	private VisualTreeAsset m_VisualTreeAsset = default;

	private CodeTemplateData curCreateTemplateData;

	public CodeTemplateData CurCreateTemplateData
	{
		get => curCreateTemplateData;

		set
		{
			curCreateTemplateData = value;
			nameField?.SetValueWithoutNotify(value.Name);
			codeTemplateFile?.SetValueWithoutNotify(value.CodeTemplateFile);
		}
	}

	private CodeTemplateFactory factory;

	private KooCodeDatas datas;

	private ListView templateListView;

	#region 页面元素

	private TextField nameField;

	private ObjectField codeTemplateFile;

	private Button createBtn;

	#endregion

	public static CodeTemplateCreateWindow ShowWindow()
	{
		CodeTemplateCreateWindow wnd = GetWindow<CodeTemplateCreateWindow>();
		wnd.titleContent = new GUIContent("创建脚本模板");
		return wnd;
	}

	public void BindListView(ListView listView)
	{
		templateListView = listView;
	}

	private void OnEnable()
	{
		factory = new();
		datas = factory.Datas;
	}


	private void CreateGUI()
	{
		VisualElement root = rootVisualElement;

		m_VisualTreeAsset.CloneTree(root);

		BindNameField(root);

		BindTextAssetField(root);

		BindCreateBtn(root);

		nameField.Focus();

		curCreateTemplateData = new();
	}


	private void BindTextAssetField(VisualElement root)
	{
		codeTemplateFile = root.Q<ObjectField>("CodeTemplateFile");

		codeTemplateFile.RegisterValueChangedCallback((value) =>
		{
			curCreateTemplateData.CodeTemplateFile = value.newValue as TextAsset;

			nameField.text.IsNullOrWhitespace().Log();

			if (nameField.text.IsNullOrWhitespace())
			{
				nameField.SetValueWithoutNotify(value.newValue.name);
			}

			curCreateTemplateData.UpdateData();
		});
	}


	private void BindNameField(VisualElement root)
	{
		nameField = root.Q<TextField>("NameInput");

		nameField.RegisterValueChangedCallback((value) =>
		{
			curCreateTemplateData.Name.ValueWithoutAction = value.newValue;
		});
	}

	private void BindCreateBtn(VisualElement root)
	{
		createBtn = root.Q<Button>("CreateBtn");

		createBtn.clicked += CreateCodeTempData;
	}

	private void CreateCodeTempData()
	{
		bool isHasSameData = false;
		bool isHasLegalChar = false;
		//先看Datas里是否有重名的  重名的不能添加
		foreach (var data in datas.CodeTemplates)
		{
			if (data.Name.Value == curCreateTemplateData.Name.Value)
			{
				isHasSameData = true;
			}
		}
		if (KooTool.IsContainsIllegalCharacters(curCreateTemplateData.Name.Value))
		{
			isHasLegalChar = true;
		}

		if (isHasSameData)
		{
			EditorUtility.DisplayDialog("警告", "已经有相同名称的成员", "确定");
			return;
		}
		if (isHasLegalChar)
		{
			EditorUtility.DisplayDialog("警告", "名称中含有非法字符", "确定");
			return;
		}

		//检查名称中是否有非法符号



		//创建模板数据
		factory.AddData(curCreateTemplateData);

		if (templateListView != null)
		{
			templateListView.Rebuild();
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
