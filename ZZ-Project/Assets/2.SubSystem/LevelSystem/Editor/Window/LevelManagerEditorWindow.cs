#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameEditor;
using GameEditor.Data;
using KooFrame;
using SubSystem.Map;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LevelManagerEditorWindow : EditorWindow
{
    #region 数据相关

    /// <summary>
    /// 关卡数据
    /// </summary>
    [SerializeField] public static LevelManagerDatas LevelManagerDatas;

    public static string LevelSavePath = "Assets/4.Scenes/Level";

    /// <summary>
    /// 默认关卡数据路径
    /// </summary>
    public static string DefaultDataPath = "Assets/2.SubSystem/8.LevelManager/Data/LevelManagerDatas.asset";

    /// <summary>
    /// 通用场景路径
    /// </summary>
    public static string MainScenePath = "Assets/4.Scenes/Level/Main/通用场景.unity";


    public static string StartScenePath = "Assets/4.Scenes/Game/StartScene.unity";

    public static string UITestScenePath = "Assets/4.Scenes/TestScene/UIFrameTest.unity";

    /// <summary>
    /// 当前的关卡
    /// </summary>
    public static Label CurShowLevelID;

    #endregion

    #region 页面相关

    private StyleSheet _styleSheet;
    private VisualTreeAsset _visualTree;
    private static ListView _levelListView;
    private VisualElement _leftVisualElement;
    private VisualElement _rightVisualElement;


    private Button _addLevelDataBtn;
    private Button _deleteLevelDataBtn;
    private Button _startSceneBtn;
    private Button _uiTestSceneBtn;
    private Button _foldBtn;

    private bool _isFold;

    private static LevelInspectorViewer _levelInspectorViewer;

    private static LevelManagerEditorWindow window;

    #endregion

    public static void ShowWindow()
    {
        window = GetWindow<LevelManagerEditorWindow>();
        window.titleContent = new GUIContent("关卡场景管理器");
    }


    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        //VisualElements对象可以按照树的层次结构包含其他VisualElement。
        //导入UXML
        _visualTree =
            AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                "Assets/2.SubSystem/8.LevelManager/Editor/LevelManagerEditorWindow.uxml");
        _visualTree.CloneTree(root);

        //样式表添加到VisualElement中。
        //该样式将应用于VisualElement及其所有子元素
        _styleSheet =
            AssetDatabase.LoadAssetAtPath<StyleSheet>(
                "Assets/2.SubSystem/8.LevelManager/Editor/LevelManagerEditorWindow.uss");
        //绑定折叠按钮
        _foldBtn = root.Q<Button>("FoldBtn");

        _leftVisualElement = root.Q<VisualElement>("LeftDiv");
        _rightVisualElement = root.Q<VisualElement>("RightDiv");

        //绑定List展示列表
        _levelListView = root.Q<ListView>();


        //绑定按钮
        _addLevelDataBtn = root.Q<Button>("AddLevelDataBtn");
        //绑定删除按钮
        _deleteLevelDataBtn = root.Q<Button>("DeleteLevelDataBtn");
        //绑定属性显示器
        _levelInspectorViewer = root.Q<LevelInspectorViewer>();
        // //绑定节点树面板
        // _nodeTreeViewer = root.Q<NodeTreeViewer>();

        //绑定开始场景按钮
        _startSceneBtn = root.Q<Button>("StartSceneBtn");

        //绑定当前场景名称显示
        CurShowLevelID = root.Q<Label>("CurLevelName");
        //绑定UI测试场景
        _uiTestSceneBtn = root.Q<Button>("UITestBtn");


        //按钮绑定事件
        ButtonRegisterOnClick();

        RefreshListView();
        GetSelectionLevelDatas();
        LoadGridData();
        UpdateCurLevelName();
    }

    private void ButtonRegisterOnClick()
    {
        _addLevelDataBtn.clicked += CreateLevelData;
        _deleteLevelDataBtn.clicked += DeleteLevelData;
        _startSceneBtn.clicked += () =>
        {
            EditorSceneManager.OpenScene(StartScenePath);
            LevelManagerEditorWindow.UpdateCurLevelName();
        };
        _uiTestSceneBtn.clicked += () =>
        {
            EditorSceneManager.OpenScene(UITestScenePath);
            LevelManagerEditorWindow.UpdateCurLevelName();
        };
        _foldBtn.clicked += () =>
        {
            _isFold = !_isFold;
            if (_isFold)
            {
                _foldBtn.text = "\u25c0";
                _leftVisualElement.style.flexGrow = 1;
                _rightVisualElement.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            }
            else
            {
                _foldBtn.text = "\u25b6";
                _leftVisualElement.style.flexGrow = 0.15f;
                _rightVisualElement.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
            }
        };
    }


    public static void RefreshListView()
    {
        if (LevelManagerDatas != null)
        {
            CreateListView(LevelManagerDatas.Datas);
        }

        _levelListView.Rebuild();
    }

    public static void UpdateCurLevelName()
    {
        CurShowLevelID.text = "当前所在场景名称：" + SceneManager.GetSceneAt(0).name;
    }

    private void OnSelectionChange()
    {
        GetSelectionLevelDatas();
    }

    private void LoadGridData()
    {
        if (LevelManagerDatas != null)
        {
            // LevelManagerDatas.GridPropertyList =
            //     AssetDatabase.LoadAssetAtPath<SO_GridPropertyList>("Assets/8.Data/Scene/GridPropertyList.asset");
        }
    }

    private void GetSelectionLevelDatas()
    {
        //检测选中对象是否是关卡数据集
        LevelManagerDatas = Selection.activeObject as LevelManagerDatas;
        if (Application.isPlaying)
        {
            if (LevelManagerDatas)
            {
                if (_levelListView != null)
                {
                    CreateListView(LevelManagerDatas.Datas);
                    _levelListView.Rebuild();
                }
            }
        }
        else
        {
            if (LevelManagerDatas && AssetDatabase.CanOpenAssetInEditor(LevelManagerDatas.GetInstanceID()))
            {
                if (LevelManagerDatas != null)
                {
                    CreateListView(LevelManagerDatas.Datas);
                    _levelListView.Rebuild();
                }
            }
        }

        //如果选中的为空 则查找默认路径
        if (LevelManagerDatas == null)
        {
            if (KooTool.CheckUnityFileExists(DefaultDataPath))
            {
                LevelManagerDatas = AssetDatabase.LoadAssetAtPath<LevelManagerDatas>(
                    DefaultDataPath);
                CreateListView(LevelManagerDatas.Datas);
                _levelListView.Rebuild();
            }
        }
    }


    public static void OnDataSelectionChanged(IEnumerable<object> enumerable)
    {
        List<LevelData> levelDataList = enumerable.OfType<LevelData>().ToList();
        // 如果有LevelData类型的元素
        if (levelDataList.Any())
        {
            _levelInspectorViewer.UpdateSelection(levelDataList[0]);
        }
    }

    private void CreateLevelData()
    {
        if (LevelManagerDatas == null)
        {
            "请选中关卡数据的SO文件".Log();
            return;
        }

        var createWindow = EditorWindow.GetWindow<CreateLevelWindow>(true);
    }


    private void DeleteLevelData()
    {
        if (_levelListView.selectedItem == null)
        {
            Debug.Log("没有选中任何关卡数据");
        }
        else
        {
            if (!EditorUtility.DisplayDialog("删除场景", "是否删除选中数据", "是", "否")) return;

            Undo.RecordObject(this, "LevelManager(DeleteLevelData)");
            LevelData data = _levelListView.selectedItem as LevelData;


            //删除对应的场景文件
            if (data != null)
            {
                foreach (var dataSceneData in data.sceneDatas)
                {
                    AssetDatabase.DeleteAsset(dataSceneData.ScenePath);
                }
            }

            string gridSOPath = "Assets/8.Data/Scene/SceneData/" + data.LevelName + ".asset";

            LevelManagerDatas.MapDatas.Remove(AssetDatabase.LoadAssetAtPath<MapData>(gridSOPath));


            //删除网格数据文件
            if (File.Exists(KooTool.ConvertAssetPathToSystemPath(gridSOPath)))
            {
                AssetDatabase.DeleteAsset(gridSOPath);
            }

            //移除Addressable中的资源


            //总节点列表移出节点
            LevelManagerDatas.Datas.Remove(data);


            //保存当前项目中的所有修改过的资产（Assets）到磁盘上的项目文件
            Undo.DestroyObjectImmediate(data);

            //设置脏数据 防止Unity没能自动保存
            EditorUtility.SetDirty(LevelManagerDatas);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            RefreshListView();
        }
    }


    protected static void CreateListView(List<LevelData> values)
    {
        if (values == null || values.Count == 0)
            return;

        var item = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
            "Assets/2.SubSystem/8.LevelManager/Editor/ListItem.uxml");
        Func<VisualElement> makeItem = () => item.Instantiate();
        Action<VisualElement, int> bindItem = (visualElement, index) =>
        {
            if (index >= values.Count || values[index] == null)
            {
                _levelListView.Remove(visualElement);
                return;
            }

            LevelDataView levelDataView = visualElement.Q<LevelDataView>("LevelDataView");
            levelDataView.ChangeLevelBtn = visualElement.Q<Button>("ChangeToLevelBtn");
            levelDataView.Init(values[index]);
            visualElement.Q<Label>("Name").text = values[index].LevelName;
        };
        _levelListView.makeItem = makeItem;
        _levelListView.bindItem = bindItem;
        _levelListView.itemsSource = values;
        _levelListView.selectionType = SelectionType.Single;
        _levelListView.selectionChanged += OnDataSelectionChanged;
    }


    private void GeneratorLevel(LevelData levelData) { }


    // /// <summary>
    // /// 动态生成枚举代码
    // /// </summary>
    // public static void GeneratorEnum()
    // {
    //     //生成关卡名称枚举代码
    //     string codePath = "Assets/1.GameBuild/11.Enums/LevelName.cs";
    //     string codeContent =
    //         File.ReadAllText(KooTool.ConvertAssetPath(codePath));
    //     string startStr = "        #region 代码动态生成区域";
    //     string endStr = "        #endregion //代码结束区域";
    //     int startIndex = codeContent.IndexOf(startStr) + startStr.Length;
    //     int endIndex = codeContent.IndexOf(endStr);
    //     string defineStr = "";
    //     for (var index = 0; index < LevelManagerEditorWindow.LevelNames.Count; index++)
    //     {
    //         var levelName = LevelManagerEditorWindow.LevelNames[index];
    //         defineStr += $"\n        {levelName} = {index},";
    //     }
    //
    //     //结束换行
    //     defineStr += "\n";
    //     string dynamicContents = "";
    //     {
    //         //先将start和end之间的代码删除干净
    //         dynamicContents = codeContent.Remove(startIndex, endIndex - startIndex);
    //         //在end前插入defineStr
    //         endIndex = dynamicContents.IndexOf(endStr);
    //         dynamicContents = dynamicContents.Insert(endIndex, defineStr);
    //     }
    //     File.WriteAllText(codePath, dynamicContents);
    //     AssetDatabase.Refresh();
    // }
}
#endif