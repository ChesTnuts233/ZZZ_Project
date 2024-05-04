using KooFrame;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CodeTemplateCreateWindow : EditorWindow
{
	[SerializeField]
	private VisualTreeAsset m_VisualTreeAsset = default;


	private string curCreateTemplateName;

	private CodeTemplateFactory factory;

	private CodeTemplateDatas datas;

	#region 页面元素

	private TextField nameField;

	private Button createBtn;

	#endregion

	public static void ShowWindow()
	{
		CodeTemplateCreateWindow wnd = GetWindow<CodeTemplateCreateWindow>();
		wnd.titleContent = new GUIContent("创建脚本模板");
	}

	private void OnEnable()
	{
		factory = new();
		datas = factory.Datas;
	}


	public void CreateGUI()
	{
		VisualElement root = rootVisualElement;

		m_VisualTreeAsset.CloneTree(root);

		BindNameField(root);

		BindCreateBtn(root);

		nameField.Focus();
	}




	private void BindNameField(VisualElement root)
	{
		nameField = root.Q<TextField>("NameInput");

		nameField.RegisterValueChangedCallback((value) =>
		{
			curCreateTemplateName = value.newValue;
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
		//先看Datas里是否有重名的  重名的不能添加
		foreach (var data in datas.CodeTemplates)
		{
			if (data.Name == curCreateTemplateName)
			{
				isHasSameData = true;
			}
		}
		if (isHasSameData)
		{
			EditorUtility.DisplayDialog("警告", "已经有相同名称的成员", "确定");
			return;
		}



		//创建模板数据
		factory.CreateData(curCreateTemplateName);


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
