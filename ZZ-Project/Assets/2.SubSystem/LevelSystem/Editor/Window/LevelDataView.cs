//****************** 代码文件申明 ************************
//* 文件：LevelDataView                                       
//* 作者：Koo
//* 创建时间：2024/02/24 17:02:54 星期六
//* 功能：
//*****************************************************

using GameEditor.Data;
using UnityEngine.UIElements;

namespace GameEditor
{
    public class LevelDataView : VisualElement
    {
        public LevelData LevelData;

        public Button ChangeLevelBtn;

        public new class UxmlFactory : UxmlFactory<LevelDataView, VisualElement.UxmlTraits> { }

        public void Init(LevelData levelData)
        {
            LevelData = levelData;
            ChangeLevelBtn.clicked += ChangeLevel;
        }

        private void ChangeLevel()
        {
            LevelData.ChangeToCurLevelInEditor();

            LevelManagerEditorWindow.UpdateCurLevelName();
        }


        /// <summary>
        /// 展示关卡的属性面板
        /// </summary>
        private void ShowLevelInspector() { }
    }
}