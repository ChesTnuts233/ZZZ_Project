using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace KooFrame
{
	public class SettingsWindow : EditorWindow
	{
		private VisualElement root => rootVisualElement;

		#region 元素

		private TextField hexoPath;

		#endregion

		public static void ShowWindow()
		{
			SettingsWindow wnd = GetWindow<SettingsWindow>();
			wnd.titleContent = new GUIContent("SettingsWindow");
		}

		public void CreateGUI()
		{
			KooCode.AssetsData.SettingsWindowVisualAsset.CloneTree(root);
			hexoPath = root.Q<TextField>("HexoPath");
		}
	}
}
