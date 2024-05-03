using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class CodeTemplateInspector : VisualElement
{
	public new class UxmlFactory : UxmlFactory<CodeTemplateInspector, VisualElement.UxmlTraits>
	{
	}

	private CodeTemplateManagerWindow managerWindow;

	public CodeTemplateDatas datas;

	public CodeTemplateData CurShowCodeTemplate;


	#region 页面元素

	[SerializeField]
	private VisualTreeAsset container_assets;

	private TextField codeContent;

	private TextField nameInputField;

	private ObjectField textAssetField;


	#endregion



	public void OnBindToManagerWindow(CodeTemplateManagerWindow managerWindow)
	{
		this.managerWindow = managerWindow;

		container_assets = managerWindow.InspectorVisualTreeAsset;

		container_assets.CloneTree(this);

		this.style.flexGrow = 1;

		BindTemplateName(); //绑定名称相关

		BindTextAssetField(); //绑定TextAssetFiled

		BindCodeContent(); //绑定IMGUI

		//首次刷新检视窗口
		if (datas != null && datas.CodeTemplates.Count > 0)
		{
			UpdateInspector(datas.CodeTemplates[0]);
		}
	}

	public void OnClose()
	{
	}



	private void BindTemplateName()
	{
		nameInputField = this.Q<TextField>("TemplateName");

		nameInputField.RegisterValueChangedCallback((value) =>
		{
			CurShowCodeTemplate.Name.SetValueWithoutAction(value.newValue);
		});

	}



	private void BindCodeContent()
	{

		codeContent = this.Q<TextField>("CodeContent");

		codeContent.RegisterValueChangedCallback((value) =>
		{
			CurShowCodeTemplate.CodeContent = value.newValue;
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
			codeContent.SetValueWithoutNotify(CurShowCodeTemplate.CodeContent);
			nameInputField.SetValueWithoutNotify(CurShowCodeTemplate.Name);
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
	}



}
