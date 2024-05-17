

using UnityEngine.UIElements;

public abstract class CustomVisualElement : VisualElement
{
	public abstract void Init();

	/// <summary>
	/// 关闭自身
	/// </summary>
	public void Hide()
	{
		this.style.display = DisplayStyle.None;
	}

	public void Show()
	{
		this.style.display = DisplayStyle.Flex;
	}
}