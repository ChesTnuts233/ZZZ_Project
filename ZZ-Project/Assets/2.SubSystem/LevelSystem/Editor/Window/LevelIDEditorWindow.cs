//****************** 代码文件申明 ************************
//* 文件：LevelIDEditorWindow                                       
//* 作者：wheat
//* 创建时间：2024/02/26 07:38:50 星期一
//* 描述：一个窗口可以选择LevelID
//*****************************************************

using UnityEngine;
using System;
using System.Collections.Generic;
using GameEditor;
using UnityEditor;
using GameEditor.Data;
using KooFrame;

namespace GameBuild
{
    public class LevelIDEditorWindow : EditorWindow
    {
        public static LevelIDEditorWindow instance;
        public static LevelManagerDatas LevelManagerDatas;
        private Action<string> action;
        private List<GUIContent> options;
        private string searchText;
        private string newLevelMainID;
        private string newLevelSubID;

        private Vector2 scrollPosition;

        public static bool IsChangeLevelData = false;

        private void Awake()
        {
            instance = this;
        }

        private void OnEnable() { }

        private void OnDisable()
        {
            instance = null;
        }

        /// <summary>
        /// 显示窗口
        /// </summary>
        public static void ShowWindow(Action<string> action, bool isChangeLevelData = false)
        {
            if (instance != null)
            {
                instance.Close();
            }

            //初始获取关卡信息
            LevelManagerDatas = GetLevelDatas();
            IsChangeLevelData = isChangeLevelData;

            //如果无法获取关卡信息
            if (LevelManagerDatas == null)
            {
                //那就提醒
                Debug.Log("无法获取关卡信息");
            }
            else
            {
                //创建窗口
                LevelIDEditorWindow window = CreateInstance<LevelIDEditorWindow>();

                //初始化
                window.Init(action, isChangeLevelData);

                //显示窗口
                window.ShowUtility();

                //移动到鼠标旁边
                Vector2 mousePosition = Event.current.mousePosition;
                Vector2 screenMousePosition = GUIUtility.GUIToScreenPoint(mousePosition) + new Vector2(0, 25f);
                window.position = new Rect(screenMousePosition, window.position.size);
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init(Action<string> action, bool isChangeLevelData = false)
        {
            this.action = action;
            searchText = string.Empty;
            titleContent.text = "点击选择LevelID";

            //初始化选项
            options = new List<GUIContent>();


            for (int i = 0; i < LevelManagerDatas.Datas.Count; i++)
            {
                //如果没有 添加
                if (!LevelManagerDatas.LevelIDs.Contains(LevelManagerDatas.Datas[i].LevelID))
                {
                    LevelManagerDatas.LevelIDs.Add(LevelManagerDatas.Datas[i].LevelID);
                }
            }

            for (var i = 0; i < LevelManagerDatas.LevelIDs.Count; i++)
            {
                options.Add(new GUIContent(LevelManagerDatas.LevelIDs[i]));
            }
        }

        /// <summary>
        /// 绘制GUI
        /// </summary>
        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();


            EditorGUILayout.BeginHorizontal();


            //显示搜索栏
            searchText = EditorGUILayout.TextField("输入ID进行搜索", searchText);
            EditorGUILayout.EndHorizontal();


            //显示添加新的LevelID
            EditorGUILayout.BeginHorizontal();

            //显示输入栏
            newLevelMainID = EditorGUILayout.TextField(newLevelMainID, GUILayout.Width(30f));
            EditorGUILayout.LabelField("_", GUILayout.Width(13f));
            newLevelSubID = EditorGUILayout.TextField(newLevelSubID, GUILayout.Width(30f));

            //添加LevelID按钮
            if (GUILayout.Button("添加LevelID", GUILayout.Width(100f)))
            {
                //检查新的ID是否已经存在了
                string newID = newLevelMainID + "_" + newLevelSubID;
                if (!LevelManagerDatas.LevelIDs.Exists((id) => id.Equals(newID)) && !(newID.Equals("_")))
                {
                    //不存在就添加
                    LevelManagerDatas.LevelIDs.Add(newID);
                    options.Add(new GUIContent(newLevelMainID + "_" + newLevelSubID));
                    //保存
                    AssetDatabase.SaveAssets();
                }
                else
                {
                    //已经存在
                    Debug.LogWarning("此ID已经存在或者为非法ID");
                }
            }

            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginScrollView(scrollPosition);

            //如果没有选项
            if (options.Count == 0)
            {
                //那就提示现在没有选项
                EditorGUILayout.LabelField("当前没有关卡选项");
            }
            else
            {
                //显示选项
                for (int i = 0; i < options.Count; i++)
                {
                    //搜索
                    if (string.IsNullOrEmpty(searchText) == false &&
                        LevelManagerDatas.LevelIDs[i].Contains(searchText) == false) continue;

                    EditorGUILayout.BeginHorizontal();

                    if (GUILayout.Button(options[i]))
                    {
                        action?.Invoke(LevelManagerDatas.LevelIDs[i]);
                        if (IsChangeLevelData)
                        {
                            LevelInspectorViewer.CurInspectorLevelData.UpdateLevelDataID(LevelManagerDatas.LevelIDs[i]);
                        }

                        Close();
                    }

                    if (GUILayout.Button("删除此LevelID"))
                    {
                        LevelManagerDatas.LevelIDs.RemoveAt(i);
                        options.RemoveAt(i);
                    }

                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.EndVertical();
        }


        /// <summary>
        /// 获取关卡信息
        /// </summary>
        /// <returns></returns>
        public static LevelManagerDatas GetLevelDatas()
        {
            if (LevelManagerDatas == null)
            {
                LevelManagerDatas = ResSystem.LoadAsset<LevelManagerDatas>("LevelManagerDatas");
            }

            return LevelManagerDatas;
        }
    }
}