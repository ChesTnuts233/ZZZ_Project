//****************** 代码文件申明 ************************
//* 文件：LevelManagerWindow                      
//* 作者：32867
//* 创建时间：2023年09月12日 星期二 00:48
//* 描述：用于进行关卡管理的窗口
//*****************************************************

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace GameEditor
{
    public class LevelManagerWindow : EditorWindow
    {
        public static void OpenLevelManger()
        {
            LevelManagerWindow window = GetWindow<LevelManagerWindow>();
            window.titleContent = new GUIContent("TempLevelManager");
        }

        private Vector2 minWindowSize = new Vector2(300f, 30f);

        //在OnEnable中设置窗口的最小大小
        private void OnEnable()
        {
            minSize = minWindowSize;
        }

        private void OnGUI()
        {
            #region 纵向简单布局

            GUILayout.FlexibleSpace(); // 添加垂直弹性空间，将按钮居中
            GUILayout.BeginVertical();

            #region 横向简单布局

            GUILayout.BeginHorizontal(); // 开始横向布局
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("打开功能测试关卡"))
            {
                string MainFuncTestPath = "Assets/4.Scenes/FuncTestScene/Main_FuncTestScene.unity";
                string SubFuncTestPath1 = "Assets/4.Scenes/FuncTestScene/Sub_FuncTest_StaticObj.unity";
                string SubFuncTestPath2 = "Assets/4.Scenes/FuncTestScene/Sub_FuncTest_TestObj.unity";
                string SubFuncTestPath3 = "Assets/4.Scenes/FuncTestScene/Sub_FuncTest_AI.unity";
                string ArtTestPath = "Assets/4.Scenes/ArtTestScene/ArtTestScene.unity";
                EditorSceneManager.OpenScene(MainFuncTestPath);
                EditorSceneManager.OpenScene(SubFuncTestPath1, OpenSceneMode.Additive);
                EditorSceneManager.OpenScene(SubFuncTestPath2, OpenSceneMode.Additive);
                EditorSceneManager.OpenScene(SubFuncTestPath3, OpenSceneMode.Additive);
                EditorSceneManager.OpenScene(ArtTestPath, OpenSceneMode.Additive);
            }

            if (GUILayout.Button("打开美术测试关卡"))
            {
                string ArtTestPath = "Assets/4.Scenes/ArtTestScene/ArtTestScene.unity";
                EditorSceneManager.OpenScene(ArtTestPath);
            }

            if (GUILayout.Button("打开曲线编辑关卡"))
            {
                string BezierEditorPath = "Assets/4.Scenes/BezierEditorScene.unity";
                EditorSceneManager.OpenScene(BezierEditorPath);
            }

            if (GUILayout.Button("打开综合测试场景"))
            {
                string MainFuncTestPath = "Assets/4.Scenes/FuncTestScene/Main_FuncTestScene.unity";
                string SubFuncTestPath2 = "Assets/4.Scenes/ComprehensiveTestScene.unity";
                string SubFuncTestPath3 = "Assets/4.Scenes/FuncTestScene/Sub_FuncTest_AI.unity";
                string ArtTestPath = "Assets/4.Scenes/ArtTestScene/ArtTestScene.unity";
                EditorSceneManager.OpenScene(MainFuncTestPath);
                EditorSceneManager.OpenScene(SubFuncTestPath2, OpenSceneMode.Additive);
                EditorSceneManager.OpenScene(SubFuncTestPath3, OpenSceneMode.Additive);
                EditorSceneManager.OpenScene(ArtTestPath, OpenSceneMode.Additive);
            }

            if (GUILayout.Button("打开BaseCamp"))
            {
                string MainFuncTestPath = "Assets/4.Scenes/FuncTestScene/Main_FuncTestScene.unity";
                string BaseCamp = "Assets/4.Scenes/BaseCamp0_0.unity";
                string SubFuncTestPath3 = "Assets/4.Scenes/FuncTestScene/Sub_FuncTest_AI.unity";
                string ArtTestPath = "Assets/4.Scenes/ArtTestScene/ArtTestScene.unity";
                EditorSceneManager.OpenScene(MainFuncTestPath);
                EditorSceneManager.OpenScene(BaseCamp, OpenSceneMode.Additive);
                EditorSceneManager.OpenScene(SubFuncTestPath3, OpenSceneMode.Additive);
                EditorSceneManager.OpenScene(ArtTestPath, OpenSceneMode.Additive);
            }

            if (GUILayout.Button("打开测试Bsk"))
            {
                string SubFuncTestPath1 = "Assets/4.Scenes/FuncTestScene/Sub_FuncTest_StaticObj.unity";
                string NewBskTest = "Assets/4.Scenes/NewBskScene.unity";
                EditorSceneManager.OpenScene(SubFuncTestPath1);
                EditorSceneManager.OpenScene(NewBskTest, OpenSceneMode.Additive);
            }

            GUILayout.FlexibleSpace();
            if (GUILayout.Button("编写相关脚本"))
            {
                Object asset =
                    AssetDatabase.LoadAssetAtPath("Assets/Editor/LevelManager/LevelManagerWindow.cs",
                        typeof(UnityEngine.Object));
                AssetDatabase.OpenAsset(asset, 15);
            }

            GUILayout.EndHorizontal(); // 结束横向布局

            #endregion

            GUILayout.EndVertical();
            GUILayout.FlexibleSpace(); // 添加垂直弹性空间，将按钮居中

            #endregion
        }
    }
}