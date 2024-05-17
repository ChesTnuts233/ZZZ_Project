//****************** 代码文件申明 ************************
//* 文件：TipContentElement                                       
//* 作者：Koo
//* 创建时间：2024/05/15 17:08:02 星期三
//* 描述：Nothing
//*****************************************************

using System;
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

		public Action OnDeleteSelf;

		#region 元素

		private TextField titleField;

		private TextField contentField;

		private Button deleteBtn;

		#endregion

		public new class UxmlFactory : UxmlFactory<TipContentElement, VisualElement.UxmlTraits>
		{

		}
		public void UpdateTitleAndContent(DirectoryTipData data)
		{
			tipData = data;
			this.title = tipData.Title;
			this.content = tipData.Content;
		}

		public void BindDirectoryPath(string path)
		{
			directoryPath = path;
		}

		public override void Init()
		{
			KooCode.AssetsData.TipContentElementVisualAsset.CloneTree(this);
			BindTextField();
			BindDeleteBtn();
		}

		private void BindTextField()
		{
			titleField = this.Q<TextField>("TitleField");
			titleField.value = title;
			titleField.RegisterValueChangedCallback((value) =>
			{
				tipData.Title = value.newValue;
			});


			contentField = this.Q<TextField>("ContentField");
			contentField.value = content;
			contentField.RegisterValueChangedCallback((value) =>
			{
				tipData.Content = value.newValue;
			});

		}

		private void BindDeleteBtn()
		{
			deleteBtn = this.Q<Button>("DeleteBtn");
			deleteBtn.clicked += DeleteSelf;
		}

		private void DeleteSelf()
		{
			//数据删除
			tipData.DirectoryData.TipsDatas.Remove(tipData);
			OnDeleteSelf?.Invoke();
		}





	}
}
