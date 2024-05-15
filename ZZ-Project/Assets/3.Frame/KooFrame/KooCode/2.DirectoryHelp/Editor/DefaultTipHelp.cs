//****************** 代码文件申明 ************************
//* 文件：DefaultTipHelp                                       
//* 作者：Koo
//* 创建时间：2024/05/13 21:28:45 星期一
//* 描述：默认文件提示
//*****************************************************

using System.IO;
using UnityEditor;
using UnityEngine.UIElements;

namespace KooFrame
{
	[CustomEditor(typeof(UnityEditor.DefaultAsset))]
	public class DefaultTipHelp : Editor
	{
		private CodeAssetsData settingsData => KooCode.AssetsData;

		private DirectoryHelpElement rootInstance;

		private DirectoryHelpElement root
		{
			get
			{
				if (rootInstance == null)
				{
					rootInstance = new DirectoryHelpElement();
					rootInstance.style.flexGrow = 1;
					rootInstance.Init(CurSelectPath);
				}
				return rootInstance;
			}
		}

		private string curSelectPath;

		/// <summary>
		/// 当前选中的路径
		/// </summary>
		public string CurSelectPath
		{
			get
			{
				if (curSelectPath.IsNullOrEmpty())
				{
					curSelectPath = AssetDatabase.GetAssetPath(target);
				}
				return curSelectPath;
			}
		}

		public override VisualElement CreateInspectorGUI()
		{
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
