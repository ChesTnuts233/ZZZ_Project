//****************** 代码文件申明 ************************
//* 文件：ChooseItem                                       
//* 作者：Koo
//* 创建时间：2024/05/14 18:20:34 星期二
//* 描述：Nothing
//*****************************************************

using UnityEngine.UIElements;

namespace KooFrame
{
	public class ChooseItem : VisualElement
	{

		public Label templateName;

		public Button addBtn;

		public Button removeBtn;

		public CodeData ChooseCodeData;

		public new class UxmlFactory : UxmlFactory<ChooseItem, VisualElement.UxmlTraits>
		{

		}

		public void Init(VisualTreeAsset uxmlAsset, CodeData codeData)
		{
			uxmlAsset.CloneTree(this);
			templateName = this.Q<Label>("TemplateName");
			addBtn = this.Q<Button>("AddBtn");
			removeBtn = this.Q<Button>("RemoveBtn");
			ChooseCodeData = codeData;
			templateName.text = codeData.Name;
		}

	}
}
