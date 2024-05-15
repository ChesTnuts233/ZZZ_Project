using UnityEngine.UIElements;

namespace KooFrame
{

	/// <summary>
	/// 主要是管理所有的代码监视器
	/// </summary>
	public class CodeInspectorManager : VisualElement
	{
		public new class UxmlFactory : UxmlFactory<CodeInspectorManager, VisualElement.UxmlTraits>
		{

		}

		private VisualTreeAsset container_assets;

		private KooCodeWindow managerWindow;

		private CodeInspector curCheckInspector;


		#region 所有检视器

		/// <summary>
		/// 代码笔记检视
		/// </summary>
		private CodeMarkInspector codeMarkInspector;

		/// <summary>
		/// 代码模板检视
		/// </summary>
		private CodeTemplateInspector codeTemplateInspector;


		#endregion


		public void BindToManagerWindow(KooCodeWindow managerWindow)
		{
			this.managerWindow = managerWindow;

			container_assets = KooCode.AssetsData.InspectorManagerTreeAsset;

			container_assets.CloneTree(this);

			BindInspectors();  //绑定所有监视器

			curCheckInspector = codeMarkInspector;

			//首次刷新检视窗口
			if (KooCode.Datas != null && KooCode.Datas.CodeTemplates.Count > 0)
			{
				UpdateInspectors(KooCode.Datas.CodeTemplates[0]);
			}
		}



		/// <summary>
		/// 显示对应的监视器
		/// </summary>
		public void ShowInspector()
		{

		}


		/// <summary>
		/// 绑定所有监视器
		/// </summary>
		private void BindInspectors()
		{
			codeMarkInspector = this.Q<CodeMarkInspector>("CodeMarkInspector");
			codeMarkInspector.BindToManagerWindow(managerWindow);  //绑定到管理窗口
			codeTemplateInspector = this.Q<CodeTemplateInspector>("CodeTemplateInspector");
			codeTemplateInspector.BindToManagerWindow(managerWindow);
		}


		public void Close()
		{
			codeMarkInspector.Close();
			codeTemplateInspector.Close();

			codeMarkInspector = null;
			codeTemplateInspector = null;
		}


		/// <summary>
		/// 更新检视面板
		/// </summary>
		public void UpdateInspectors(CodeData data)
		{
			//更新对应的数据
			if (data is CodeTemplateData templateData)
			{
				//应该先更新数据 再显示出面板
				codeTemplateInspector.UpdateInspector(templateData);
				ShowInspector(codeTemplateInspector);
			}
			else if (data is CodeMarkData markData)
			{
				codeMarkInspector.UpdateInspector(markData);
				ShowInspector(codeMarkInspector);
			}
		}


		private void ShowInspector(CodeInspector showInspector)
		{
			codeTemplateInspector.Close();
			codeMarkInspector.Close();
			showInspector.Show();
		}

	}
}


