using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace KooFrame
{
	public class SettingsWindow : EditorWindow
	{

		public static void ShowExample()
		{
			SettingsWindow wnd = GetWindow<SettingsWindow>();
			wnd.titleContent = new GUIContent("SettingsWindow");
		}

		public void CreateGUI()
		{

		}
	}
}
