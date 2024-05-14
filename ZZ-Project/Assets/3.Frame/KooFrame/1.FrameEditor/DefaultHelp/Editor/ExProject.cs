//****************** 代码文件申明 ************************
//* 文件：ExProject                                       
//* 作者：Koo
//* 创建时间：2024/05/13 22:22:25 星期一
//* 描述：Nothing
//*****************************************************

using KooFrame;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GameBuild
{
	public class ExProject
	{
		[InitializeOnLoadMethod]
		static void InitializeOnLoad()
		{
			EditorApplication.projectWindowItemOnGUI = delegate (string guid, Rect selectionRect)
			{
				if (Selection.activeObject && Selection.assetGUIDs.Contains(guid) && Directory.Exists(AssetDatabase.GUIDToAssetPath(guid)))
				{

					//按钮的宽度和高度
					float buttonWidth = 20f;
					float buttonHeight = 20f;

					//设置按钮区域，使其位于选中图标的右上角
					Rect buttonRect = new Rect(
						selectionRect.x + selectionRect.width - buttonWidth - 10f,
						selectionRect.y,
						buttonWidth,
						buttonHeight
					);

					// 绘制按钮
					if (GUI.Button(buttonRect, "!"))
					{
						//打开默认资源编辑窗口

					}
				}
			};
		}
	}
}
