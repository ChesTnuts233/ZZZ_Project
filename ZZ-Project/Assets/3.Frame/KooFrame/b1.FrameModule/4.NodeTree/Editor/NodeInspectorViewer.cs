//****************** 代码文件申明 ************************
//* 文件：InspectorViewer                                       
//* 作者：Koo
//* 创建时间：2024/01/13 00:15:03 星期六
//* 功能：节点树属性查看器
//*****************************************************

using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace KooFrame
{
    public class NodeInspectorViewer : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<NodeInspectorViewer, VisualElement.UxmlTraits> { }

        private UnityEditor.Editor editor;

        private Vector2 scrollPosition = Vector2.zero;

        public NodeInspectorViewer() { }

        internal void UpdateSelection(NodeView nodeView)
        {
            Clear(); //清除之前的信息
            UnityEngine.Object.DestroyImmediate(editor);
            editor = UnityEditor.Editor.CreateEditor(nodeView.node);
            IMGUIContainer container = new IMGUIContainer(() =>
            {
                if (editor.target)
                {
                    //使用滚动视图
                    scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                    editor.OnInspectorGUI();
                    EditorGUILayout.EndScrollView();
                }
            });

            Add(container);
        }
    }
}