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
		//创建模板数据
		factory.CreateData("curCreateTemplateName");
		//创建模板文件
		CreateTXTTemplateFile();

	}


	/// <summary>
	/// 创建模板文件
	/// </summary>
	private void CreateTXTTemplateFile()
	{

	}

	private void UpdateTemplateMenuItem()
	{
		//遍历所有的codeTemplateDatas，根据名称生成对应的MenuItem
		string updateContent = "";
		foreach (CodeTemplateData data in datas.CodeTemplates)
		{
			updateContent += "\t\t[MenuItem(\"Assets/KooFrame-脚本/" + data.Name + "\", false, 0)]\r\n\t\tpublic static void Create" + data.Name + "Scripts()\r\n\t\t{\r\n\t\t\tScriptsTemplatesCreater.CreateMyScript(\"DefaultMonoScripts.cs\",\r\n\t\t\t\tTemplatesPath + \"/04-KooFrame__MonoBehaviour 完整架构模板.cs.txt\");\r\n\t\t}";

		}

		////生成右键菜单选项
		//KooTool.CodeGenerator_ByTag<CodeTemplateMenuItem>();
	}


}
