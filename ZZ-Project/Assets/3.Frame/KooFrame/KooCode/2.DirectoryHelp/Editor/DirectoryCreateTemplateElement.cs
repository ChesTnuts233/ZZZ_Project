//****************** 代码文件申明 ************************
//* 文件：DirectoryInspector                                       
//* 作者：Koo
//* 创建时间：2024/05/14 15:38:00 星期二
//* 描述：默认文件的检视器
//*****************************************************

using System.Collections.Generic;
using UnityEngine.UIElements;

namespace KooFrame
{
	public class DirectoryCreateTemplateElement : CustomVisualElement
	{

		private List<CodeTemplateData> needToCreateList = new();

		private Dictionary<CodeTemplateData, string> needToCreateFileNameDic = new();
		private KooCodeDatas datas => KooCode.Datas;

		private CodeAssetsData settingsData => KooCode.AssetsData;

		private string curInspectorPath;


		#region 元素

		private Label curSelectPath;
		private ListView needCreatedsListView;
		private ScrollView chooesScrollView;
		private Button createFileBtn;

		#endregion



		public new class UxmlFactory : UxmlFactory<DirectoryCreateTemplateElement, VisualElement.UxmlTraits>
		{

		}


		/// <summary>
		/// 必须先Init
		/// </summary>
		public override void Init()
		{
			settingsData.DefaultFoldTipVistalTreeAsset.CloneTree(this);
			BindAllElement();
		}

		/// <summary>
		/// 先绑定目录路径
		/// </summary>
		/// <param name="curInspectorPath"></param>
		public void BindDirectoryPath(string curInspectorPath)
		{
			this.curInspectorPath = curInspectorPath;
		}


		public void BindAllElement()
		{
			BindPathLabel();
			BindScrollView();
			CreateChooses();
			BindAndCreateListView();
			BindCreateFileBtn();  //绑定创建文件的按钮
		}


		private void BindPathLabel()
		{
			curSelectPath = this.Q<Label>("CurSelectPath");

			curSelectPath.text = curInspectorPath;
		}

		private void BindScrollView()
		{
			chooesScrollView = this.Q<ScrollView>("ChooesScrollView");
		}


		private void BindAndCreateListView()
		{
			needCreatedsListView = this.Q<ListView>("NeedCreateds");

			//needCreatedsListView.

			//这里创建并且读取所有的样本模板
			needCreatedsListView.CreateListView(KooCode.AssetsData.DefaultFoldNeedCreatedItemVisualAsset, needToCreateList);

			needCreatedsListView.bindItem += BindTemplateItem;

			void BindTemplateItem(VisualElement element, int index)
			{
				Label nameLabel = element.Q<Label>("TemplateName");
				TextField createdName = element.Q<TextField>("CreatedName");
				//绑定列表名称
				if (nameLabel != null)
				{
					nameLabel.text = needToCreateList[index].Name.Value;
				}
				//绑定对应的创建名称
				if (createdName != null)
				{
					if (needToCreateFileNameDic.ContainsKey(needToCreateList[index])) //如果字典中有这个模版
					{
						createdName.SetValueWithoutNotify(needToCreateFileNameDic[needToCreateList[index]]);
					}
					else
					{
						createdName.SetPlaceholderText(needToCreateList[index].Name.Value + "(待修改)");
					}

					createdName.RegisterValueChangedCallback((value) =>
					{
						needToCreateFileNameDic[needToCreateList[index]] = value.newValue;
					});
				}

			}
		}



		private void CreateChooses()
		{
			//根据Datas中所有的模版创建需要的元素
			foreach (CodeTemplateData item in datas.CodeTemplates)
			{
				ChooseItem chooseElement = new ChooseItem();
				chooseElement.Init(settingsData.DefaultFoldChooseItemVisualAsset, item);
				chooesScrollView.Add(chooseElement);
				chooseElement.Toggle.RegisterValueChangedCallback((value) =>
				{
					if (value.newValue == true)
					{
						//添加到待被创建的列表
						needToCreateList.Add(item);
						needToCreateFileNameDic.Add(item, item.Name.Value);
						needCreatedsListView.Rebuild();
					}
					else
					{
						needToCreateList.Remove(item);
						needToCreateFileNameDic.Remove(item);
						needCreatedsListView.Rebuild();
					}
				});
			}
		}

		private void BindCreateFileBtn()
		{
			createFileBtn = this.Q<Button>("CreateFileBtn");

			createFileBtn.clicked += CreateFileToLocalFile;
		}

		private void CreateFileToLocalFile()
		{
			//创建文件到对应目录
			foreach (CodeTemplateData item in needToCreateList)
			{
				item.CreateFileToPath(needToCreateFileNameDic[item], curSelectPath.text);
			}
		}



	}
}
