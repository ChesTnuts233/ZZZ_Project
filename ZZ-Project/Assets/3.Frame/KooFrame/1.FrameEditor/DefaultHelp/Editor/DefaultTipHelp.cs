//****************** 代码文件申明 ************************
//* 文件：DefaultTipHelp                                       
//* 作者：Koo
//* 创建时间：2024/05/13 21:28:45 星期一
//* 描述：默认文件提示
//*****************************************************

using UnityEditor;
using UnityEngine.UIElements;

namespace KooFrame
{
	[CustomEditor(typeof(UnityEditor.DefaultAsset))]
	public class DefaultTipHelp : Editor
	{
		private CodeSettingsData settingsData;
		public CodeSettingsData SettingData
		{
			get
			{
				if (settingsData == null)
				{
					settingsData = AssetDatabase.LoadAssetAtPath<CodeSettingsData>("Assets/3.Frame/KooFrame/1.FrameEditor/CodeTemplate/Data/SettingsData.asset");
				}
				return settingsData;
			}
		}

		private VisualElement root;


		/// <summary>
		/// 当前选中的路径
		/// </summary>
		public string CurSelectPath;

		public override VisualElement CreateInspectorGUI()
		{

			root = new VisualElement();

			SettingData.DefaultFoldTipVistalTreeAsset.CloneTree(root);

			return root;
		}
	}
}
