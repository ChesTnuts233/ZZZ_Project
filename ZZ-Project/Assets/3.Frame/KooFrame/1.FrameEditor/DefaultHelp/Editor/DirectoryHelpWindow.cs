using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class DirectoryHelpWindow : EditorWindow
{
	[SerializeField]
	private VisualTreeAsset m_VisualTreeAsset = default;

	[MenuItem("Window/UI Toolkit/DirectoryHelpWindow")]
	public static void ShowExample()
	{
		DirectoryHelpWindow wnd = GetWindow<DirectoryHelpWindow>();
		wnd.titleContent = new GUIContent("DirectoryHelpWindow");
	}

	public void CreateGUI()
	{
		VisualElement root = rootVisualElement;


	}
}
