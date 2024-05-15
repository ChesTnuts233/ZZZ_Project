//****************** 代码文件申明 ************************
//* 文件：DirectoryTipInspector                                       
//* 作者：Koo
//* 创建时间：2024/05/15 15:54:45 星期三
//* 描述：目录便签监视器
//*****************************************************

using UnityEngine.UIElements;

namespace KooFrame
{
	public class DirectoryTipInspector : CustomVisualElement
	{

		private string curDirectoryPath;

		#region 元素

		private TextField titleField;

		private TextField contentField;

		/// <summary>
		/// 所有的提示内容
		/// </summary>
		private ScrollView tipsScroll;

		private Button addTipBtn;

		#endregion


		public new class UxmlFactory : UxmlFactory<DirectoryTipInspector, VisualElement.UxmlTraits>
		{

		}


		public void BindDirectoryPath(string directoryPath)
		{
			curDirectoryPath = directoryPath;
		}

		public override void Init()
		{
			KooCode.AssetsData.DirectoryTipInspectorElement.CloneTree(this);
			BindButton();
		}

		private void BindButton()
		{
			addTipBtn = this.Q<Button>("AddTipBtn");
		}


	}
}
