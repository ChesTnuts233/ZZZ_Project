// //****************** 代码文件申明 ************************
// //* 文件：LevelSystem                                      
// //* 作者：wheat
// //* 创建时间：2023/09/12 13:36:12 星期二
// //* 功能：用于管理、储存、加载等场景关卡
// //*****************************************************
//
// using KooFrame;
// using Sirenix.OdinInspector;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using GameEditor.Data;
// using UnityEngine.SceneManagement;
// using SubSystem;
// using UnityEngine.AddressableAssets;
// using UnityEngine.ResourceManagement.AsyncOperations;
// using UnityEngine.ResourceManagement.ResourceProviders;
// using System;
// using Debug = UnityEngine.Debug;
//
// #if UNITY_EDITOR
//
// #endif
//
// #if RL_DEBUG
//
// using IngameDebugConsole;
//
// #endif
//
// namespace GameBuild
// {
//     /// <summary>
//     /// 切换场景的处理信息
//     /// </summary>
//     public class SwitchLevelModifyData
//     {
//         /// <summary>
//         /// 要切换的关卡ID
//         /// </summary>
//         public string SwitchLevelID = "";
//
//         /// <summary>
//         /// 重生点名称
//         /// </summary>
//         public string SpawnPosName = "";
//
//         /// <summary>
//         /// 渐入动画
//         /// </summary>
//         public bool FadeIn = true;
//
//         /// <summary>
//         /// 启用加载中
//         /// </summary>
//         public bool EnableLoading = true;
//
//         /// <summary>
//         /// 玩家生成之后的callback
//         /// </summary>
//         public Action<Berserker> callbackAfterPlayerSpawn;
//
//         /// <summary>
//         /// 切换完成后的callback
//         /// </summary>
//         public Action callbackAfterSwitch;
//
//         public SwitchLevelModifyData() { }
//
//         public SwitchLevelModifyData(string levelId, string spawnPosName)
//         {
//             SwitchLevelID = levelId;
//             SpawnPosName = spawnPosName;
//         }
//     }
//
//     public static class LevelSystem
//     {
//         #region 字段
//
//         private static LevelManagerDatas levelDatas;
//         [LabelText("关卡信息字典")] private static Dictionary<string, LevelData> m_LevelDataDic;
//         [SerializeField, LabelText("是否在场景切换中")] private static bool swithingScene;
//
//         /// <summary>
//         /// 是否在场景切换中
//         /// </summary>
//         [SerializeField, LabelText("是否在场景切换中")]
//         public static bool SwithingScene => swithingScene;
//
//         [SerializeField, LabelText("当前所在场景id")] private static string _curLevelID;
//         [SerializeField, LabelText("当前所在场景名称")] private static string _curSceneName;
//
//         [SerializeField, LabelText("当前所选存档")]
//         public static SaveItem CurSave
//         {
//             get => GameSaveSystem.CurSave;
//             set => GameSaveSystem.CurSave = value;
//         }
//
//         [SerializeField, LabelText("当前游玩所选存档")]
//         public static SavePlayData CurSavePlayData
//         {
//             get => GameSaveSystem.CurSavePlayData;
//             set => GameSaveSystem.CurSavePlayData = value;
//         }
//
//         public static string CurLevelID
//         {
//             get => _curLevelID;
//             set
//             {
//                 _curLevelID = value;
// #if UNITY_EDITOR
//                 levelDatas.CurLevelID = value;
// #endif
//             }
//         }
//
//         #endregion
//
//         #region 心中训练场相关字段
//
//         /// <summary>
//         /// 在训练场
//         /// </summary>
//         public static bool InTraningLevel => inTraningLevel;
//
//         /// <summary>
//         /// 在训练场
//         /// </summary>
//         private static bool inTraningLevel;
//
//         /// <summary>
//         /// 上一个场景所在的父级
//         /// </summary>
//         private static List<GameObject> lastSceneParents;
//
//         /// <summary>
//         /// 之前的玩家
//         /// </summary>
//         private static List<Berserker> oldPlayers;
//
//         /// <summary>
//         /// 缓存的动画信息
//         /// </summary>
//         private static Dictionary<Animator, AnimatorStateInfo> cacheAnimInfo;
//
//         /// <summary>
//         /// 上一个所在场景的关卡id
//         /// </summary>
//         private static string oldLevelId;
//
//         /// <summary>
//         /// 上一个所在场景名称
//         /// </summary>
//         private static string oldSceneName;
//
//         /// <summary>
//         /// 原先相机位置
//         /// </summary>
//         private static Vector3 oldCamPos;
//
//         #endregion
//
//         #region 方法
//
//         #region 初始化
//
//         /// <summary>
//         /// 初始化
//         /// </summary>
//         public static void Init()
//         {
//             m_LevelDataDic = new Dictionary<string, LevelData>();
//
//             levelDatas = ResSystem.LoadAsset<LevelManagerDatas>("LevelManagerDatas");
//
//             foreach (LevelData levelData in levelDatas.Datas)
//             {
//                 if (levelData != null)
//                 {
//                     m_LevelDataDic.Add(levelData.LevelID, levelData);
//                 }
//             }
//
// #if UNITY_EDITOR
//
//             //更新LevelId和Name
//             if (string.IsNullOrEmpty(CurLevelID) || string.IsNullOrEmpty(_curSceneName))
//             {
//                 for (int i = 0; i < SceneManager.sceneCount; i++)
//                 {
//                     Scene scene = SceneManager.GetSceneAt(i);
//
//                     if (scene.name != "通用场景")
//                     {
//                         _curSceneName = scene.name;
//                         MonoSystem.Start_Coroutine(SetActiveSceneLate(scene));
//                         break;
//                     }
//                 }
//
//                 foreach (var scene in levelDatas.Datas)
//                 {
//                     if (scene.sceneDatas[0].SceneName == _curSceneName)
//                     {
//                         CurLevelID = scene.LevelID;
//                         break;
//                     }
//                 }
//             }
//
// #endif
//         }
//
// #if UNITY_EDITOR
//         /// <summary>
//         /// 编辑器中使用，等待若干初始化完成后再设置激活场景
//         /// </summary>
//         /// <param name="scene"></param>
//         /// <returns></returns>
//         private static IEnumerator SetActiveSceneLate(Scene scene)
//         {
//             //等一会
//             yield return CoroutineTool.WaitForFrames();
//
//             SceneManager.SetActiveScene(scene);
//         }
// #endif
//
//         #endregion
//
//         #region Private方法
//
//         [Button("卸载场景")]
//         /// <summary>
//         /// 卸载场景并保存场景数据
//         /// </summary>
//         private static void UnloadScene()
//         {
//             //如果没有在切换场景
//             if (!swithingScene)
//             {
//                 //那就开始卸载场景
//                 MonoSystem.Start_Coroutine(UnloadSceneCoroutine(new SwitchLevelModifyData()));
//             }
//         }
//
//         [Button("加载场景")]
//         /// <summary>
//         /// 加载场景
//         /// </summary>
//         private static void LoadScene(SwitchLevelModifyData modifyData)
//         {
//             //如果没有在切换场景那就开始加载场景
//             if (!swithingScene)
//             {
//                 MonoSystem.Start_Coroutine(LoadSceneCoroutine(modifyData));
//             }
//         }
//
//         #region 协程
//
//         #region 训练场相关
//
//         /// <summary>
//         /// 传送训练场的卸载场景的协程
//         /// </summary>
//         /// <returns></returns>
//         private static IEnumerator TrainingUnloadSceneCoroutine(SwitchLevelModifyData modifyData)
//         {
//             //切换场景中设为true
//             swithingScene = true;
//
//             //暂停游戏
//             GameTime.TimeScale = 0f;
//
//             //卸载地图
//             yield return MapManager.UnloadMap();
//
//             //等待一帧
//             yield return CoroutineTool.WaitForEndOfFrame();
//
//             //黑屏渐入
//             yield return LoadGamePanel.FadeIn(true, modifyData.FadeIn);
//             //等待一帧
//             yield return CoroutineTool.WaitForEndOfFrame();
//
//             //记录场景信息
//
//             //记录玩家
//             oldPlayers = PlayerCenter.Players;
//
//             //记录当前场景的物品的父级
//             lastSceneParents = new List<GameObject>();
//             GameObject[] objs = GameObject.FindObjectsOfType<GameObject>();
//             foreach (var obj in objs)
//             {
//                 if (obj.transform.parent == null)
//                 {
//                     if (obj.scene.name != "DontDestroyOnLoad")
//                     {
//                         lastSceneParents.Add(obj);
//                     }
//                 }
//             }
//
//             //关闭GameObject
//             foreach (var parent in lastSceneParents)
//             {
//                 parent.gameObject.SetActive(false);
//             }
//
//             //记录关卡id
//             oldLevelId = CurLevelID;
//             oldSceneName = _curSceneName;
//             oldCamPos = CameraManager.Instance.transform.position;
//
//             swithingScene = false;
//         }
//
//         /// <summary>
//         /// 从训练场传送回来的卸载场景的协程
//         /// </summary>
//         /// <returns></returns>
//         private static IEnumerator TrainingReturnUnloadSceneCoroutine(SwitchLevelModifyData modifyData)
//         {
//             //切换场景中设为true
//             swithingScene = true;
//
//             //暂停游戏
//             GameTime.TimeScale = 0f;
//
//             //卸载地图
//             yield return MapManager.UnloadMap();
//
//             //等待一帧
//             yield return CoroutineTool.WaitForEndOfFrame();
//
//             //黑屏渐入
//             yield return LoadGamePanel.FadeIn(true, modifyData.FadeIn);
//
//             //等待一帧
//             yield return CoroutineTool.WaitForEndOfFrame();
//
//             //把玩家都摧毁
//             PlayerCenter.RemoveAllPlayers();
//
//             //等待一帧
//             yield return CoroutineTool.WaitForEndOfFrame();
//
//             //卸载训练场场景
//             var asnycUnload = SceneManager.UnloadSceneAsync(_curSceneName);
//
//             yield return asnycUnload;
//
//             swithingScene = false;
//         }
//
//         /// <summary>
//         /// 训练场景的加载场景的协程
//         /// </summary>
//         /// <returns></returns>
//         private static IEnumerator TrainingLoadSceneCoroutine(SwitchLevelModifyData modifyData)
//         {
//             swithingScene = true;
//
//             yield return CoroutineTool.WaitForEndOfFrame();
//
//             //设置当前场景名称为新的场景名称
//             inTraningLevel = true;
//
//
//             CurLevelID = modifyData.SwitchLevelID;
//
//
//             LevelData data = m_LevelDataDic[CurLevelID];
//
// #if UNITY_EDITOR
//
//             //如果无法找到对应关卡数据
//             if (data == null)
//             {
//                 Debug.Log($"错误:{modifyData.SwitchLevelID},无法获取对应关卡");
//                 yield break;
//             }
//
// #endif
//
//             //获取场景路径
//             string scenePath = data.sceneDatas[0].SceneName;
//             //获取场景名称
//             _curSceneName = data.sceneDatas[0].SceneName;
//             //加载场景(以场景附加模式)
//             AsyncOperationHandle<SceneInstance> sceneAsync =
//                 Addressables.LoadSceneAsync(scenePath, LoadSceneMode.Additive);
//
//             yield return sceneAsync;
//
//             AsyncOperation load = sceneAsync.Result.ActivateAsync();
//
//             //先禁止关卡自动替换
//             load.allowSceneActivation = false;
//
//             //等待关卡加载完毕
//             while (load.progress < 0.9f)
//             {
//                 yield return CoroutineTool.WaitForEndOfFrame();
//             }
//
//             //加载关卡障碍信息
//             AStarAlgorithm.LoadSceneObstacles(modifyData.SwitchLevelID);
//
//             yield return CoroutineTool.WaitForEndOfFrame();
//
//             //允许场景加载
//             load.allowSceneActivation = true;
//
//             //等待场景加载完毕
//             while (load.isDone == false)
//             {
//                 yield return CoroutineTool.WaitForEndOfFrame();
//             }
//
//             //设置当前的ActiveScene
//             SceneManager.SetActiveScene(sceneAsync.Result.Scene);
//
//             //重新生成玩家
//             PlayerCenter.Players = new List<Berserker>();
//             for (int i = 0; i < oldPlayers.Count; i++)
//             {
//                 //生成和之前玩家数量一样的玩家
//                 var testPlayer = PlayerCenter.SpawnPlayer(AreasManager.Instance.DefaultSpawnPos, false);
//                 //设置玩家的名称
//                 testPlayer.gameObject.name = "Berserker_Train";
//                 //加载训练场景数据
//                 testPlayer.LoadPlayerData(AreasManager.Instance.TraningPlayerSaveData);
//
//                 //回调函数
//                 modifyData.callbackAfterPlayerSpawn?.Invoke(testPlayer);
//             }
//
//             //显示UI
//             PlayerCenter.ShowPlayerUI();
//
//             yield return CoroutineTool.WaitForFrames();
//
//             //重新激活附近房间
//             CameraManager.Instance.ReloadAroundRooms();
//
//             yield return CoroutineTool.WaitForFrames();
//
//             UISystem.GetWindow<GameTotalInfoPanel>().PanelReset();
//
//
//             //恢复时间流动
//             GameTime.TimeScale = 1f;
//
//             //黑屏渐出
//             yield return LoadGamePanel.FadeOut();
//
//             //回调
//             modifyData.callbackAfterSwitch?.Invoke();
//
//             yield return CoroutineTool.WaitForEndOfFrame();
//
//             swithingScene = false;
//         }
//
//         /// <summary>
//         /// 训练场景回来的加载场景的协程
//         /// </summary>
//         /// <returns></returns>
//         private static IEnumerator TrainingReturnLoadSceneCoroutine(SwitchLevelModifyData modifyData)
//         {
//             swithingScene = true;
//
//             yield return CoroutineTool.WaitForEndOfFrame();
//
//             //设置当前关卡id
//             inTraningLevel = false;
//             CurLevelID = oldLevelId;
//             _curSceneName = oldSceneName;
//
//             //加载关卡障碍信息
//             AStarAlgorithm.LoadSceneObstacles(CurLevelID);
//
//             yield return CoroutineTool.WaitForFrames();
//
//             //设置激活的Scene
//             for (int i = 0; i < SceneManager.sceneCount; i++)
//             {
//                 Scene scene = SceneManager.GetSceneAt(i);
//
//                 if (scene.name == _curSceneName)
//                 {
//                     SceneManager.SetActiveScene(scene);
//                     break;
//                 }
//             }
//
//             //重新加载先前关卡
//             foreach (var parent in lastSceneParents)
//             {
//                 parent.gameObject.SetActive(true);
//             }
//
//             yield return CoroutineTool.WaitForEndOfFrame();
//
//             //恢复相机位置
//             CameraManager.Instance.TeleportToPos(oldCamPos);
//
//             //重新激活附近房间
//             CameraManager.Instance.ReloadAroundRooms();
//
//             yield return CoroutineTool.WaitForFrames();
//             //恢复玩家
//             PlayerCenter.Players = oldPlayers;
//             PlayerCenter.ShowPlayerUI();
//
//             yield return CoroutineTool.WaitForEndOfFrame();
//
//             UISystem.GetWindow<GameTotalInfoPanel>().PanelReset();
//
//             //回调
//             modifyData.callbackAfterSwitch?.Invoke();
//
//             //黑屏渐出
//             yield return LoadGamePanel.FadeOut();
//
//             //恢复时间流动
//             GameTime.TimeScale = 1f;
//
//
//             yield return CoroutineTool.WaitForEndOfFrame();
//
//             swithingScene = false;
//         }
//
//         /// <summary>
//         /// 切换训练场场景的协程
//         /// </summary>
//         /// <returns></returns>
//         private static IEnumerator SwitchTrainingSceneCoroutine(SwitchLevelModifyData modifyData)
//         {
//             swithingScene = true;
//
//             //卸载场景
//             yield return TrainingUnloadSceneCoroutine(modifyData);
//
//             //等待一帧
//             yield return CoroutineTool.WaitForFrames();
//
//             //加载新的场景
//             yield return TrainingLoadSceneCoroutine(modifyData);
//
//             swithingScene = false;
//         }
//
//         /// <summary>
//         /// 重新加载训练场场景的协程
//         /// </summary>
//         /// <returns></returns>
//         private static IEnumerator ReloadTrainingSceneCoroutine(SwitchLevelModifyData modifyData)
//         {
//             swithingScene = true;
//
//             modifyData.SwitchLevelID = _curLevelID;
//
//             //卸载场景
//             yield return TrainingReturnUnloadSceneCoroutine(modifyData);
//
//             //等待一帧
//             yield return CoroutineTool.WaitForFrames();
//
//             //加载新的场景
//             yield return TrainingLoadSceneCoroutine(modifyData);
//
//             swithingScene = false;
//         }
//
//         /// <summary>
//         /// 从训练场回来的协程
//         /// </summary>
//         /// <returns></returns>
//         private static IEnumerator ReturnFromTrainingSceneCoroutine(SwitchLevelModifyData modifyData)
//         {
//             swithingScene = true;
//
//             //卸载训练场场景
//             yield return TrainingReturnUnloadSceneCoroutine(modifyData);
//
//             //等待一帧
//             yield return CoroutineTool.WaitForFrames();
//
//             //加载原来的场景回归先前场景
//             yield return TrainingReturnLoadSceneCoroutine(modifyData);
//
//             swithingScene = false;
//         }
//
//         #endregion
//
//         #region 开始游戏相关
//
//         /// <summary>
//         /// 开始游戏卸载场景的协程
//         /// </summary>
//         /// <returns></returns>
//         private static IEnumerator StartGameUnloadSceneCoroutine()
//         {
//             //切换场景中设为true
//             swithingScene = true;
//
//             //等待一帧
//             yield return CoroutineTool.WaitForEndOfFrame();
//
//             //保存游戏数据
//             yield return GameSaveSystem.SaveGameDataCoroutine(true);
//
//             yield return CoroutineTool.WaitForEndOfFrame();
//
//             //黑屏渐入
//             yield return LoadGamePanel.FadeIn(true, true);
//
//             yield return CoroutineTool.WaitForEndOfFrame();
//
//             swithingScene = false;
//         }
//
//         /// <summary>
//         /// 开始游戏切换场景的协程
//         /// </summary>
//         /// <param name="modifyData">加载过程中的处理信息</param>
//         /// <returns></returns>
//         private static IEnumerator StarGameSwitchSceneCoroutine(SwitchLevelModifyData modifyData)
//         {
//             swithingScene = true;
//
//             //卸载场景
//             yield return StartGameUnloadSceneCoroutine();
//
//             //等待一帧
//             yield return CoroutineTool.WaitForFrames();
//
//             //如果没有设置玩家名称
//             if (string.IsNullOrEmpty(GameSaveSystem.CurSavePlayData.PlayerName))
//             {
//                 //关闭提示UI
//                 UISystem.Close<SaveGameHintUI>();
//
//                 //打开设置名字面板
//                 SetNamePanel panel = UISystem.Show<SetNamePanel>();
//                 //然后添加关闭后执行的事件
//                 panel.OnCloseAction += () =>
//                 {
//                     //保存游戏
//                     GameSaveSystem.StartGameSave(true);
//                     //加载游戏场景
//                     MonoSystem.Start_Coroutine(LoadSceneCoroutine(modifyData));
//                 };
//             }
//             else
//             {
//                 //加载新的场景
//                 yield return LoadSceneCoroutine(modifyData);
//             }
//
//             swithingScene = false;
//         }
//
//         /// <summary>
//         /// 返回主菜单的协程
//         /// </summary>
//         /// <returns></returns>
//         private static IEnumerator ReturnMainMenuCoroutine()
//         {
//             swithingScene = true;
//
//             //黑屏渐入
//             yield return LoadGamePanel.FadeIn(true, true);
//
//             //等待一帧
//             yield return CoroutineTool.WaitForEndOfFrame();
//
//             //移除所有玩家
//             PlayerCenter.RemoveAllPlayers();
//
//             //卸载当前存档
//             GameSaveSystem.UnloadGameSaveData();
//
//             //等待一帧
//             yield return CoroutineTool.WaitForEndOfFrame();
//
//             //加载主菜单
//             AsyncOperation load = SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
//
//             //先禁止关卡自动替换
//             load.allowSceneActivation = false;
//
//             //等待关卡加载完毕
//             while (load.progress < 0.9f)
//             {
//                 yield return CoroutineTool.WaitForEndOfFrame();
//             }
//
//             yield return CoroutineTool.WaitForEndOfFrame();
//
//             //允许场景加载
//             load.allowSceneActivation = true;
//
//             //等待场景加载完毕
//             while (load.isDone == false)
//             {
//                 yield return CoroutineTool.WaitForEndOfFrame();
//             }
//
//             //黑幕渐出
//             yield return LoadGamePanel.FadeOut();
//
//             swithingScene = false;
//         }
//
//         #endregion
//
//         #region 普通
//
//         /// <summary>
//         /// 卸载场景的协程
//         /// </summary>
//         /// <returns></returns>
//         private static IEnumerator UnloadSceneCoroutine(SwitchLevelModifyData modifyData)
//         {
//             //切换场景中设为true
//             swithingScene = true;
//
//             //暂停时间流逝
//             GameTime.TimeScale = 0f;
//
//             //发出卸载场景事件信号
//             EventBroadCastSystem.EventTrigger(EventTags.UnloadSceneEvent);
//
//             //卸载地图
//             yield return MapManager.UnloadMap();
//
//             //等待一帧
//             yield return CoroutineTool.WaitForEndOfFrame();
//
//             //保存游戏数据
//             yield return GameSaveSystem.SaveGameDataCoroutine(true);
//
//             yield return CoroutineTool.WaitForEndOfFrame();
//
//             //黑屏渐入
//             if (modifyData.FadeIn)
//             {
//                 yield return LoadGamePanel.FadeIn(true, modifyData.FadeIn);
//             }
//
//             yield return CoroutineTool.WaitForEndOfFrame();
//
//             //把玩家收起来
//             PlayerCenter.PushAllPlayerToPool();
//
//             swithingScene = false;
//         }
//
//         /// <summary>
//         /// 加载场景的协程
//         /// </summary>
//         /// <param name="modifyData">加载过程中的处理信息</param>
//         /// <returns></returns>
//         private static IEnumerator LoadSceneCoroutine(SwitchLevelModifyData modifyData)
//         {
//             swithingScene = true;
//
//             yield return CoroutineTool.WaitForEndOfFrame();
//
//             //如果启用加载动画
//             if (modifyData.EnableLoading)
//             {
//                 if (LoadGamePanel.Instance != null)
//                 {
//                     LoadGamePanel.Instance.LoadingAnim.SetActive(true);
//                 }
//             }
//
//             yield return CoroutineTool.WaitForFrames();
//
//             //设置当前场景名称为新的场景名称
//             CurLevelID = modifyData.SwitchLevelID;
//             LevelData data = m_LevelDataDic[CurLevelID];
//
//             //如果无法找到对应关卡数据
//             if (data == null)
//             {
//                 Debug.Log($"错误:{modifyData.SwitchLevelID},无法获取对应关卡");
//                 yield break;
//             }
//
//             //获取场景路径
//             string scenePath = data.sceneDatas[0].SceneName;
//             //获取场景名称
//             _curSceneName = data.sceneDatas[0].SceneName;
//
//             //加载关卡障碍信息
//             AStarAlgorithm.LoadSceneObstacles(modifyData.SwitchLevelID);
//
//             //加载场景(以场景独占模式)
//             AsyncOperationHandle<SceneInstance> sceneAsync =
//                 Addressables.LoadSceneAsync(_curSceneName, LoadSceneMode.Single);
//
//             //等待加载
//             yield return sceneAsync;
//
//             AsyncOperation load = sceneAsync.Result.ActivateAsync();
//
//             //先禁止关卡自动替换
//             load.allowSceneActivation = false;
//
//             //等待关卡加载完毕
//             while (load.progress < 0.9f)
//             {
//                 yield return CoroutineTool.WaitForFrames();
//             }
//
//             yield return CoroutineTool.WaitForFrames();
//
//             //允许场景加载
//             load.allowSceneActivation = true;
//
//             //等待场景加载完毕
//             while (load.isDone == false)
//             {
//                 yield return CoroutineTool.WaitForFrames();
//             }
//
//             yield return CoroutineTool.WaitForFrames();
//
//             //关卡初始化
//             AreasManager.Instance.LevelInit();
//
//             yield return CoroutineTool.WaitForFrames();
//             //设置重生点
//             SpawnPos spawnPos = null;
//
//             //如果重生点名字不为空
//             if (modifyData.SpawnPosName != "")
//             {
//                 //那就从该地区寻找对应重生点
//                 //查找重生点
//                 SpawnPos[] posList = GameObject.FindObjectsOfType<SpawnPos>();
//
//                 foreach (var pos in posList)
//                 {
//                     //比对名字
//                     if (pos != null && pos.PosName == modifyData.SpawnPosName)
//                     {
//                         //如果找到了那就赋值
//                         spawnPos = pos;
//                         break;
//                     }
//                 }
//             }
//
//             //重生点为空
//             if (spawnPos == null)
//             {
//                 //那就取默认重生点
//                 spawnPos = AreasManager.Instance.DefaultSpawnPos;
//
//                 //更新出生点名称
//                 if (GameSaveSystem.CurSavePlayData != null)
//                 {
//                     //保存一下出生点名称
//                     GameSaveSystem.CurSavePlayData.SpawnPosName = spawnPos.PosName;
//                     GameSaveSystem.SaveGamePlayData();
//                 }
//             }
//
//             //放出玩家
//             //如果没有玩家
//             if (PlayerCenter.PopAllPlayerFromPool(spawnPos) == 0)
//             {
//                 //那就生成玩家
//                 PlayerCenter.SpawnPlayer(spawnPos, true);
//                 //显示UI
//                 PlayerCenter.ShowPlayerUI();
//             }
//
//             //玩家生成后的回调
//             foreach (var player in PlayerCenter.Players)
//             {
//                 modifyData.callbackAfterPlayerSpawn?.Invoke(player);
//             }
//
//
//             //设置相机位置
//             CameraManager.Instance.TeleportToPos(spawnPos.transform.position);
//
//             yield return CoroutineTool.WaitForFrames();
//
//             //重新激活附近房间
//             CameraManager.Instance.ReloadAroundRooms();
//
//             yield return CoroutineTool.WaitForFrames();
//
//             UISystem.GetWindow<GameTotalInfoPanel>().PanelReset();
//
//             //回调
//             modifyData.callbackAfterSwitch?.Invoke();
//
//             //恢复时间流逝
//             GameTime.TimeScale = 1f;
//
//
//             //黑屏渐出
//             yield return LoadGamePanel.FadeOut();
//
//             yield return CoroutineTool.WaitForFrames();
//
//             swithingScene = false;
//         }
//
//         /// <summary>
//         /// 切换场景的协程
//         /// </summary>
//         /// <param name="modifyData">加载过程中的处理信息</param>
//         /// <returns></returns>
//         private static IEnumerator SwitchSceneCoroutine(SwitchLevelModifyData modifyData)
//         {
//             swithingScene = true;
//
//             //卸载场景
//             yield return UnloadSceneCoroutine(modifyData);
//
//             //等待一帧
//             yield return CoroutineTool.WaitForFrames();
//
//             //加载新的场景
//             yield return LoadSceneCoroutine(modifyData);
//
//             swithingScene = false;
//         }
//
//         #endregion
//
//         #endregion
//
//         #endregion
//
//         #region Public方法
//
//         /// <summary>
//         /// 切换场景
//         /// </summary>
// #if UNITY_EDITOR || RL_DEBUG
//         [ConsoleMethod("SwitchLevel", "切换关卡", "关卡ID", "指定重生点")]
// #endif
//         public static void SwitchScene(string levelID, string spawnPos)
//         {
//             ///防空
//             if (string.IsNullOrEmpty(levelID))
//             {
//                 Debug.LogWarning("LevelId" + "为空");
//                 return;
//             }
//
//             //如果没有在切换场景那就开始
//             if (!swithingScene)
//             {
//                 MonoSystem.Start_Coroutine(SwitchSceneCoroutine(new SwitchLevelModifyData(levelID, spawnPos)));
//             }
//         }
//         [Button("切换场景")]
//         /// <summary>
//         /// 切换场景
//         /// </summary>
//         /// <param name="modifyData">加载过程中的处理信息</param>
//         public static void SwitchScene(SwitchLevelModifyData modifyData)
//         {
//             ///防空
//             if (modifyData == null || string.IsNullOrEmpty(modifyData.SwitchLevelID))
//             {
//                 Debug.LogWarning("LevelId" + "为空");
//                 return;
//             }
//
//             //如果没有在切换场景那就开始
//             if (!swithingScene)
//             {
//                 MonoSystem.Start_Coroutine(SwitchSceneCoroutine(modifyData));
//             }
//         }
//
//         [Button("切换训练场景")]
//         /// <summary>
//         /// 切换训练场景
//         /// </summary>
//         public static void SwitchTrainingScene(SwitchLevelModifyData modifyData)
//         {
//             ///防空
//             if (string.IsNullOrEmpty(modifyData.SwitchLevelID))
//             {
//                 Debug.Log(modifyData.SwitchLevelID + "为空");
//                 return;
//             }
//
//             //如果没有在切换场景那就开始
//             if (!swithingScene)
//             {
//                 MonoSystem.Start_Coroutine(SwitchTrainingSceneCoroutine(modifyData));
//             }
//         }
//
//         /// <summary>
//         /// 重新加载训练场景
//         /// </summary>
//         public static void ReloadTraningScene(SwitchLevelModifyData modifyData)
//         {
//             //如果没有在切换场景那就开始
//             if (!swithingScene)
//             {
//                 MonoSystem.Start_Coroutine(ReloadTrainingSceneCoroutine(modifyData));
//             }
//         }
//
//         [Button("从训练场景回到上个场景")]
//         /// <summary>
//         /// 从训练场景回到上个场景
//         /// </summary>
//         public static void ReturnFromTrainingScene(SwitchLevelModifyData modifyData)
//         {
//             //如果没有在切换场景那就开始
//             if (!swithingScene)
//             {
//                 MonoSystem.Start_Coroutine(ReturnFromTrainingSceneCoroutine(modifyData));
//             }
//         }
//
//         /// <summary>
//         /// 开始游戏切换场景
//         /// </summary>
//         /// <param name="modifyData">加载过程中的处理信息</param>
//         public static void StartGameSwitchScene(SwitchLevelModifyData modifyData)
//         {
//             ///防空
//             if (modifyData == null || string.IsNullOrEmpty(modifyData.SwitchLevelID))
//             {
//                 Debug.Log("LevelId" + "为空");
//                 return;
//             }
//
//             //如果没有在切换场景那就开始
//             if (!swithingScene)
//             {
//                 MonoSystem.Start_Coroutine(StarGameSwitchSceneCoroutine(modifyData));
//             }
//         }
//
//         /// <summary>
//         /// 开始游戏加载存档
//         /// </summary>
//         /// <param name="save">要加载的游戏存档</param>
//         public static void StartGame(SaveItem save)
//         {
//             //如果存档为空那就返回
//             if (save == null) return;
//
//             //如果当前存档不为空，加载的是新存档，那就清空之前的LevelData
//             if (CurSave != null && CurSave.saveID != save.saveID)
//             {
//                 m_LevelDataDic.Clear();
//             }
//
//             //清空当前所选面板
//             UISelectManager.CurUIPanel = null;
//
//             //更新当前存档
//             CurSave = save;
//
//             //加载游戏存档
//             GameSaveSystem.LoadGameData();
//
//             //更新游玩数据
//             GamePlayDataSystem.Instance.PlayTime = 0;
//             GamePlayDataSystem.PlayData.LastSaveIndex = CurSave.saveID;
//             GamePlayDataSystem.Instance.SavePlayData();
//
//             //加载场景
//             StartGameSwitchScene(new SwitchLevelModifyData(CurSavePlayData.LevelID, CurSavePlayData.SpawnPosName));
//         }
//
//         /// <summary>
//         /// 返回主菜单
//         /// </summary>
//         public static void ReturnMainMenu()
//         {
//             //如果没有在切换场景那就开始
//             if (!swithingScene)
//             {
//                 MonoSystem.Start_Coroutine(ReturnMainMenuCoroutine());
//             }
//         }
//
//         #endregion
//
//         #region Static工具方法
//
//         /// <summary>
//         /// 通过LevelID返回关卡中包含的场景名数组
//         /// 如果LevelID不存在 返回null
//         /// </summary>
//         /// <param name="levelID"></param>
//         /// <returns></returns>
//         public static string[] GetLevelSceneNamesByLevelID(string levelID)
//         {
//             if (m_LevelDataDic.TryGetValue(levelID, out var levelData))
//             {
//                 string[] sceneNames = new string[levelData.sceneDatas.Count];
//
//                 for (var index = 0; index < levelData.sceneDatas.Count; index++)
//                 {
//                     sceneNames[index] = levelData.sceneDatas[index].SceneName;
//                 }
//
//                 return sceneNames;
//             }
//
//             return null;
//         }
//
//         // //通过场景名称得到LevelID
//         //
//         // public static string GetLevelIDBySceneName(string sceneName)
//         // {
//         //     var datas = m_LevelDataDic.Values.ToList();
//         //     
//         //     
//         //     foreach (var levelData in m_LevelDataDic.Values)
//         //     {
//         //         foreach (var levelDataSceneData in levelData.sceneDatas)
//         //         {
//         //             if (levelDataSceneData.SceneName == sceneName)
//         //             {
//         //                 
//         //             }
//         //         }
//         //     }
//         // }
//
//         #endregion
//
//
//
//         #endregion
//
// #if UNITY_EDITOR
//
//         #region 测试
//
//         [Button("缓存动画",30)]
//         private static void EditorSaveAnim()
//         {
//             //找到所有的动画机
//             Animator[] anims = GameObject.FindObjectsOfType<Animator>(false);
//             cacheAnimInfo = new Dictionary<Animator, AnimatorStateInfo>();
//
//             //遍历所有动画机，获取当前状态
//             foreach (var anim in anims)
//             {
//                 if(anim != null)
//                 {
//                     AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
//                     cacheAnimInfo[anim] = stateInfo;
//                 }
//             }
//         }
//         [Button("加载缓存动画",30)]
//         private static void EditorLoadAnim()
//         {
//             //如果没有缓存信息就返回
//             if (cacheAnimInfo == null) return;
//
//             //遍历设置
//             foreach (var data in cacheAnimInfo)
//             {
//                 if(data.Key != null)
//                 {
//                     data.Key.Play(data.Value.shortNameHash, 0);
//                 }
//             }
//         }
//
//
//         #endregion
//
// #endif
//
//     }
// }