using CodeTemplate;
using KooFrame;
using UnityEngine.UIElements;

public class CodeDataInspector : VisualElement
{
	private CodeManagerWindow managerWindow;

	public CodeData curCodeData;


	#region 元素

	private VisualTreeAsset container_assets;

	private IMGUIContainer imGUIcontainer;

	public MG.MDV.MarkdownViewer Viewer;


	#endregion



	public new class UxmlFactory : UxmlFactory<CodeDataInspector, VisualElement.UxmlTraits>
	{

	}

	public void BindToManagerWindow(CodeManagerWindow managerWindow)
	{
		this.managerWindow = managerWindow;

		container_assets = managerWindow.DataInspectorVisualTreeAsset;

		container_assets.CloneTree(this);

		Viewer = new MG.MDV.MarkdownViewer(managerWindow.DarkSkin, curCodeData.MarkDownPath, curCodeData.codeMD.text);

		

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

	public void UpdateInspector(CodeData data)
	{
		curCodeData = data;
	}


}