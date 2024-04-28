//****************** 代码文件申明 ************************
//* 文件：LevelInspectorViewer                                       
//* 作者：Koo
//* 创建时间：2024/02/20 15:38:21 星期二
//* 功能：nothing
//*****************************************************

using GameEditor.Data;
using UnityEngine.UIElements;

namespace GameEditor
{
    public class LevelInspectorViewer : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<LevelInspectorViewer, VisualElement.UxmlTraits> { }

        private UnityEditor.Editor editor;

        public LevelInspectorViewer() { }


        public static LevelData CurInspectorLevelData;

        internal void UpdateSelection(LevelData levelData)
        {
            Clear(); //清除之前的信息
            UnityEngine.Object.DestroyImmediate(editor);
            editor = UnityEditor.Editor.CreateEditor(levelData);
            IMGUIContainer container = new IMGUIContainer(() =>
            {
                if (editor.target)
                {
                    editor.OnInspectorGUI();
                }
            });
            Add(container);
            CurInspectorLevelData = levelData;
        }
    }
}