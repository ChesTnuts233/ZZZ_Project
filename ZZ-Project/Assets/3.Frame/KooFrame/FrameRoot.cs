using UnityEngine;

namespace KooFrame
{
    using UnityEngine.SceneManagement;
    using System.Collections.Generic;


#if UNITY_EDITOR
    using UnityEditor;

    [InitializeOnLoad]
#endif

    [DefaultExecutionOrder(-50)]
    public class FrameRoot : MonoBehaviour
    {
        private FrameRoot() { }

        private static FrameRoot instance;

        public static FrameRoot Instance => instance;

        public static Transform RootTransform { get; private set; }


        public static FrameSetting Setting
        {
            get => instance.frameSetting;
        }

        // 框架层面的配置文件
        [SerializeField] private FrameSetting frameSetting;


        public GameObject eventSystem;

        private void Awake()
        {
            // 防止Editor下的Instance已经存在，并且是自身
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            RootTransform = transform;
            DontDestroyOnLoad(gameObject);
            Init();
#if UNITY_EDITOR
            //如果是UI测试场景
            if (SceneManager.GetActiveScene().name == "UIFrameTest")
            {
                if (EditorApplication.isPlaying)
                {
                    //执行一次初始化
                    UI_WindowBase[] window = instance.transform.GetComponentsInChildren<UI_WindowBase>();
                    foreach (UI_WindowBase win in window)
                    {
                        win.Init();
                    }
                }
            }

#endif
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            InitSystems();
            //InitManagers();
            "Root初始化完毕".Log();
        }


        private void InitSystems()
        {
            PoolSystem.Init();
            EventBroadCastSystem.Init();
            MonoSystem.Init();
            SaveSystem.Init();
            //GameSaveSystem.Init();
            LocalizationSystem.Init();
            KeyTipSystem.Init();
            InputMgrSystem.Init();
            UISystem.Init();
            AudioSystem.Init();
        }
//todo
//         #region GamePlayer
//
//         private void InitManagers()
//         {
//             LevelSystem.Init();
//             ItemDic.Init();
//             CharacterDic.Init();
//             AudioDic.Init();
//             PlayerCenter.Init();
//             AStarAlgorithm.Init();
//
// #if UNITY_EDITOR
//
//             //先获取关卡数据
//             LevelManagerDatas leveldata = ResSystem.LoadAsset<LevelManagerDatas>("LevelManagerDatas");
//             Dictionary<string, string> levelIDDic = new Dictionary<string, string>();
//
//             //然后注册场景名称对应LevelID的字典
//             foreach (var data in leveldata.Datas)
//             {
//                 levelIDDic.Add(data.LevelName, data.LevelID);
//             }
//
//             bool find = false;
//             //编辑器刚启动的时候初始化加载A*
//             for (int i = 0; i < SceneManager.sceneCount; i++)
//             {
//                 //遍历当前打开的场景，获取名称
//                 string sceneName = SceneManager.GetSceneAt(i).name;
//
//                 //如果有对应的
//                 if (levelIDDic.TryGetValue(sceneName, out string levelID))
//                 {
//                     //那就加载障碍信息
//                     AStarAlgorithm.LoadSceneObstacles(levelID);
//                     find = true;
//                     break;
//                 }
//             }
//
//             //如果不是在主菜单中，并且没有找到对应对应的场景障碍信息
//             if (!find && SceneManager.GetActiveScene().name != "StartScene")
//             {
//                 //那就警告
//                 Debug.LogWarning("未能找到对应的场景障碍信息，当前关卡障碍信息没加载！");
//             }
//
// #endif
//         }
//
//         #endregion


        #region Editor

#if UNITY_EDITOR
        // 编辑器专属事件系统
        public static EventModule EditorEventModule;

        static FrameRoot()
        {
            EditorEventModule = new EventModule();
            EditorApplication.update += () => { InitForEditor(); };
        }

        [InitializeOnLoadMethod]
        public static void InitForEditor()
        {
            // 当前是否要进行播放或准备播放中
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }

            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<FrameRoot>();
                if (instance == null) return;


                instance.frameSetting.InitOnEditor();

                //如果是UI测试场景
                if (SceneManager.GetActiveScene().name == "UIFrameTest")
                {
                    return;
                }

                // 场景的所有窗口都进行一次Show
                UI_WindowBase[] window = instance.transform.GetComponentsInChildren<UI_WindowBase>();
                foreach (UI_WindowBase win in window)
                {
                    win.ShowGeneralLogic(-1);
                }
            }
        }
#endif

        #endregion
    }
}