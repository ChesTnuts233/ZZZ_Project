// //****************** 代码文件申明 ************************
// //* 文件：AudioLibrary                                       
// //* 作者：wheat
// //* 创建时间：2023/10/01 10:04:00 星期日
// //* 描述：负责存储和管理AudioStack
// //*****************************************************
//
// using UnityEngine;
// using System;
// using System.Collections.Generic;
// using Sirenix.OdinInspector;
// using UnityEngine.Audio;
//
// #if UNITY_EDITOR
// using System.IO;
// using UnityEditor;
// #endif
//
// namespace KooFrame
// {
//     [CreateAssetMenu(fileName = "AudioLibrary", menuName = "ScriptableObject/Audio/new AudioLibrary")]
//     public class AudioLibrary : ScriptableObject
//     {
// #if ENABLE_ADDRESSABLES
//         [SerializeField, LabelText("SO文件保存路径"), FoldoutGroup("信息预览")] private string savePath;
//         [SerializeField, LabelText("AB包设置"), FoldoutGroup("信息预览")] private AddressableAssetSettings addressableAssetSettings;
//         [SerializeField, LabelText("AB包分组"), FoldoutGroup("信息预览")] private AddressableAssetGroup addressableGroup;
//
// #endif
//         [field: SerializeField, LabelText("音效库"), FoldoutGroup("信息预览")]
//         public List<AudioStack> Audioes { get; private set; }
//
//         [field: SerializeField, LabelText("BGM库"), FoldoutGroup("信息预览")]
//         public List<BGMStack> BGMs { get; private set; }
//
//         [field: SerializeField, LabelText("音效音源库"), FoldoutGroup("参数设置"), ListDrawerSettings(ShowIndexLabels = true)]
//         public List<AudioClip> AudioClips { get; private set; }
//
//         [field: SerializeField, LabelText("BGM音源库"), FoldoutGroup("参数设置"), ListDrawerSettings(ShowIndexLabels = true)]
//         public List<AudioClip> BGMClips { get; private set; }
//
//         [field: SerializeField, LabelText("音轨库"), FoldoutGroup("参数设置")]
//         public List<AudioMixerGroup> AudioGroups { get; private set; }
//
// #if UNITY_EDITOR
//         private enum ExcelLabelAudio
//         {
//             AudioName = 0,
//             AudioIndex = 1,
//             ClipIndex = 2,
//             AudioGroup = 3,
//             AudioType = 4,
//             Volume = 5,
//             Pitch = 6,
//             RandomMinPitch = 7,
//             RandomMaxPitch = 8,
//             MaxPitch = 9,
//             LimitContinuousPlay = 10,
//             Loop = 11,
//             Is3D = 12,
//         }
//
//         private enum ExcelLabelBGM
//         {
//             AudioName = 0,
//             AudioIndex = 1,
//             ClipIndex = 2,
//             Volume = 3,
//             AudioGroup = 4,
//             Loop = 5,
//         }
//
//         #region 基础方法
//
//         #region 音效生成
//
//         /// <summary>
//         /// 读取信息生成音效文件数据
//         /// </summary>
//         /// <param name="datas"></param>
//         public void LoadAudioes(string[,] datas)
//         {
//             //添加已有的
//             Dictionary<int, AudioStack> oldAudioDic = new Dictionary<int, AudioStack>();
//             //清掉为null的
//             for (int i = 0; i < Audioes.Count; i++)
//             {
//                 if (Audioes[i] == null)
//                 {
//                     Audioes.RemoveAt(i);
//                     i--;
//                 }
//             }
//
//             foreach (var audio in Audioes)
//             {
//                 oldAudioDic.Add(audio.AudioIndex, audio);
//             }
//
//             int row = datas.GetLength(0);
//             //依次读取数据
//             //跳过前几行，前几行为无效信息
//             for (int r = 3; r < row; r++)
//             {
//                 //如果为空就跳过，或者一些不是音效的选项就跳过
//                 if (datas[r, 0] == "" || datas[r, 0] == null || datas[r, 0] == "无" || datas[r, 0] == "音效名称" ||
//                     datas[r, 0] == "默认")
//                 {
//                     continue;
//                 }
//
//                 #region 初始化变量
//
//                 int audioIndex;
//                 string audioName;
//                 List<AudioClip> audioClips;
//                 AudioMixerGroup audioGroup;
//                 float volume;
//                 float pitch;
//                 float randomMinPitch;
//                 float randomMaxPitch;
//                 float maxPitch;
//                 bool limitContinuousPlay;
//                 bool loop;
//                 bool is3D;
//                 AudioSFXType audioType;
//
//                 #endregion
//
//                 //音效id
//                 if (!int.TryParse(datas[r, (int)ExcelLabelAudio.AudioIndex], out audioIndex))
//                 {
//                     Debug.Log(datas[r, (int)ExcelLabelAudio.AudioIndex]);
//                     Debug.Log("在读取第" + r + "个音效id时发生错误");
//                     continue;
//                 }
//
//                 //音效名称
//                 audioName = datas[r, (int)ExcelLabelAudio.AudioName];
//
//                 //音效Clip
//                 audioClips = new List<AudioClip>();
//                 //clip信息
//                 //id可能有多个所以用','分开
//                 string clipStrData = datas[r, (int)ExcelLabelAudio.ClipIndex].Replace("，", ",");
//                 string[] clipData = clipStrData.Split(',');
//                 foreach (var strs in clipData)
//                 {
//                     //id1~id2表示，id1~id2的音效id
//                     if (strs.Contains('~'))
//                     {
//                         string[] ids = strs.Split('~');
//                         if (int.TryParse(ids[0], out int _clipIndex) && int.TryParse(ids[1], out int _clipIndex2))
//                         {
//                             if (_clipIndex < 0 || _clipIndex >= AudioClips.Count
//                                                || _clipIndex2 < 0 || _clipIndex2 >= AudioClips.Count)
//                             {
//                                 Debug.Log("在读取第" + r + "个音源id时发生错误");
//                             }
//                             else
//                             {
//                                 for (int i = _clipIndex; i <= _clipIndex2; i++)
//                                 {
//                                     audioClips.Add(AudioClips[i]);
//                                 }
//                             }
//                         }
//                         else
//                         {
//                             Debug.Log("在读取第" + r + "个音源id时发生错误");
//                         }
//                     }
//                     else
//                     {
//                         if (int.TryParse(strs, out int _clipIndex))
//                         {
//                             if (_clipIndex >= AudioClips.Count)
//                             {
//                                 Debug.Log("在读取第" + r + "个音源id时发生错误");
//                             }
//                             else if (_clipIndex >= 0)
//                             {
//                                 audioClips.Add(AudioClips[_clipIndex]);
//                             }
//                         }
//                         else
//                         {
//                             Debug.Log("在读取第" + r + "个音源id时发生错误");
//                         }
//                     }
//                 }
//
//                 //音轨
//                 switch (datas[r, (int)ExcelLabelAudio.AudioGroup])
//                 {
//                     case "音效":
//                         audioGroup = AudioGroups[0];
//                         break;
//                     case "音乐":
//                         audioGroup = AudioGroups[1];
//                         break;
//                     case "环境":
//                         audioGroup = AudioGroups[2];
//                         break;
//                     case "机关":
//                         audioGroup = AudioGroups[3];
//                         break;
//                     case "UI":
//                         audioGroup = AudioGroups[10];
//                         break;
//                     default:
//                         audioGroup = null;
//                         break;
//                 }
//
//                 //音量
//                 if (!float.TryParse(datas[r, (int)ExcelLabelAudio.Volume], out volume))
//                 {
//                     Debug.Log(datas[r, (int)ExcelLabelAudio.Volume]);
//                     Debug.Log("在读取第" + r + "个音量时发生错误");
//                     continue;
//                 }
//
//                 //音高
//                 if (!float.TryParse(datas[r, (int)ExcelLabelAudio.Pitch], out pitch))
//                 {
//                     Debug.Log(datas[r, (int)ExcelLabelAudio.Pitch]);
//                     Debug.Log("在读取第" + r + "个音高时发生错误");
//                     continue;
//                 }
//
//                 //随机最低音高
//                 if (!float.TryParse(datas[r, (int)ExcelLabelAudio.RandomMinPitch], out randomMinPitch))
//                 {
//                     Debug.Log(datas[r, (int)ExcelLabelAudio.RandomMinPitch]);
//                     Debug.Log("在读取第" + r + "个最低音高时发生错误");
//                     continue;
//                 }
//
//                 //随机最高音高
//                 if (!float.TryParse(datas[r, (int)ExcelLabelAudio.RandomMaxPitch], out randomMaxPitch))
//                 {
//                     Debug.Log(datas[r, (int)ExcelLabelAudio.RandomMaxPitch]);
//                     Debug.Log("在读取第" + r + "个最高音高时发生错误");
//                     continue;
//                 }
//
//                 //最高音高
//                 if (!float.TryParse(datas[r, (int)ExcelLabelAudio.RandomMaxPitch], out maxPitch))
//                 {
//                     Debug.Log(datas[r, (int)ExcelLabelAudio.RandomMaxPitch]);
//                     Debug.Log("在读取第" + r + "个最高音高时发生错误");
//                     continue;
//                 }
//
//                 //限制连续播放
//                 limitContinuousPlay = datas[r, (int)ExcelLabelAudio.LimitContinuousPlay] == "是";
//                 //是否循环播放
//                 loop = datas[r, (int)ExcelLabelAudio.Loop] == "是";
//                 //是否为3D音效
//                 is3D = datas[r, (int)ExcelLabelAudio.Is3D] == "是";
//                 //音效类型
//                 switch (datas[r, (int)ExcelLabelAudio.AudioType])
//                 {
//                     case "攻击前摇":
//                         audioType = AudioSFXType.AttackForward;
//                         break;
//                     case "攻击ATKFrame":
//                         audioType = AudioSFXType.AttackATKFrame;
//                         break;
//                     case "攻击后摇":
//                         audioType = AudioSFXType.AttackBack;
//                         break;
//                     case "环境音效":
//                         audioType = AudioSFXType.EnvAudio;
//                         break;
//                     case "投掷":
//                         audioType = AudioSFXType.Throw;
//                         break;
//                     case "摧毁物品":
//                         audioType = AudioSFXType.Break;
//                         break;
//                     default:
//                         audioType = AudioSFXType.None;
//                         break;
//                 }
//
//
//                 //如果已经有audioStack了
//                 if (oldAudioDic.TryGetValue(audioIndex, out AudioStack audioStack))
//                 {
//                     //那就找到然后更新数据
//                     audioStack.UpdateDatas(audioIndex, audioName, audioClips, audioGroup, volume,
//                         pitch, randomMinPitch, randomMaxPitch, maxPitch, limitContinuousPlay, loop, is3D, audioType);
//                     //保存
//                     EditorUtility.SetDirty(audioStack);
//                 }
//                 else
//                 {
//                     //没有就新创一个
//                     audioStack = CreateAudioStack(audioIndex, audioName, audioClips, audioGroup, volume,
//                         pitch, randomMinPitch, randomMaxPitch, maxPitch, limitContinuousPlay, loop, is3D, audioType);
//
//                     //添加入列表
//                     Audioes.Add(audioStack);
//                 }
//             }
//
//
//             //保存SO文件
//             EditorUtility.SetDirty(this);
//         }
//
//         #endregion
//
//         #region BGM生成
//
//         /// <summary>
//         /// 读取信息生成音效文件数据
//         /// </summary>
//         /// <param name="datas"></param>
//         public void LoadBGMs(string[,] datas)
//         {
//             //用来存储当前已有的bgm
//             Dictionary<int, BGMStack> bgmDic = new Dictionary<int, BGMStack>();
//
//             //首先清除list
//             BGMs.Clear();
//
//             //获取行数
//             int row = datas.GetLength(0);
//
//             #region BGMStack包生成
//
//             //开始遍历读取信息优先读取Stack包(跳过一些没有用的信息)
//             for (int r = 2; r < row; r++)
//             {
//                 //如果为空就跳过，或者一些不是音效的选项就跳过
//                 if (string.IsNullOrEmpty(datas[r, 0]) || datas[r, 0] == "无" || datas[r, 0] == "BGM名称")
//                 {
//                     continue;
//                 }
//
//                 //如果不是Stack包，那就跳过
//                 if (datas[r, (int)ExcelLabelBGM.AudioGroup] != "Stack包")
//                 {
//                     continue;
//                 }
//
//                 #region 初始化变量
//
//                 int bgmIndex;
//                 string bgmName;
//                 List<BGMClipStack> clips;
//
//                 #endregion
//
//                 #region 赋值
//
//                 //id
//                 if (!int.TryParse(datas[r, (int)ExcelLabelBGM.AudioIndex], out bgmIndex))
//                 {
//                     Debug.Log(datas[r, (int)ExcelLabelBGM.AudioIndex]);
//                     Debug.Log("在读取第" + r + "个音效id时发生错误");
//                     continue;
//                 }
//
//                 //名称
//                 bgmName = datas[r, (int)ExcelLabelBGM.AudioName];
//
//                 //BGMclip
//                 clips = new List<BGMClipStack>();
//
//                 #endregion
//
//                 //new一个新的BGM
//                 BGMStack stack = new BGMStack(bgmIndex, bgmName, clips);
//
//                 //添加入字典和列表
//                 bgmDic.Add(bgmIndex, stack);
//                 BGMs.Add(stack);
//             }
//
//             #endregion
//
//             #region BGMClip生成
//
//             //开始遍历读取信息读取Stack包剩下的(跳过一些没有用的信息)
//             for (int r = 2; r < row; r++)
//             {
//                 //如果为空就跳过，或者一些不是音效的选项就跳过
//                 if (string.IsNullOrEmpty(datas[r, 0]) || datas[r, 0] == "无" || datas[r, 0] == "BGM名称")
//                 {
//                     continue;
//                 }
//
//                 //如果是Stack包，那就跳过
//                 if (datas[r, (int)ExcelLabelBGM.AudioGroup] == "Stack包")
//                 {
//                     continue;
//                 }
//
//                 #region 初始化变量
//
//                 string bgmName;
//                 int bgmIndex;
//                 List<AudioClip> clips;
//                 int audioTrackIndex;
//                 AudioMixerGroup group;
//                 float volume;
//                 bool loop;
//
//                 #endregion
//
//                 #region 赋值
//
//                 //名称
//                 bgmName = datas[r, (int)ExcelLabelBGM.AudioName];
//
//                 //id
//                 if (!int.TryParse(datas[r, (int)ExcelLabelBGM.AudioIndex], out bgmIndex))
//                 {
//                     Debug.Log(datas[r, (int)ExcelLabelBGM.AudioIndex]);
//                     Debug.Log("在读取第" + r + "个音效id时发生错误");
//                     continue;
//                 }
//
//                 //如果字典中不存在那就跳过
//                 if (bgmDic.ContainsKey(bgmIndex) == false)
//                 {
//                     Debug.Log("字典中不存在对应id的BGM：" + bgmIndex);
//                     continue;
//                 }
//
//                 //音轨
//                 if (!int.TryParse(datas[r, (int)ExcelLabelBGM.AudioGroup], out audioTrackIndex))
//                 {
//                     Debug.Log(datas[r, (int)ExcelLabelBGM.AudioGroup]);
//                     Debug.Log("在读取第" + r + "个音轨id时发生错误");
//                     continue;
//                 }
//
//                 group = AudioGroups[3 + audioTrackIndex];
//
//                 //音量
//                 if (!float.TryParse(datas[r, (int)ExcelLabelBGM.Volume], out volume))
//                 {
//                     Debug.Log(datas[r, (int)ExcelLabelBGM.Volume]);
//                     Debug.Log("在读取第" + r + "个音轨id时发生错误");
//                     continue;
//                 }
//
//                 //循环播放
//                 loop = datas[r, (int)ExcelLabelBGM.Loop] == "是";
//
//                 //音源
//                 clips = new List<AudioClip>();
//                 //先获取文本
//                 string clipData = datas[r, (int)ExcelLabelBGM.ClipIndex];
//                 //以防中文逗号
//                 clipData = clipData.Replace("，", ",");
//
//                 //把","间隔的拆分开
//                 string[] clipDatas = clipData.Split(",");
//
//                 //遍历每个字段
//                 foreach (var str in clipDatas)
//                 {
//                     //如果有x~x
//                     if (str.Contains("~"))
//                     {
//                         string[] p = str.Split("~");
//                         if (!int.TryParse(p[0], out int a))
//                         {
//                             Debug.Log("在读取第" + r + "个音源id时发生错误");
//                             continue;
//                         }
//
//                         if (!int.TryParse(p[1], out int b))
//                         {
//                             Debug.Log("在读取第" + r + "个音源id时发生错误");
//                             continue;
//                         }
//
//                         int min = Math.Min(a, b);
//                         int max = Math.Max(a, b);
//
//                         //把每个Clip添加到列表中
//                         for (int i = min; i <= max; i++)
//                         {
//                             //防止越界
//                             if (i >= BGMClips.Count || i < 0) continue;
//
//                             clips.Add(BGMClips[i]);
//                         }
//                     }
//                     //如果是单个的直接获取
//                     else if (int.TryParse(str, out int x) && x >= 0 && x < BGMClips.Count)
//                     {
//                         clips.Add(BGMClips[x]);
//                     }
//                     else
//                     {
//                         Debug.Log("在读取音源的时候，读取错误，下标越界或错误文本: " + str);
//                     }
//                 }
//
//                 #endregion
//
//                 //初始化BGMClipStack
//                 BGMClipStack bgmClipStack =
//                     new BGMClipStack(bgmIndex, bgmName, audioTrackIndex, clips, group, volume, loop);
//                 //更新BGMStack
//                 BGMStack bgmStack = bgmDic[bgmIndex];
//                 bgmStack.Clips.Add(bgmClipStack);
//             }
//
//             #endregion
//
//             //保存SO文件
//             EditorUtility.SetDirty(this);
//         }
//
//         #endregion
//
//         /// <summary>
//         /// 创建AudioStack的SO文件
//         /// </summary>
//         private AudioStack CreateAudioStack(int audioIndex, string audioName, List<AudioClip> clips,
//             AudioMixerGroup audioGroup, float volume, float pitch,
//             float randomMinPitch, float randomMaxPitch, float maxPitch, bool limitContinuousPlay, bool loop, bool is3D,
//             AudioSFXType audioType)
//         {
//             //初始化
//             AudioStack audioStack = ScriptableObject.CreateInstance<AudioStack>();
//             audioStack.UpdateDatas(audioIndex, audioName, clips, audioGroup, volume,
//                 pitch, randomMinPitch, randomMaxPitch, maxPitch, limitContinuousPlay, loop, is3D, audioType);
//
//             //获取保存路径
//             string targetPath = savePath;
//
//             if (audioGroup != null)
//             {
//                 targetPath += "/" + audioGroup.name;
//             }
//
//             //如果路径不存在就创建文件夹
//             if (!File.Exists(targetPath))
//             {
//                 Directory.CreateDirectory(targetPath);
//             }
//
//             targetPath += "/AS" + audioIndex + audioName;
//
//             //防止路径冲突
//             targetPath = AvoidPathCollision(targetPath, ".asset", 0);
//
//             //保存到目标路径
//             EditorUtility.SetDirty(audioStack);
//
//             AssetDatabase.CreateAsset(audioStack, targetPath);
//             AssetDatabase.SaveAssets();
//
//             AddToAddressable(targetPath, "AS" + audioIndex + audioName);
//
//             return audioStack;
//         }
//
//         /// <summary>
//         /// 将文件添加到Addressable
//         /// </summary>
//         /// <param name="path">文件路径</param>
//         /// <param name="soName">文件名称</param>
//         private void AddToAddressable(string path, string soName)
//         {
//             AddressableAssetEntry entry =
//                 addressableAssetSettings.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID(path), addressableGroup);
//             entry.address = soName;
//
//             addressableAssetSettings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true, true);
//         }
//
//         #endregion
//
//         #region 读表
//
//         /// <summary>
//         /// 打开Excel加载物品信息
//         /// </summary>
//         [Button("打开Excel加载BGM和音效", 30), FoldoutGroup("信息预览"), PropertySpace(5f, 5f)]
//         public void OpenExcelLoad()
//         {
//             bool success = ReadExcelPath(out string newPath);
//
//             if (success)
//             {
//                 AnalyzeData(newPath, true, true);
//             }
//         }
//
//         /// <summary>
//         /// 打开Excel加载物品信息
//         /// </summary>
//         [Button("打开Excel加载BGM", 30), FoldoutGroup("信息预览"), PropertySpace(5f, 5f)]
//         public void OpenExcelLoadBGM()
//         {
//             bool success = ReadExcelPath(out string newPath);
//
//             if (success)
//             {
//                 AnalyzeData(newPath, true, false);
//             }
//         }
//
//         /// <summary>
//         /// 打开Excel加载物品信息
//         /// </summary>
//         [Button("打开Excel加载音效", 30), FoldoutGroup("信息预览"), PropertySpace(5f, 5f)]
//         public void OpenExcelLoadAudio()
//         {
//             bool success = ReadExcelPath(out string newPath);
//
//             if (success)
//             {
//                 AnalyzeData(newPath, false, true);
//             }
//         }
//
//         /// <summary>
//         /// 获取路径
//         /// </summary>
//         /// <param name="newPath"></param>
//         /// <returns></returns>
//         protected bool ReadExcelPath(out string newPath)
//         {
//             newPath = EditorUtility.OpenFilePanel("选择表格", "", "");
//
//             if (newPath == "")
//             {
//                 return false;
//             }
//
//             if (!newPath.Contains("xlsx"))
//             {
//                 EditorUtility.DisplayDialog("表格读取失败", "不是正确的文件格式，请读取xlsx的格式的文件", "确认");
//                 return false;
//             }
//
//             return true;
//         }
//
//         protected virtual bool AnalyzeData(string path, bool bgm, bool audio)
//         {
//             string[,] datasAudioes = null;
//             string[,] datasBGM = null;
//             int Row = 0;
//             int Column = 0;
//             //读取路径
//             FileInfo fileInfo = new FileInfo(path);
//
//             using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
//             {
//                 //先获取工作区
//                 ExcelWorksheet excelWorksheet = null;
//
//                 //如果读取音效
//                 if (audio)
//                 {
//                     List<ExcelWorksheet> audioSheets = new List<ExcelWorksheet>();
//                     //找到音效表
//                     foreach (var sheet in excelPackage.Workbook.Worksheets)
//                     {
//                         if (sheet.Dimension != null && sheet.Name.Contains("音效表"))
//                         {
//                             excelWorksheet = sheet;
//                         }
//                         else
//                         {
//                             continue;
//                         }
//
//                         //防止报空
//                         if (excelWorksheet.Dimension == null)
//                         {
//                             continue;
//                         }
//                         else
//                         {
//                             audioSheets.Add(sheet);
//                         }
//
//                         //统计行数
//                         Row += excelWorksheet.Dimension.End.Row;
//                         //获取列(这边跳过一列:最后一列是备注)
//                         Column = excelWorksheet.Dimension.End.Column;
//
//                         //防空
//                         while (excelWorksheet.GetValue(1, Column) == null)
//                         {
//                             Column--;
//                         }
//
//                         //去掉最后一列，最后一列是备注
//                         Column--;
//                     }
//
//                     //生成数据库
//                     datasAudioes = new string[Row, Column];
//                     //统计当前行数
//                     int curRow = 0;
//
//                     //遍历每一个音效表格工作区
//                     foreach (var sheet in audioSheets)
//                     {
//                         int rowCount = 0;
//                         for (int i = sheet.Dimension.Start.Row; i <= sheet.Dimension.End.Row; i++)
//                         {
//                             //如果首个元素为空就跳过该行
//                             var _tableValue = sheet.GetValue(i, 1);
//                             if (_tableValue == null)
//                             {
//                                 continue;
//                             }
//
//                             //在第一行，第i列拿到需要的字段
//                             //遍历每一行的每一列
//                             for (int j = sheet.Dimension.Start.Column;
//                                  j <= Column;
//                                  j++)
//                             {
//                                 //每一列的第一行 读取当前位置的文本
//                                 var tableValue = sheet.GetValue(i, j);
//                                 //赋值
//                                 try
//                                 {
//                                     datasAudioes[curRow + i - 1, j - 1] = tableValue.ToString();
//                                 }
//                                 catch
//                                 {
//                                     Debug.Log(i + "," + j);
//                                 }
//                             }
//
//                             //统计当前的行数
//                             rowCount++;
//                         }
//
//                         //更新行数
//                         curRow += rowCount;
//                     }
//                 }
//
//                 //如果读取BGM
//                 if (bgm)
//                 {
//                     //找到BGM表
//                     foreach (var sheet in excelPackage.Workbook.Worksheets)
//                     {
//                         if (sheet.Dimension != null && sheet.Name == ("BGM表"))
//                         {
//                             excelWorksheet = sheet;
//                         }
//                         else
//                         {
//                             continue;
//                         }
//
//                         //统计行数
//                         int Row2 = excelWorksheet.Dimension.End.Row;
//
//                         //获取列(这边跳过一列:最后一列是备注)
//                         int Column2 = excelWorksheet.Dimension.End.Column;
//
//                         //防空
//                         while (excelWorksheet.GetValue(1, Column2) == null)
//                         {
//                             Column2--;
//                         }
//
//                         //去掉最后一列，最后一列是备注
//                         Column2--;
//                         datasBGM = new string[Row2, Column2];
//
//                         for (int i = excelWorksheet.Dimension.Start.Row; i <= Row2; i++)
//                         {
//                             //如果首个元素为空就跳过该行
//                             var _tableValue = excelWorksheet.GetValue(i, 1);
//                             if (_tableValue == null)
//                             {
//                                 continue;
//                             }
//
//                             //在第一行，第i列拿到需要的字段
//                             //遍历每一行的每一列
//                             for (int j = excelWorksheet.Dimension.Start.Column;
//                                  j <= Column2;
//                                  j++)
//                             {
//                                 //每一列的第一行 读取当前位置的文本
//                                 var tableValue = excelWorksheet.GetValue(i, j);
//                                 //赋值
//                                 try
//                                 {
//                                     datasBGM[i - 1, j - 1] = tableValue.ToString();
//                                 }
//                                 catch
//                                 {
//                                     Debug.Log(i + "," + j);
//                                 }
//                             }
//                         }
//                     }
//                 }
//             }
//
//             /*测试用的
//             for (int i = 0; i < datas.GetLength(0); i++)
//             {
//                 string d = "";
//                 for (int j = 0; j < datas.GetLength(1); j++)
//                 {
//                     d += datas[i, j];
//                     d += " , ";
//                 }
//
//                 Debug.Log(d);
//             }
//             */
//
//             //生成音效信息表
//             if (datasAudioes != null)
//             {
//                 LoadAudioes(datasAudioes);
//             }
//
//             //生成BGM信息表
//             if (datasBGM != null)
//             {
//                 LoadBGMs(datasBGM);
//             }
//
//             //保存SO文件
//             EditorUtility.SetDirty(this);
//             AssetDatabase.Refresh();
//             AssetDatabase.SaveAssets();
//
//             Debug.Log("音效信息更新完毕");
//
//             return true;
//         }
//
//         /// <summary>
//         /// 防止路径冲突
//         /// </summary>
//         /// <param name="path">路径</param>
//         /// <param name="format">格式</param>
//         /// <param name="id">id</param>
//         private string AvoidPathCollision(string path, string format, int id = 0)
//         {
//             //先获取路径
//             var res = path;
//             //如果id不为0那就加入(id)
//             if (id != 0)
//             {
//                 res += "(" + id + ")";
//             }
//
//             //最后添加后缀
//             res += format;
//
//             //如果文件已存在那就继续更改
//             if (File.Exists(res))
//             {
//                 return AvoidPathCollision(path, format, id + 1);
//             }
//             else
//             {
//                 //不存在就输出
//                 return res;
//             }
//         }
//
//         #endregion
//
// #endif
//
//         #region 一些静态音效id
//
//         /// <summary>
//         /// 击打_钝击充能
//         /// </summary>
//         public const int HitBluntCharge = 300037;
//
//         /// <summary>
//         /// 击打_钝击L
//         /// </summary>
//         public const int HitBluntL = 300038;
//
//         /// <summary>
//         /// 击打_钝击击杀L
//         /// </summary>
//         public const int HitBluntKillL = 300039;
//
//         /// <summary>
//         /// 击打_钝击S
//         /// </summary>
//         public const int HitBluntS = 300040;
//
//         /// <summary>
//         /// 击打_钝击击杀S
//         /// </summary>
//         public const int HitBluntKillS = 300041;
//
//         /// <summary>
//         /// 击打_切割充能
//         /// </summary>
//         public const int HitSlashCharge = 300042;
//
//         /// <summary>
//         /// 击打_切割L
//         /// </summary>
//         public const int HitSlashL = 300043;
//
//         /// <summary>
//         /// 击打_切割击杀L
//         /// </summary>
//         public const int HitSlashKillL = 300044;
//
//         /// <summary>
//         /// 击打_切割S
//         /// </summary>
//         public const int HitSlashS = 300045;
//
//         /// <summary>
//         /// 击打_切割击杀S
//         /// </summary>
//         public const int HitSlashKillS = 300046;
//
//         /// <summary>
//         /// 击打_失败
//         /// </summary>
//         public const int HitFailure = 300047;
//
//         /// <summary>
//         /// 掉落悬崖
//         /// </summary>
//         public const int Fall = 300065;
//
//         /// <summary>
//         /// 翻滚起身
//         /// </summary>
//         public const int RollJump = 300002;
//
//         /// <summary>
//         /// 翻滚落地
//         /// </summary>
//         public const int RollLand = 300003;
//
//         /// <summary>
//         /// 背包切换
//         /// </summary>
//         public const int BackPackSwitchItem = 300010;
//
//         /// <summary>
//         /// 抓取物品出手
//         /// </summary>
//         public const int CaptureOut = 300011;
//
//         /// <summary>
//         /// 获得物品
//         /// </summary>
//         public const int GetItem = 300012;
//
//         /// <summary>
//         /// 地刺刺击受伤
//         /// </summary>
//         public const int ThornHit = 300030;
//
//         /// <summary>
//         /// 玩家手持物品损毁
//         /// </summary>
//         public const int PlayerItemBroken = 300033;
//
//         /// <summary>
//         /// 炸弹点燃
//         /// </summary>
//         public const int BombFuzz = 300013;
//
//         /// <summary>
//         /// 炸弹爆炸
//         /// </summary>
//         public const int BombBoom = 300014;
//
//         /// <summary>
//         /// 破碎地砖
//         /// </summary>
//         public const int BreakTile = 300015;
//
//         /// <summary>
//         /// Impale充能完毕
//         /// </summary>
//         public const int ImpaleDing = 300048;
//
//         /// <summary>
//         /// Impale投掷Forward
//         /// </summary>
//         public const int ImpaleThrowForward = 300049;
//
//         /// <summary>
//         /// Round充能完毕
//         /// </summary>
//         public const int RoundDing = 300050;
//
//         /// <summary>
//         /// Round投掷Forward
//         /// </summary>
//         public const int RoundForward = 300051;
//
//         /// <summary>
//         /// Scatter充能完毕
//         /// </summary>
//         public const int ScatterDing = 300052;
//
//         /// <summary>
//         /// Scatter投掷Forward
//         /// </summary>
//         public const int ScatterForward = 300053;
//
//         /// <summary>
//         /// SpinOut充能完毕
//         /// </summary>
//         public const int SpinOutDing = 300054;
//
//         /// <summary>
//         /// SpinOut投掷Forward
//         /// </summary>
//         public const int SpinOutForward = 300055;
//
//         /// <summary>
//         /// 投掷Shake
//         /// </summary>
//         public const int ThrowShake = 300056;
//
//         /// <summary>
//         /// 玩家受伤轻
//         /// </summary>
//         public const int PlayerInjureSoft = 300057;
//
//         /// <summary>
//         /// 玩家受伤重
//         /// </summary>
//         public const int PlayerInjureWeight = 300058;
//
//         /// <summary>
//         /// 玩家受伤严重
//         /// </summary>
//         public const int PlayerInjureWorset = 300061;
//
//         /// <summary>
//         /// 投掷直射L
//         /// </summary>
//         public const int ThrowImpaleL = 320000;
//
//         /// <summary>
//         /// 投掷直射S
//         /// </summary>
//         public const int ThrowImpaleS = 320001;
//
//         /// <summary>
//         /// 投掷旋转丢出L
//         /// </summary>
//         public const int ThrowSpinOutL = 320002;
//
//         /// <summary>
//         /// 投掷旋转丢出S
//         /// </summary>
//         public const int ThrowSpinOutS = 320003;
//
//         #endregion
//     }
// }