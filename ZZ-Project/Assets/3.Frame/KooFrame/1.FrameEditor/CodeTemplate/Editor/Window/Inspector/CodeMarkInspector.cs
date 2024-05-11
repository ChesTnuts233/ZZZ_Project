using UnityEngine.UIElements;

/// <summary>
/// 代码笔记监视器
/// </summary>
public class CodeMarkInspector : CodeInspector
{
	public CodeMarkData curCheckMarkData;

	#region 元素

	private VisualTreeAsset container_assets;

	private IMGUIContainer imGUIcontainer;

	private MG.MDV.MarkdownViewer viewer;

	#endregion

	public new class UxmlFactory : UxmlFactory<CodeMarkInspector, VisualElement.UxmlTraits>
	{

	}

	/// <summary>
	/// 绑定到管理窗口
	/// </summary>
	/// <param name="managerWindow"></param>
	public override void BindToManagerWindow(CodeManagerWindow managerWindow)
	{
		base.BindToManagerWindow(managerWindow);
		container_assets = managerWindow.SettingsData.MarkInspectorVisualTreeAsset;
		container_assets.CloneTree(this);

		imGUIcontainer = this.Q<IMGUIContainer>("MarkDownView");

		//添加绘制监听
		imGUIcontainer.onGUIHandler += OnMarkDownViewGUI;
	}

	public void Close()
	{
		imGUIcontainer.onGUIHandler -= OnMarkDownViewGUI;
	}

	private void OnMarkDownViewGUI()
	{
		viewer?.Draw();
	}

	public override void UpdateInspector()
	{
		UpdateInspector(curCheckMarkData);
	}

	public void UpdateInspector(CodeMarkData data)
	{
		curCheckMarkData = data;
		viewer = new MG.MDV.MarkdownViewer(ManagerWindow.SettingsData.DarkSkin, curCheckMarkData.MarkDownPath, curCheckMarkData.codeMD.text);
	}


}