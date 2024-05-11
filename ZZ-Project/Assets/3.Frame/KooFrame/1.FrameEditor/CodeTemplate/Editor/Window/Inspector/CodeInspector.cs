
using UnityEngine.UIElements;

public abstract class CodeInspector : VisualElement
{
	protected CodeManagerWindow ManagerWindow;

	private VisualTreeAsset container_assets;

	public KooCodeDatas Datas;

	public virtual void BindToManagerWindow(CodeManagerWindow managerWindow)
	{
		ManagerWindow = managerWindow;
		
	}

	/// <summary>
	/// 更新检视器
	/// </summary>
	/// <param name="data"></param>
	public abstract void UpdateInspector();

}