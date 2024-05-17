using GameBuild;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


public class AnimatorHelpWindow : OdinEditorWindow
{
	public AnimatorHashData setting;     // 通过面板拖拽赋值
	public VisualTreeAsset editorUIAsset;   // 通过面板拖拽赋值


	[MenuItem("项目工具/动画参数Hash生成器")]
	public static void ShowExample()
	{
		AnimatorHelpWindow wnd = GetWindow<AnimatorHelpWindow>();
		wnd.titleContent = new GUIContent("动画机参数Hash生成器");
	}

	public void CreateGUI()
	{
		VisualElement root = rootVisualElement;

		root.Add(new IMGUIContainer(base.OnGUI));
		root.Add(editorUIAsset.Instantiate());
	}

	protected override object GetTarget()
	{
		return setting;
	}
}