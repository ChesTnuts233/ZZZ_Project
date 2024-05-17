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
		#region 元素

		private Button showTipsBtn;
		private Button showTemplatesBtn;


		#region Inpsectors

		/// <summary>
		/// 目录便签检视
		/// </summary>
		private DirectoryTipInspector directoryTipInspector;


		/// <summary>
		/// 模版生成系统
		/// </summary>
		private DirectoryCreateTemplateElement createTemplateElement;
		#endregion


		#endregion

		private string curInspectorPath;

		public new class UxmlFactory : UxmlFactory<DirectoryHelpElement, VisualElement.UxmlTraits>
		{

		}

		public void Init(string inspectorPath)
		{
			KooCode.AssetsData.DirectoryHelpElement.CloneTree(this);
			this.curInspectorPath = inspectorPath;
			BindAllButton();
			BindAllInspector();

			ShowInspector(directoryTipInspector);
		}

		private void BindAllButton()
		{
			showTipsBtn = this.Q<Button>("ShowTipsBtn");

			showTipsBtn.clicked += () => ShowInspector(directoryTipInspector);

			showTemplatesBtn = this.Q<Button>("ShowTemplatesBtn");

			showTemplatesBtn.clicked += () => ShowInspector(createTemplateElement);

		}

		private void BindAllInspector()
		{
			createTemplateElement = this.Q<DirectoryCreateTemplateElement>("CreateTemplateElement");
			createTemplateElement.BindDirectoryPath(curInspectorPath);
			createTemplateElement.Init();

			directoryTipInspector = this.Q<DirectoryTipInspector>("DirectoryTipInspector");
			directoryTipInspector.BindDirectoryPath(curInspectorPath);
			directoryTipInspector.Init();
		}


		private void ShowInspector(CustomVisualElement visual)
		{
			directoryTipInspector.Hide();
			createTemplateElement.Hide();
			visual.Show();
		}


	}
}
