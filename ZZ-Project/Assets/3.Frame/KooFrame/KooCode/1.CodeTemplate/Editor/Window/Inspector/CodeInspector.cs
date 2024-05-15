
using UnityEngine.UIElements;

namespace KooFrame
{
	public abstract class CodeInspector : VisualElement
	{
		protected KooCodeWindow ManagerWindow;

		private VisualTreeAsset container_assets;

		public KooCodeDatas Datas;

		public virtual void BindToManagerWindow(KooCodeWindow managerWindow)
		{
			ManagerWindow = managerWindow;
		}

		public virtual void Show()
		{

		}

		public virtual void Close()
		{

		}

		/// <summary>
		/// 更新检视器
		/// </summary>
		/// <param name="data"></param>
		public abstract void UpdateInspector();

	}
}