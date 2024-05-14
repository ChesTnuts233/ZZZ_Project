//****************** 代码文件申明 ************************
//* 文件：DefaultTipHelp                                       
//* 作者：Koo
//* 创建时间：2024/05/13 21:28:45 星期一
//* 描述：默认文件提示
//*****************************************************

using GameBuild;
using System;
using System.IO;
using UnityEditor;
using UnityEngine.UIElements;

namespace KooFrame
{
	[CustomEditor(typeof(UnityEditor.DefaultAsset))]
	public class DefaultTipHelp : Editor
	{
		private CodeSettingsData settingsData => KooCode.SettingsData;

		private DirectoryCreateTemplateElement rootInstance;

		private DirectoryCreateTemplateElement root
		{
			get
			{
				if (rootInstance == null)
				{
					rootInstance = new DirectoryCreateTemplateElement();
					rootInstance.Init();
				}
				return rootInstance;
			}
		}


		/// <summary>
		/// 当前选中的路径
		/// </summary>
		public static string CurSelectPath;

		public override VisualElement CreateInspectorGUI()
		{
			CurSelectPath = AssetDatabase.GetAssetPath(target);
			if (Directory.Exists(CurSelectPath)) //如果是文件夹
			{
				return root;
			}
			else
			{
				return base.CreateInspectorGUI();
			}
		}

	}
}
