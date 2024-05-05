using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class CodeTemplateInspector : VisualElement
{
	public new class UxmlFactory : UxmlFactory<CodeTemplateInspector, VisualElement.UxmlTraits>
	{

	}

	private CodeManagerWindow managerWindow;

	public CodeDatas datas;

	public CodeTemplateData CurShowCodeTemplate;




	#region 页面元素

	[SerializeField]
	private VisualTreeAsset container_assets;

	private TextField codeContent;

	private TextField nameInputField;

	private Label codeView;

	private ObjectField textAssetField;

	private Button editorBtn;

	private Button viewBtn;

	private ScrollView codeViewScroll;

	private ScrollView codeEditorScroll;


	#endregion


	public void BindToManagerWindow(CodeManagerWindow managerWindow)
	{
		this.managerWindow = managerWindow;

		container_assets = managerWindow.TemplateInspectorVisualTreeAsset;

		container_assets.CloneTree(this);

		this.style.flexGrow = 1;

		BindTemplateName(); //绑定名称相关

		BindTextAssetField(); //绑定TextAssetFiled

		BindCodeContent(); //绑定CodeContent

		BindAboutViewOrEditorChange();

		//首次刷新检视窗口
		if (datas != null && datas.CodeTemplates.Count > 0)
		{
			UpdateInspector(datas.CodeTemplates[0]);
		}
	}

	public void OnClose()
	{
	}

	private void BindAboutViewOrEditorChange()
	{
		codeView = this.Q<Label>("CodeView");
		codeViewScroll = this.Q<ScrollView>("CodeView");
		codeEditorScroll = this.Q<ScrollView>("CodeEditor");
		codeEditorScroll.style.display = DisplayStyle.None;
		codeViewScroll.style.display = DisplayStyle.Flex;


		BindEditorBtn();
		BindViewBtn();
	}

	private void BindTemplateName()
	{
		nameInputField = this.Q<TextField>("TemplateName");

		nameInputField.RegisterValueChangedCallback((value) =>
		{
			CurShowCodeTemplate.Name.SetValueWithoutAction(value.newValue);
		});

	}


	private void BindEditorBtn()
	{
		editorBtn = this.Q<Button>("EditorBtn");

		editorBtn.clicked += () =>
		{
			codeViewScroll.style.display = DisplayStyle.None;
			codeViewScroll.RemoveFromClassList("show-ani");
			codeEditorScroll.style.display = DisplayStyle.Flex;
			codeEditorScroll.AddToClassList("show-ani");
		};
	}

	private void BindViewBtn()
	{
		viewBtn = this.Q<Button>("ViewBtn");

		viewBtn.clicked += () =>
		{
			codeEditorScroll.style.display = DisplayStyle.None;
			codeEditorScroll.RemoveFromClassList("show-ani");
			codeViewScroll.style.display = DisplayStyle.Flex;
			codeViewScroll.AddToClassList("show-ani");
		};


	}

	private void UpdateCodeView(string codeContent)
	{

		string coloredCode = codeContent;

		// 遍历字典，为每个关键词着色
		foreach (var kvp in keywordColors)
		{
			string keyword = kvp.Key;
			string color = kvp.Value;

			// 使用正则表达式替换关键词并添加颜色标识
			coloredCode = Regex.Replace(coloredCode, "(^|\\s)(" + keyword + ")(?=$|\\s)", "$1<color=" + color + ">$2</color>");
		}

		codeView.text = coloredCode;
	}


	private void BindCodeContent()
	{

		codeContent = this.Q<TextField>("CodeContent");

		codeContent.RegisterValueChangedCallback((value) =>
		{
			CurShowCodeTemplate.CodeContent = value.newValue;
			UpdateCodeView(value.newValue);
		});



		datas = managerWindow.Datas;
	}


	private void BindTextAssetField()
	{
		textAssetField = this.Q<ObjectField>("TextAssetField");
		textAssetField.RegisterValueChangedCallback((value) =>
		{
			CurShowCodeTemplate.CodeTemplateFile = value.newValue as TextAsset;
			CurShowCodeTemplate.UpdateCodeContent();

			UpdateInspector(CurShowCodeTemplate);
		});
	}


	/// <summary>
	/// 更新检视面板
	/// </summary>
	public void UpdateInspector(CodeTemplateData templateData)
	{
		CurShowCodeTemplate = templateData;

		nameInputField.SetValueWithoutNotify(templateData.Name.Value);
		textAssetField.SetValueWithoutNotify(templateData.CodeTemplateFile);
		codeContent.SetValueWithoutNotify(templateData.CodeContent);
		UpdateCodeView(CurShowCodeTemplate.CodeContent);
	}


	Dictionary<string, string> keywordColors = new Dictionary<string, string>()
{
	{"class", "yellow"}, // yellow
    {"public", "yellow"}, // yellow
    {"private", "yellow"}, // yellow
    {"protected", "yellow"}, // yellow
    {"internal", "yellow"}, // yellow
    {"void", "yellow"}, // yellow
    {"string", "#008cba"}, // light blue
    {"int", "#00a0e8"}, // sky blue
    {"float", "#00a0e8"}, // sky blue
    {"double", "#00a0e8"}, // sky blue
    {"bool", "#00a0e8"}, // sky blue
    {"char", "#00a0e8"}, // sky blue
    {"byte", "#00a0e8"}, // sky blue
    {"short", "#00a0e8"}, // sky blue
    {"long", "#00a0e8"}, // sky blue
    {"decimal", "#00a0e8"}, // sky blue
    {"object", "#00a8a8"}, // teal
    {"dynamic", "#00a8a8"}, // teal
    {"readonly", "#00a8a8"}, // teal
    {"const", "#00a8a8"}, // teal
    {"static", "#00a8a8"}, // teal
    {"if", "#00a0e8"}, // sky blue
    {"else", "#00a0e8"}, // sky blue
    {"else if", "#00a0e8"}, // sky blue
    {"switch", "#00a0e8"}, // sky blue
    {"case", "#00a0e8"}, // sky blue
    {"default", "#00a0e8"}, // sky blue
    {"for", "#00a0e8"}, // sky blue
    {"foreach", "#00a0e8"}, // sky blue
    {"while", "#00a0e8"}, // sky blue
    {"do", "#00a0e8"}, // sky blue
    {"try", "#00a0e8"}, // sky blue
    {"catch", "#00a0e8"}, // sky blue
    {"finally", "#00a0e8"}, // sky blue
    {"throw", "#00a0e8"}, // sky blue
    {"return", "#00a0e8"}, // sky blue
    {"continue", "#00a0e8"}, // sky blue
    {"break", "#00a0e8"}, // sky blue
    {"new", "#00a0e8"}, // sky blue
    {"using", "#00a0e8"}, // sky blue
    {"namespace", "#00a0e8"}, // sky blue
    {"assembly", "#00a0e8"}, // sky blue
    {"params", "#00a0e8"}, // sky blue
    {"var", "#00a0e8"}, // sky blue
    {"true", "#00c3b1"}, // turquoise
    {"false", "#00c3b1"}, // turquoise
    {"null", "#00c3b1"}, // turquoise
    {"this", "#00c3b1"}, // turquoise
    {"base", "#00c3b1"}, // turquoise
    {"get", "#00c3b1"}, // turquoise
    {"set", "#00c3b1"}, // turquoise
    {"value", "#00c3b1"}, // turquoise
    {"delegate", "#00a0e8"}, // sky blue
    {"event", "#00a0e8"}, // sky blue
    {"=>", "#00c3b1"}, // turquoise
    {"List", "#8258FA"}, // medium purple
    {"ArrayList", "#8258FA"}, // medium purple
    {"Dictionary", "#8258FA"}, // medium purple
    {"HashSet", "#8258FA"}, // medium purple
    {"LinkedList", "#8258FA"}, // medium purple
    {"Queue", "#8258FA"}, // medium purple
    {"Stack", "#8258FA"}, // medium purple
    {"IEnumerable", "#8258FA"}, // medium purple
    {"IEnumerator", "#8258FA"}, // medium purple
    {"IEnumerable<T>", "#8258FA"}, // medium purple
    {"IEnumerator<T>", "#8258FA"}, // medium purple
    {"#region", "#00a0e8"}, // sky blue
    {"#endregion", "#00a0e8"}, // sky blue
    {"#if", "#00a0e8"}, // sky blue
    {"#else", "#00a0e8"}, // sky blue
    {"#elif", "#00a0e8"}, // sky blue
    {"#endif", "#00a0e8"}, // sky blue
    {"#define", "#00a0e8"}, // sky blue
    {"#undef", "#00a0e8"}, // sky blue
    {"#warning", "#00a0e8"}, // sky blue
    {"#error", "#00a0e8"}, // sky blue
    {"#line", "#00a0e8"}, // sky blue
    {"#nullable", "#00a0e8"}, // sky blue
    {"#pragma", "#00a0e8"}, // sky blue
    {"#pragma warning", "#00a0e8"}, // sky blue
    {"#pragma checksum", "#00a0e8"}, // sky blue
    {"#pragma warning disable", "#00a0e8"}, // sky blue
    {"#pragma warning restore", "#00a0e8"}, // sky blue
	{"#SCRIPTNAME#", "#8258FA"}, // medium purple
    {"#AUTHORNAME#", "#8258FA"}, // medium purple
    {"#CREATETIME#", "#8258FA"}, // medium purple

};
}


