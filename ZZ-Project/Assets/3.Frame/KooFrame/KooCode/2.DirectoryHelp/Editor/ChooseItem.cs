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

		public Toggle Toggle;

		public CodeTemplateData TemplateData;

		public new class UxmlFactory : UxmlFactory<ChooseItem, VisualElement.UxmlTraits>
		{

		}

		public void Init(VisualTreeAsset uxmlAsset, CodeTemplateData templateData)
		{
			uxmlAsset.CloneTree(this);
			Toggle = this.Q<Toggle>("ChooseToggle");
			TemplateData = templateData;
			Toggle.label = templateData.Name;
		}

	}
}