//****************** 代码文件申明 ************************
//* 文件：DirectoryTipInspector                                       
//* 作者：Koo
//* 创建时间：2024/05/15 15:54:45 星期三
//* 描述：目录便签监视器
//*****************************************************

using System.Collections.Generic;
using System.IO;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEditor;

namespace KooFrame
{
    public class DirectoryTipInspector : CustomVisualElement
    {
        private bool isHasDirectoryData;

        private string curDirectoryPath;

        private string curDirectoryAssetPath;

        private DirectoryData directoryData;

        #region 元素

        /// <summary>
        /// 文件夹名称
        /// </summary>
        private TextField titleField;

        /// <summary>
        /// 文件夹描述
        /// </summary>
        private TextField descriptionField;

        /// <summary>
        /// 所有的提示内容
        /// </summary>
        private ScrollView allTipsScroll;

        private Button addTipBtn;

        private VisualElement hasDataDiv;

        private VisualElement noDataDiv;

        private Button createDirectoryDataBtn;

        #endregion

        public new class UxmlFactory : UxmlFactory<DirectoryTipInspector, VisualElement.UxmlTraits> { }

        /// <summary>
        /// 先绑定目录的路径
        /// </summary>
        /// <param name="directoryPath"></param>
        public void BindDirectoryPath(string directoryPath)
        {
            curDirectoryPath = directoryPath;
            //当前目录下是否有目录SO数据
            curDirectoryAssetPath = curDirectoryPath + "/" + Path.GetFileName(curDirectoryPath) + ".asset";
            isHasDirectoryData = File.Exists(curDirectoryAssetPath);
            //KooCode.Datas.DirectoryDataDic.Dic.ContainsKey(curDirectoryPath);

            if (isHasDirectoryData)
            {
                directoryData = AssetDatabase.LoadAssetAtPath<DirectoryData>(curDirectoryAssetPath);
            }
            else
            {
                isHasDirectoryData = false;
            }
        }

        public override void Init()
        {
            KooCode.AssetsData.DirectoryTipInspectorElement.CloneTree(this);

            #region 绑定所有元素

            titleField = this.Q<TextField>("TitleField");
            descriptionField = this.Q<TextField>("ContentField");
            allTipsScroll = this.Q<ScrollView>("AllTipsScroll");
            addTipBtn = this.Q<Button>("AddTipBtn");

            hasDataDiv = this.Q<VisualElement>("HasDataDiv");
            noDataDiv = this.Q<VisualElement>("NoDataDiv");
            createDirectoryDataBtn = this.Q<Button>("CreateDirectoryDataBtn");

            #endregion

            //当有数据的时候 如何处理元素
            if (isHasDirectoryData)
            {
                ShowDataDiv();
            }
            else //无数据的时候
            {
                hasDataDiv.style.display = DisplayStyle.None;
                noDataDiv.style.display = DisplayStyle.Flex;
                BindTitleAndDescriptionWhenNoData();
                createDirectoryDataBtn.clicked += CreateDirectoryData;
            }
        }

        private void CreateDirectoryData()
        {
            //创建对应数据到字典中
            //DirectoryData newData = new DirectoryData();
            ScriptableObject so = ScriptableObject.CreateInstance(typeof(DirectoryData));
            AssetDatabase.CreateAsset(so, curDirectoryAssetPath);
            directoryData = so as DirectoryData;
            directoryData.Title = titleField.text;
            directoryData.DirectoryPath = curDirectoryPath;
            if (descriptionField.text != "请创建目录便签数据来保存此描述")
            {
                directoryData.Description = descriptionField.text;
            }
            else
            {
                directoryData.Description = "还没什么描述呢~";
            }

            //在当前目录下创建对应的SO文件


            isHasDirectoryData = true;
            ShowDataDiv();
        }


        private void ShowDataDiv()
        {
            hasDataDiv.style.display = DisplayStyle.Flex;
            noDataDiv.style.display = DisplayStyle.None;
            BindTitleAndDescriptionWhenHasData();
            BindButton();
            UpdateAllTipsElement();
        }


        private void BindTitleAndDescriptionWhenNoData()
        {
            //当没数据的时候
            titleField.SetValueWithoutNotify(Path.GetFileName(curDirectoryPath));

            descriptionField.SetValueWithoutNotify("请创建目录便签数据来保存此描述");
        }

        private void BindTitleAndDescriptionWhenHasData()
        {
            //读取对应目录的数据
            titleField.SetValueWithoutNotify(directoryData.Title.IsNullOrEmpty()
                ? Path.GetFileNameWithoutExtension(curDirectoryPath)
                : directoryData.Title);
            //注册赋值监听
            titleField.RegisterValueChangedCallback((value) => { directoryData.Title = value.newValue; });


            descriptionField.SetValueWithoutNotify(directoryData.Description.IsNullOrEmpty()
                ? "还什么描述都没有呢"
                : directoryData.Description);
            //注册赋值监听
            descriptionField.RegisterValueChangedCallback((value) => { directoryData.Description = value.newValue; });
        }


        private void UpdateAllTipsElement()
        {
            //初始化提示列表信息
            List<DirectoryTipData> allDatas = directoryData.TipsDatas;
            allTipsScroll.Clear();
            foreach (DirectoryTipData item in allDatas)
            {
                AddTipContent(item);
            }
        }

        private void BindButton()
        {
            addTipBtn.clicked += AddTips;
        }

        private void AddTips()
        {
            //添加提示元素到
            DirectoryTipData newitem = new("默认提示", "这里还什么内容都没有呢");
            newitem.DirectoryData = AssetDatabase.LoadAssetAtPath<DirectoryData>(curDirectoryAssetPath);
            directoryData.TipsDatas.Add(newitem);
            AddTipContent(newitem);
        }

        private void AddTipContent(DirectoryTipData item)
        {
            TipContentElement tipContentElement = new TipContentElement();
            tipContentElement.BindDirectoryPath(curDirectoryPath);
            tipContentElement.UpdateTitleAndContent(item);
            tipContentElement.Init();
            tipContentElement.OnDeleteSelf += UpdateAllTipsElement;
            allTipsScroll.Add(tipContentElement);
        }
    }
}