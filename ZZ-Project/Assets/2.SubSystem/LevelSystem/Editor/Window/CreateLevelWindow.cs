//****************** 代码文件申明 ************************
//* 文件：CreateLevelWindow                                       
//* 作者：Koo
//* 创建时间：2024/02/21 21:05:20 星期三
//* 功能：创建Level的窗口
//*****************************************************

using System.IO;
using GameBuild;
using GameEditor.Data;
using KooFrame;
using Sirenix.OdinInspector;
using SubSystem.Map;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameEditor
{
    public class CreateLevelWindow : EditorWindow
    {
        /// <summary>
        /// 创建的关卡名称
        /// </summary>
        public string CreateLevelName;

        /// <summary>
        /// 创建的关卡ID
        /// </summary>
        public string CreateLevelID;


        private void OnGUI()
        {
            CreateLevelName = EditorGUILayout.TextField("关卡名称", CreateLevelName);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("关卡ID");

            //显示按钮选项
            string btnLabel = string.IsNullOrEmpty(CreateLevelID) ? "点击选择一个LevelID" : CreateLevelID;
            if (GUILayout.Button(btnLabel))
            {
                //显示编辑窗口
                LevelIDEditorWindow.ShowWindow((levelID) =>
                {
                    //赋值
                    CreateLevelID = levelID;
                });
            }

            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("创建关卡数据"))
            {
                CreateLevelData();
            }
        }


        [Button("创建关卡数据")]
        public void CreateLevelData()
        {
            //Undo.RecordObject(this, "LevelManager(CreateLevelData)");
            //创建新的LevelData
            LevelData levelData = CreateInstance<LevelData>();
            levelData.guid = GUID.Generate().ToString();
            levelData.OnDataSelected = LevelManagerEditorWindow.OnDataSelectionChanged;
            levelData.LevelID = CreateLevelID;
            levelData.LevelName = CreateLevelName;
            levelData.name = CreateLevelID;


            //添加到LevelDatas
            LevelManagerEditorWindow.LevelManagerDatas.Datas.Add(levelData);
            //添加到资源集中
            if (!Application.isPlaying)
            {
                AssetDatabase.AddObjectToAsset(levelData, LevelManagerEditorWindow.LevelManagerDatas);
            }

            //注册撤销
            //Undo.RegisterCreatedObjectUndo(levelData, "LevelManager(CreateLevelData)");
            //保存资源
            AssetDatabase.SaveAssets();
            //List列表刷新
            LevelManagerEditorWindow.RefreshListView();
            //添加场景资源到levelData中
            SceneData sceneData = CreateSceneData(LevelManagerEditorWindow.LevelManagerDatas);
            levelData.sceneDatas.Add(sceneData);

            //创建对应的网格数据
            MapData mapData = ScriptableObject.CreateInstance<MapData>();
            mapData.LevelID = levelData.LevelID;
            mapData.name = levelData.LevelName;
            //保存网格SO数据
            if (Application.isEditor)
            {
                AssetDatabase.CreateAsset(mapData,
                    "Assets/8.Data/Scene/SceneData/" + levelData.LevelName + ".asset");
                AssetDatabase.Refresh();
            }

            Scene createScene = EditorSceneManager
                .OpenScene(LevelManagerEditorWindow.LevelSavePath + "/" + levelData.LevelName + ".unity");
            //打开对应的Scene
            var objs = createScene.GetRootGameObjects();
            //对Grid进行赋值
            foreach (var gameObject in objs)
            {
                if (gameObject.name == "Grid")
                {
                    //gameObject.GetComponent<TilemapGridProperties>().gridProperties = gridProperties;
                    EditorUtility.SetDirty(gameObject);
                }

                if (gameObject.name == "Areas")
                {
                    //gameObject.GetComponent<AreasManager>().LevelID = CreateLevelID;
                    EditorUtility.SetDirty(gameObject);
                }
            }

            EditorSceneManager.SaveScene(createScene);


            levelData.MapData = mapData;
            LevelManagerEditorWindow.LevelManagerDatas.MapDatas.Add(mapData);


            // // 获取Build Settings中的场景列表 用Addressable代替
            // EditorBuildSettingsScene[] editorBuildSettingsScenes = EditorBuildSettings.scenes;
            // //新建的场景添加到BuildSettings
            // foreach (var levelDataSceneData in levelData.sceneDatas)
            // {
            //     // 检查场景是否已经存在于Build Settings中
            //     bool sceneAlreadyAdded = false;
            //     foreach (var buildSettingsScene in editorBuildSettingsScenes)
            //     {
            //         if (buildSettingsScene.path == levelDataSceneData.ScenePath)
            //         {
            //             sceneAlreadyAdded = true;
            //             break;
            //         }
            //     }
            //
            //     // 如果场景未在Build Settings中，则将其添加
            //     if (!sceneAlreadyAdded)
            //     {
            //         EditorBuildSettingsScene newScene =
            //             new EditorBuildSettingsScene(levelDataSceneData.ScenePath, true);
            //         ArrayUtility.Add(ref editorBuildSettingsScenes, newScene);
            //         EditorBuildSettings.scenes = editorBuildSettingsScenes;
            //         Debug.Log("Scene added to Build Settings: " + levelDataSceneData.ScenePath);
            //     }
            //     else
            //     {
            //         Debug.Log("Scene is already in Build Settings: " + levelDataSceneData.ScenePath);
            //     }
            // }

#if ENABLE_ADDRESSABLES
            foreach (var levelDataSceneData in levelData.sceneDatas)
            {
                //在Addressable中添加场景文件
                KooTool.AddToAddressable(levelDataSceneData.ScenePath, levelDataSceneData.SceneName, "Scenes");
            }
#endif


            EditorUtility.SetDirty(LevelManagerEditorWindow.LevelManagerDatas);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            //关闭窗口
            Close();
            Debug.Log("新建关卡数据完毕");
        }


        /// <summary>
        /// 创建关卡数据
        /// </summary>
        private SceneData CreateSceneData(LevelManagerDatas managerDatas)
        {
            string destScenePath = LevelManagerEditorWindow.LevelSavePath + "/" + CreateLevelName + ".unity";

            //通过样板复制出场景文件
            if (File.Exists(destScenePath))
            {
                File.Delete(destScenePath);
            }

            File.Copy("Assets/4.Scenes/Level/Template/TemplateScene.unity",
                destScenePath);


            //刷新资源
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorSceneManager.OpenScene(destScenePath);

            Scene levelBaseScene = SceneManager.GetSceneByPath(destScenePath);
            //设置创建场景的名称

            SceneData sceneData = new SceneData(levelBaseScene);


            if (sceneData.SceneName.IsNullOrEmpty() || sceneData.ScenePath.IsNullOrEmpty())
            {
                sceneData.ScenePath = destScenePath;
                sceneData.SceneName = CreateLevelName;
            }


            EditorUtility.SetDirty(LevelManagerEditorWindow.LevelManagerDatas);

            return sceneData;
        }
    }
}