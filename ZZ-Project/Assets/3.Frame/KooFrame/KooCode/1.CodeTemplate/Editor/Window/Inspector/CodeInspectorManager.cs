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
		private CodeDataInspector codeDataInspector;


		#endregion


		public void BindToManagerWindow(KooCodeWindow managerWindow)
		{
			this.managerWindow = managerWindow;

			container_assets = KooCode.AssetsData.InspectorManagerTreeAsset;

			container_assets.CloneTree(this);

			BindInspectors();  //绑定所有监视器

			curCheckInspector = codeMarkInspector;

			//首次刷新检视窗口
			if (KooCode.Datas != null && KooCode.Datas.CodeDatas.Count > 0)
			{
				UpdateInspectors(KooCode.Datas.CodeDatas[0]);
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
			codeDataInspector = this.Q<CodeDataInspector>("CodeDataInspector");
			codeDataInspector.BindToManagerWindow(managerWindow);
		}


		public void Close()
		{
			codeMarkInspector.Close();
			codeDataInspector.Close();

			codeMarkInspector = null;
			codeDataInspector = null;
		}


		/// <summary>
		/// 更新检视面板
		/// </summary>
		public void UpdateInspectors(CodeData data)
		{
			if (data == null)
			{
				return;
			}
			//更新对应的数据
			if (data.GetType() == typeof(CodeData))
			{
				//应该先更新数据 再显示出面板
				codeDataInspector.UpdateInspector(data);
				ShowInspector(codeDataInspector);
			}
			else if (data.GetType() == typeof(CodeMarkData))
			{
				CodeMarkData markData = (CodeMarkData)data;
				codeMarkInspector.UpdateInspector(markData);
				ShowInspector(codeMarkInspector);
			}
		}


		private void ShowInspector(CodeInspector showInspector)
		{
			codeDataInspector.Close();
			codeMarkInspector.Close();
			showInspector.Show();
		}

	}
}


