//****************** 代码文件申明 ************************
//* 文件：LocalizationConfig                      
//* 作者：32867
//* 创建时间：2023年08月22日 星期二 17:07
//* 功能：
//*****************************************************


using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.IO;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace KooFrame
{
    [CreateAssetMenu(menuName = "KooFrame/LocalizationConfig")]
    public class LocalizationOdinConfig : LocalizationOdinConfigBase<LanguageType>
    {

// #if UNITY_EDITOR
//         [FoldoutGroup("配置方法"), SerializeField, LabelText("表格工作区名称")]private string SheetName = "UI文本本地化配置";
//
//
//         [Button("转为Excel的文本",30), FoldoutGroup("配置方法"), PropertySpace(5, 5)]
//         protected void GetExcelTxt()
//         {
//             string txt = "";
//             string tab = ",";
//
//             foreach (var data in config)
//             {
//                 var dic = data.Value;
//                 //只记录文本
//                 if (!(dic[LanguageType.SimplifiedChinese] is LocalizationStringData))
//                 {
//                     continue;
//                 }
//
//                 txt += data.Key;
//                 txt += tab;
//                 if (dic.ContainsKey(LanguageType.SimplifiedChinese))
//                 {
//                     txt += ((LocalizationStringData)dic[LanguageType.SimplifiedChinese]).content;
//                 }
//                 else
//                 {
//                     txt += "/";
//                 }
//                 txt += tab;
//                 if (dic.ContainsKey(LanguageType.TraditionalChinese))
//                 {
//                     txt += ((LocalizationStringData)dic[LanguageType.TraditionalChinese]).content;
//                 }
//                 else
//                 {
//                     txt += "/";
//                 }
//                 txt += tab;
//                 if (dic.ContainsKey(LanguageType.English))
//                 {
//                     txt += ((LocalizationStringData)dic[LanguageType.English]).content;
//                 }
//                 else
//                 {
//                     txt += "/";
//                 }
//                 txt += tab;
//                 if (dic.ContainsKey(LanguageType.Japanese))
//                 {
//                     txt += ((LocalizationStringData)dic[LanguageType.Japanese]).content;
//                 }
//                 else
//                 {
//                     txt += "/";
//                 }
//                 txt += '\n';
//             }
//
//             GUIUtility.systemCopyBuffer = txt;
//         }
//
//         #region Excel读表
//         /// <summary>
//         /// 加载Excel的数据
//         /// </summary>
//         private void LoadExcelDatas(string[,] datas)
//         {
//             //语言的数量
//             int languageCount = datas.GetLength(1) -1;
//             //行的数量
//             int row = datas.GetLength(0);
//             //跳过第一行，第一行为标题
//             for (int i = 1; i < row; i++)
//             {
//                 //首先获取Key
//                 string key = datas[i, 0];
//                 //如果这个key的内容为空那就跳过
//                 if (string.IsNullOrEmpty(key)) continue;
//                 //获取中文内容
//                 string cnString = datas[i, 1];
//                 //如果中文是空的那就跳过
//                 if(string.IsNullOrEmpty(cnString)) continue;
//
//                 //获取字典
//                 Dictionary<LanguageType, LocalizationDataBase> dataDic;
//                 //如果没有那就new一个新的
//                 if(!config.TryGetValue(key, out dataDic))
//                 {
//                     dataDic = new Dictionary<LanguageType, LocalizationDataBase>();
//                     config[key] = dataDic;
//                 }
//                 //如果当前key已经有配置的了，并且不是string类型那就跳过
//                 else if (!(dataDic[LanguageType.SimplifiedChinese] is LocalizationStringData))
//                 {
//                     Debug.Log("Key:" + key + "已经有配置，且不为文本");
//                     continue;
//                 }
//
//                 //设置每个语言的内容
//                 for (int l = 0; l < languageCount; l++)
//                 {
//                     //new一个新的data
//                     LocalizationStringData stringData = new LocalizationStringData();
//                     //如果这个语言没有配置，那就默认选取中文
//                     if (string.IsNullOrEmpty(datas[i, l + 1]) || datas[i, l+1]=="/")
//                     {
//                         stringData.content = ((LocalizationStringData)dataDic[LanguageType.SimplifiedChinese]).content;
//                     }
//                     else
//                     {
//                         stringData.content = datas[i, l+1];
//                     }
//
//                     //写入字典
//                     dataDic[(LanguageType)l] = stringData;
//                 }
//             }
//
//             //保存数据
//             EditorUtility.SetDirty(this);
//
//         }
//         /// <summary>
//         /// 打开Excel加载物品信息
//         /// </summary>
//         [Button("打开Excel加载",30), FoldoutGroup("配置方法"),PropertySpace(5,5)]
//         public void OpenExcelLoad()
//         {
//             bool success = ReadExcelPath(out string newPath);
//
//             if (success)
//             {
//                 AnalyzeData(newPath);
//             }
//         }
//         /// <summary>
//         /// 获取路径
//         /// </summary>
//         /// <param name="newPath"></param>
//         /// <returns></returns>
//         public bool ReadExcelPath(out string newPath)
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
//         public virtual bool AnalyzeData(string path)
//         {
//             string[,] datas;
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
//                 //找到首个不为空的表
//                 foreach (var sheet in excelPackage.Workbook.Worksheets)
//                 {
//                     if (sheet.Dimension != null && sheet.Name == SheetName)
//                     {
//                         excelWorksheet = sheet;
//                     }
//                 }
//
//                 //防止报空
//                 if (excelWorksheet.Dimension == null)
//                 {
//                     Debug.Log("表格为空");
//                     return false;
//                 }
//
//                 //获取行
//                 Row = excelWorksheet.Dimension.End.Row;
//                 //获取列
//                 Column = excelWorksheet.Dimension.End.Column;
//
//                 //防空
//                 while (excelWorksheet.GetValue(1, Column) == null)
//                 {
//                     Column--;
//                 }
//
//                 //生成数据库
//                 datas = new string[Row, Column];
//
//                 for (int i = excelWorksheet.Dimension.Start.Row; i <= Row; i++)
//                 {
//                     //如果首个元素为空就跳过该行
//                     var _tableValue = excelWorksheet.GetValue(i, 1);
//                     if (_tableValue == null)
//                     {
//                         continue;
//                     }
//
//                     //在第一行，第i列拿到需要的字段
//                     //遍历每一行的每一列
//                     for (int j = excelWorksheet.Dimension.Start.Column;
//                          j <= Column;
//                          j++)
//                     {
//                         //每一列的第一行 读取当前位置的文本
//                         var tableValue = excelWorksheet.GetValue(i, j);
//                         //赋值
//                         datas[i - 1,j - 1] = tableValue.ToString();
//                     }
//
//                 }
//
//             }
//
//             //加载数据
//             LoadExcelDatas(datas);
//
//             return true;
//         }
//
//         #endregion
//
// #endif


    }
}