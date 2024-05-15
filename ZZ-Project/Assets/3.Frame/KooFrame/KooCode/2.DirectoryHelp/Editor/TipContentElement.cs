//****************** 代码文件申明 ************************
//* 文件：TipContentElement                                       
//* 作者：Koo
//* 创建时间：2024/05/15 17:08:02 星期三
//* 描述：Nothing
//*****************************************************

using UnityEngine.UIElements;

namespace KooFrame
{
	/// <summary>
	/// 提示内容元素
	/// </summary>
	public class TipContentElement : CustomVisualElement
	{
		private DirectoryTipData tipData;

		private string title;

		private string content;

		/// <summary>
		/// 所属的路径
		/// </summary>
		private string directoryPath;

		#region 元素

		private TextField titleField;

		private TextField contentField;

		#endregion

		public new class UxmlFactory : UxmlFactory<TipContentElement, VisualElement.UxmlTraits>
		{

		}
		public void UpdateTitleAndContent(string title, string content)
		{
			this.title = title;
			this.content = content;
		}

		public override void Init()
		{
			KooCode.AssetsData.TipContentElementVisualAsset.CloneTree(this);
			BindTextField();
		}

		private void BindTextField()
		{
			titleField = this.Q<TextField>("TitleField");
			contentField = this.Q<TextField>("ContentField");
		}





	}
}
