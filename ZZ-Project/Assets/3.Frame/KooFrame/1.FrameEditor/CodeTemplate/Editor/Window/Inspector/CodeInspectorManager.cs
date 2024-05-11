using UnityEngine.UIElements;

/// <summary>
/// 主要是管理所有的代码监视器
/// </summary>
public class CodeInspectorManager : VisualElement
{
	public new class UxmlFactory : UxmlFactory<CodeInspectorManager, VisualElement.UxmlTraits>
	{

	}

	private VisualTreeAsset container_assets;

	private CodeManagerWindow managerWindow;

	private CodeInspector curCheckInspector;


	#region 所有检视器

	/// <summary>
	/// 代码笔记检视
	/// </summary>
	private CodeMarkInspector codeMarkInspector;

	/// <summary>
	/// 代码模板检视
	/// </summary>
	private CodeTemplateInspector codeTemplateInspector;


	#endregion


	public void BindToManagerWindow(CodeManagerWindow managerWindow)
	{
		this.managerWindow = managerWindow;

		container_assets = managerWindow.SettingsData.InspectorManagerTreeAsset;

		container_assets.CloneTree(this);

		BindInspectors();  //绑定所有监视器

		curCheckInspector = codeMarkInspector;

		//首次刷新检视窗口
		if (managerWindow.Datas != null && managerWindow.Datas.CodeTemplates.Count > 0)
		{
			UpdateInspectors(managerWindow.Datas.CodeTemplates[0]);
		}
	}

	public void OnClose()
	{
	}



	/// <summary>
	/// 显示对应的监视器
	/// </summary>
	public void ShowInspector()
	{

	}


	/// <summary>
	/// 绑定所有监视器
	/// </summary>
	private void BindInspectors()
	{
		codeMarkInspector = this.Q<CodeMarkInspector>("CodeMarkInspector");
		codeMarkInspector.BindToManagerWindow(managerWindow);  //绑定到管理窗口
		codeTemplateInspector = this.Q<CodeTemplateInspector>("CodeTemplateInspector");
		codeTemplateInspector.BindToManagerWindow(managerWindow);
	}


	private void Close()
	{
		codeMarkInspector.Close();
		//codeTemplateInspector.Close();

		codeMarkInspector = null;
		codeTemplateInspector = null;
	}


	/// <summary>
	/// 更新检视面板
	/// </summary>
	public void UpdateInspectors(CodeData data)
	{
		//更新对应的数据
		if (data is CodeTemplateData templateData)
		{
			ShowInspector(codeTemplateInspector);
			codeTemplateInspector.UpdateInspector(templateData);
		}
		else if (data is CodeMarkData markData)
		{
			ShowInspector(codeMarkInspector);
			codeMarkInspector.UpdateInspector(markData);
		}
	}


	private void ShowInspector(VisualElement showElement)
	{
		codeTemplateInspector.Close();
		codeMarkInspector.Close();
		showElement.style.display = DisplayStyle.Flex;
	}

}


