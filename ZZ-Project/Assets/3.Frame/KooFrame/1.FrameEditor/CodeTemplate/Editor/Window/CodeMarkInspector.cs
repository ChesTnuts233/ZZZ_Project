using UnityEngine.UIElements;

/// <summary>
/// 代码笔记监视器
/// </summary>
public class CodeMarkInspector : CodeInspector
{
	private CodeManagerWindow managerWindow;

	public CodeMarkData curCodeMarkData;


	#region 元素

	private VisualTreeAsset container_assets;

	private IMGUIContainer imGUIcontainer;

	public MG.MDV.MarkdownViewer Viewer;


	#endregion

	public new class UxmlFactory : UxmlFactory<CodeMarkInspector, VisualElement.UxmlTraits>
	{

	}

	public void BindToManagerWindow(CodeManagerWindow managerWindow)
	{
		this.managerWindow = managerWindow;

		container_assets = managerWindow.DataInspectorVisualTreeAsset;

		container_assets.CloneTree(this);

		Viewer = new MG.MDV.MarkdownViewer(managerWindow.DarkSkin, curCodeMarkData.MarkDownPath, curCodeMarkData.codeMD.text);

		imGUIcontainer = this.Q<IMGUIContainer>("MarkDownView");

		//添加绘制监听
		imGUIcontainer.onGUIHandler += OnMarkDownViewGUI;
		//EditorApplication.update += UpdateRequests;
	}

	public void Close()
	{
		//EditorApplication.update -= UpdateRequests;
	}


	private void OnMarkDownViewGUI()
	{
		Viewer?.Draw();
	}


	//void UpdateRequests()
	//{
	//	if (Viewer.Update())
	//	{
	//		Repaint();
	//	}
	//}

	public void UpdateInspector(CodeMarkData data)
	{
		curCodeMarkData = data;
	}


}