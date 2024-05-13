//****************** 代码文件申明 ************************
//* 文件：ExProject                                       
//* 作者：Koo
//* 创建时间：2024/05/13 22:22:25 星期一
//* 描述：Nothing
//*****************************************************

using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace GameBuild
{
	public class ExProject
	{
		[InitializeOnLoadMethod]
		static void InitializeOnLoad()
		{


			//EditorApplication.projectWindowItemOnGUI = delegate (string guid, Rect selectionRect)
			//{
			//	if (Selection.activeObject)
			//	{
			//		//设置拓展区域
			//		float width = 20f;
			//		float height = 13f;
			//		selectionRect.x += selectionRect.width - width;
			//		selectionRect.y += selectionRect.height - height - 2f;
			//		selectionRect.width = width;
			//		selectionRect.height = height;

			//		GUIStyle centeredStyle = new GUIStyle(GUI.skin.button);
			//		centeredStyle.alignment = TextAnchor.MiddleCenter;

			//		if (GUILayout.Button("X"))
			//		{
			//			Debug.Log("Test");
			//		}
			//	}
			//};
		}
	}
}
