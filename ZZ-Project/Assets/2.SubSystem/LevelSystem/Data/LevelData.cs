using System;
using System.Collections.Generic;
using GameBuild;
using KooFrame;
using Sirenix.OdinInspector;
using SubSystem.Map;
using UnityEngine;
using UnityEngine.SceneManagement;
using Path = System.IO.Path;

#if UNITY_EDITOR
using SubSystem.LevelSystem;
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace GameEditor.Data
{
    /// <summary>
    /// 关卡数据  Level中有多个Scene(Unity叠加的Scene主要也是方便管理)
    /// </summary>
    public class LevelData : ConfigBase_SO
    {
        /// <summary>
        /// 此关卡数据的ID
        /// </summary>
        [LevelID] public string LevelID;

        /// <summary>
        /// 关卡名称
        /// </summary>
        public string LevelName;

        /// <summary>
        /// 此数据的GUID
        /// </summary>
        [HideInInspector] public string guid;


        /// <summary>
        /// 节点选中时候的Action
        /// </summary>
        public Action<IEnumerable<object>> OnDataSelected;

        /// <summary>
        /// 关卡有那些Scene场景数据
        /// </summary>
        [SerializeField] public List<SceneData> sceneDatas = new();

        /// <summary>
        /// 关卡的网格数据
        /// </summary>
        public MapData MapData;


        #region 关卡操作

        /// <summary>
        /// 切换到此关卡
        /// </summary>
        public LevelData ChangeToThis()
        {
            //如果是编辑器下调用编辑器的传送方式
            return Application.isPlaying ? ChangeToLevelInRunTime() : ChangeToLevelInEditor();
        }

        private LevelData ChangeToLevelInRunTime()
        {
            return null;
        }

        private LevelData ChangeToLevelInEditor()
        {
            return null;
        }

        #endregion


        #if UNITY_EDITOR

        [Button("更新LevelID,将同步修改相关资源")]
        public void UpdateLevelDataID(string newID)
        {
            //修改资源名称
            foreach (var datasData in LevelSystem.ManagerDatas.Datas)
            {
                if (datasData.LevelName == LevelName)
                {
                    var data = datasData;
                    data.name = newID;
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }

            //修改SO网格的ID
            MapData.LevelID = newID;


            //获取一下当前所在的场景名字 

            string oldLevelID = LevelSystem.CurLevelID;

            // if (LevelSystem.CurLevelID != newID)
            // {
            //     //切换关卡
            //     var scene = ChangeToCurLevelInEditor();
            //     var gos = scene.GetRootGameObjects();
            //     foreach (var gameObject in gos)
            //     {
            //         if (gameObject.TryGetComponent<AreasManager>(out var areas))
            //         {
            //             areas.LevelID = newID;
            //         }
            //     }
            // }
            // else
            // {
            //     //直接找到Areas修改
            //     GameObject.FindObjectOfType<AreasManager>().LevelID = newID;
            // }
        }

        /// <summary>
        /// 编辑器下场景配置切换到此关卡
        /// </summary>
        /// <returns></returns>
        public Scene ChangeToCurLevelInEditor()
        {
            Scene firstScene = default;
            //如果当前管理器中
            if (LevelSystem.CurLevelID != LevelID || SceneManager.GetSceneAt(0).name != LevelName)
            {
                LevelSystem.CurLevelID = LevelID;
                for (var index = 0; index < sceneDatas.Count; index++)
                {
                    var sceneData = sceneDatas[index];
                    if (index == 0)
                    {
                        firstScene = EditorSceneManager.OpenScene(sceneData.ScenePath);
                    }
                    else
                    {
                        EditorSceneManager.OpenScene(sceneData.ScenePath, OpenSceneMode.Additive);
                    }

                    if (string.IsNullOrEmpty(sceneData.SceneName))
                    {
                        sceneData.SceneName = Path.GetFileNameWithoutExtension(sceneData.ScenePath);
                        EditorUtility.SetDirty(LevelSystem.ManagerDatas);
                        AssetDatabase.SaveAssets();
                    }
                }

                Scene mainScene =
                    EditorSceneManager.OpenScene("Assets/4.Scenes/Level/Main/通用场景.unity", OpenSceneMode.Additive);
                SceneManager.SetActiveScene(mainScene);
            }

            return firstScene;
        }

    #endif
    }
}