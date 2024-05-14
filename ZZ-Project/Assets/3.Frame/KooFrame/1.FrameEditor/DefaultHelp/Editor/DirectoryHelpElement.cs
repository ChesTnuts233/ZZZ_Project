//****************** 代码文件申明 ************************
//* 文件：DirectoryHelpElement                                       
//* 作者：Koo
//* 创建时间：2024/05/14 22:31:14 星期二
//* 描述：Nothing
//*****************************************************

using UnityEngine.UIElements;

namespace KooFrame
{
	public class DirectoryHelpElement : VisualElement
	{
		private DirectoryCreateTemplateElement createTemplateElement;


		public new class UxmlFactory : UxmlFactory<DirectoryHelpElement, VisualElement.UxmlTraits>
		{

		}

		public void Init()
		{
			KooCode.SettingsData


			BindAllInspector();
		}

		private void BindAllInspector()
		{
			createTemplateElement = this.Q<DirectoryCreateTemplateElement>("CreateTemplateElement");
			createTemplateElement.Init();
		}



	}
}
