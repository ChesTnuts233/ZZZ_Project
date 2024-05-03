using KooFrame;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CodeTemplateInspector : VisualElement
{
	public new class UxmlFactory : UxmlFactory<CodeTemplateInspector, VisualElement.UxmlTraits>
	{
	}

	public CodeTemplateDatas datas;

	public CodeTemplateData CurShowCodeTemplate;


	#region 页面元素

	private TemplateContainer container;

	private IMGUIContainer imguiContainer;

	private TextField inputField;

	/// <summary>
	/// 滚动坐标
	/// </summary>
	private Vector2 scrollPosition = Vector2.zero;

	#endregion


	public CodeTemplateInspector()
	{
		container = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/3.Frame/KooFrame/1.FrameEditor/Editor/CodeTemplate/EditorWindow/TemplateInspector.uxml").Instantiate();

		container.style.flexGrow = 1;
		Add(container);

		BindTemplateName(); //绑定名称相关

		#region IMGUI

		imguiContainer = container.Q<IMGUIContainer>("Inspector");

		imguiContainer.onGUIHandler += ImOnGUI;

		datas = AssetDatabase.LoadAssetAtPath<CodeTemplateDatas>("Assets/3.Frame/KooFrame/1.FrameEditor/Editor/CodeTemplate/Data/CodeTemplateDatas.asset");

		if (datas != null && datas.CodeTemplates.Count > 0)
		{
			UpdateInspector(datas.CodeTemplates[0]);
		}

		#endregion


	}

	private void BindTemplateName()
	{
		inputField = container.Q<TextField>("TemplateName");

		inputField.RegisterValueChangedCallback((value) =>
		{
			CurShowCodeTemplate.Name = value.newValue;
		});
	}

	private void ImOnGUI()
	{
		//使用滚动视图
		scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

		if (CurShowCodeTemplate != null)
		{
			CurShowCodeTemplate.CodeContent = EditorGUI.TextArea(new Rect(0, 0, imguiContainer.worldBound.width, imguiContainer.worldBound.height), CurShowCodeTemplate.CodeContent);
		}

		EditorGUILayout.EndScrollView();
	}


	/// <summary>
	/// 更新检视面板
	/// </summary>
	public void UpdateInspector(CodeTemplateData templateData)
	{
		CurShowCodeTemplate = templateData;
		inputField.SetValueWithoutNotify(templateData.Name);
	}



}
