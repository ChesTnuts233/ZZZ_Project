using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace KooFrame
{
	public class UMLNodeEditor : EditorWindow
	{
		NodeTreeViewer nodeTreeViewer;

		//节点SO属性查看器
		protected NodeInspectorViewer _nodeInspectorViewer;
		protected static Label curEditorNodeTreeName;
		NodeTree curEditorNodeTree;

		public NodeTree CurEditorTree
		{
			get => curEditorNodeTree;
			set => curEditorNodeTree = value;
		}


		public static void ShowNodeEditorWindow(UMLTree tree)
		{
			UMLNodeEditor wnd = GetWindow<UMLNodeEditor>();
			wnd.CurEditorTree = tree;
			wnd.titleContent = new GUIContent("简易类图编辑");
		}

		[OnOpenAsset]
		public static bool OnOpenAsset(int instanceID, int line)
		{
			//如果是对话树
			if (Selection.activeObject is UMLTree umlTree)
			{
				ShowNodeEditorWindow(umlTree);
				Selection.activeObject = null;
				return true;
			}
			return false;
		}



		public void CreateGUI()
		{
			VisualElement root = rootVisualElement;

			var nodeTree = KooCode.AssetsData.UMLNodeTreeVisualAsset;
			// 此处不使用visualTree.Instantiate() 为了保证行为树的单例防止重复实例化，以及需要将此root作为传参实时更新编辑器状态
			nodeTree.CloneTree(root);

			var styleSheet = KooCode.AssetsData.UMLNodeTreeStyleSheet;
			root.styleSheets.Add(styleSheet);



			// 将节点树视图添加到节点编辑器中 Q是查询元素的方法
			nodeTreeViewer = root.Q<NodeTreeViewer>();
			// 将节属性面板视图添加到节点编辑器中
			_nodeInspectorViewer = root.Q<NodeInspectorViewer>();
			//节点树的OnNodeSelected 被触发 触发的是 更新inspectorViewer的方法


			nodeTreeViewer.OnNodeSelected = OnNodeSelectionChanged;

			//nodeTreeViewer.PopulateView(curEditorNodeTree); //刷新网格视图






			curEditorNodeTreeName = root.Q<Label>("CurEditorNodeTree");


			OnSelectionChange();
		}


		protected virtual void OnNodeSelectionChanged(NodeView view)
		{
			_nodeInspectorViewer.UpdateSelection(view);
		}

		protected void OnSelectionChange()
		{
			// if (Selection.activeObject is NodeTree)
			// {
			//     if (Selection.activeObject is DialogueTree)
			//     {
			//         // DialogueNodeTreeView dialogueNodeTreeView  = _DialogueNodeTreeView = root.Q<NodeTreeViewer>();
			//         // ;
			//         // 检测该选中对象中是否存在节点树  这里决定了 编辑器窗口 对选中对象的操作内容 
			//         curEditorNodeTree = Selection.activeObject as DialogueTree;
			//         curEditorNodeTreeName.text = "当前正在编辑:" + curEditorNodeTree.name;
			//         //_nodeInspectorViewer.UpdateSelection(_DialogueNodeTreeView);
			//
			//         //dialogueNodeTreeView.PopulateView(curEditorNodeTree);
			//     }
			//
			//     if (Selection.activeObject is TaskTree)
			//     {
			//         TaskNodeTreeView actionTreeView = (TaskNodeTreeView)nodeTreeViewer;
			//         // 检测该选中对象中是否存在节点树  这里决定了 编辑器窗口 对选中对象的操作内容 
			//         curEditorNodeTree = Selection.activeObject as TaskTree;
			//         curEditorNodeTreeName.text = "当前正在编辑:" + curEditorNodeTree.name;
			//         //_nodeInspectorViewer.UpdateSelection(_DialogueNodeTreeView);
			//
			//         actionTreeView.PopulateView(curEditorNodeTree);
			//     }
			// }
			// else
			// {
			//     curEditorNodeTreeName.text = "没有选中节点树SO";
			// }
			//
			if (Application.isPlaying)
			{
				if (curEditorNodeTree)
				{
					if (nodeTreeViewer != null)
					{
						nodeTreeViewer.PopulateView(curEditorNodeTree);
					}
				}
			}
			else
			{
				if (curEditorNodeTree && AssetDatabase.CanOpenAssetInEditor(curEditorNodeTree.GetInstanceID()))
				{
					if (nodeTreeViewer != null)
					{
						nodeTreeViewer.PopulateView(curEditorNodeTree);
					}
				}
			}
		}

		protected virtual void OnInspectorUpdate()
		{
			nodeTreeViewer?.UpdateNodeStates();
		}
	}
}