using UnityEditor;
using UnityEngine.UIElements;

namespace KooFrame
{
	public class DirectoryHelpWindow : EditorWindow
	{
		private VisualElement root => rootVisualElement;

		private DirectoryHelpElement DirectoryHelpElement;


		public void CreateGUI()
		{
			KooCode.AssetsData.DirectoryExWindow.CloneTree(root);
			DirectoryHelpElement = root.Q<DirectoryHelpElement>("DirectoryHelpElement");
		}

		public void Init(string inspectorPath)
		{
			DirectoryHelpElement.Init(inspectorPath);
		}
	}
}
