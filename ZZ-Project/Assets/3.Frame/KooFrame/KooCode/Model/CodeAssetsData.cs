using UnityEngine;
using UnityEngine.UIElements;

namespace KooFrame
{

	[CreateAssetMenu(fileName = "SettingsData", menuName = "KooCode/SettingsData")]
	public class CodeAssetsData : ScriptableObject
	{
		#region 排布资源
		[SerializeField, Header("主管理器UXML资源")]
		public VisualTreeAsset ManagerVisualTreeAsset = default;

		[SerializeField, Header("检视管理器UXML资源")]
		public VisualTreeAsset InspectorManagerTreeAsset = default;

		[SerializeField, Header("模板检视器UXML资源")]
		public VisualTreeAsset TemplateInspectorVisualTreeAsset = default;

		[SerializeField, Header("笔记检视器UXML资源")]
		public VisualTreeAsset MarkInspectorVisualTreeAsset = default;

		[SerializeField, Header("模版列表选项UXML资源")]
		public VisualTreeAsset TemplateListItemVistalTreeAsset = default;

		[SerializeField, Header("笔记列表选项UXML资源")]
		public VisualTreeAsset MarkDataListItemVistalTreeAsset = default;

		[SerializeField, Header("MarkDownGUISkin资源")]
		public GUISkin DarkSkin;


		[SerializeField, Header("文件夹InspectorUXML资源")]
		public VisualTreeAsset DefaultFoldTipVistalTreeAsset = default;


		[SerializeField, Header("文件夹内选择ItemUXML资源")]
		public VisualTreeAsset DefaultFoldChooseItemVisualAsset = default;

		[SerializeField, Header("文件夹内待被创建的列表UXML资源")]
		public VisualTreeAsset DefaultFoldNeedCreatedItemVisualAsset = default;

		[SerializeField, Header("目录管理元素UXML资源")]
		public VisualTreeAsset DirectoryHelpElement = default;

		[SerializeField, Header("目录拓展窗口UXML资源")]
		public VisualTreeAsset DirectoryExWindow = default;

		[SerializeField, Header("便签检视UXML资源")]
		public VisualTreeAsset DirectoryTipInspectorElement = default;

		[SerializeField, Header("便签元素UXML资源")]
		public VisualTreeAsset TipContentElementVisualAsset = default;


		#endregion
	}
}

