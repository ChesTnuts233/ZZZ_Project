using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace KooFrame
{
	public class CodeDataInspector : CodeInspector
	{
		public new class UxmlFactory : UxmlFactory<CodeDataInspector, VisualElement.UxmlTraits>
		{

		}

		public CodeData CurCheckData;

		#region 元素

		private VisualTreeAsset container_assets;

		private TextField codeContent;

		private TextField nameInputField;

		private Label codeView;

		private ObjectField textAssetField;

		private Button editorBtn;

		private Button viewBtn;

		private Button createTemplateBtn;

		private Button updateContentInFileBtn;

		private ScrollView codeViewScroll;

		private ScrollView codeEditorScroll;

		#endregion

		public override void BindToManagerWindow(KooCodeWindow managerWindow)
		{
			base.BindToManagerWindow(managerWindow);
			container_assets = KooCode.AssetsData.DataInspectorVisualTreeAsset;
			container_assets.CloneTree(this);
			Datas = KooCode.Datas;

			BindAboutViewOrEditorChange(); //绑定代码检视器 1
			BindTemplateName(); //绑定名称相关 2
			BindTextAssetField(); //绑定TextAssetFiled 3
			BindCodeContent(); //绑定CodeContent 4
			BindCreateTemplateBtn(); //绑定创建模板的按钮 5
			BindUpdateContentBtn();
		}

		public override void Show()
		{
			this.style.display = DisplayStyle.Flex;
		}


		public override void Close()
		{
			base.Close();
			this.style.display = DisplayStyle.None;
		}


		private void BindAboutViewOrEditorChange()
		{
			codeView = this.Q<Label>("CodeView");
			codeViewScroll = this.Q<ScrollView>("CodeView");
			codeEditorScroll = this.Q<ScrollView>("CodeEditor");
			codeEditorScroll.style.display = DisplayStyle.None;
			codeViewScroll.style.display = DisplayStyle.Flex;

			BindEditorBtn(); //绑定编辑显示按钮
			BindViewBtn();  //绑定预览显示按钮
		}

		private void BindTemplateName()
		{
			nameInputField = this.Q<TextField>("TemplateName");

			nameInputField.RegisterValueChangedCallback((value) =>
			{
				CurCheckData.Name.Value = value.newValue;
			});
		}


		private void BindEditorBtn()
		{
			editorBtn = this.Q<Button>("EditorBtn");

			editorBtn.clicked += () =>
			{
				codeViewScroll.style.display = DisplayStyle.None;
				codeViewScroll.RemoveFromClassList("show-ani");
				codeEditorScroll.style.display = DisplayStyle.Flex;
				codeEditorScroll.AddToClassList("show-ani");
			};
		}

		private void BindViewBtn()
		{
			viewBtn = this.Q<Button>("ViewBtn");

			viewBtn.clicked += () =>
			{
				codeEditorScroll.style.display = DisplayStyle.None;
				codeEditorScroll.RemoveFromClassList("show-ani");
				codeViewScroll.style.display = DisplayStyle.Flex;
				codeViewScroll.AddToClassList("show-ani");
			};
		}

		private void BindCreateTemplateBtn()
		{
			createTemplateBtn = this.Q<Button>("CreateTemplateBtn");

			createTemplateBtn.clicked += CreateTemplate;
		}

		private void CreateTemplate()
		{
			//把CodeData尝试添加到Templates中
			//检查是否有重名的模板
			var sameData = KooCode.Datas.CodeTemplates.Find((data) => (data.Name.Value == CurCheckData.Name.Value));
			if (sameData != null)
			{
				KooCode.Datas.CodeTemplates.Remove(sameData);
			}
			CodeTemplateData newTemplateData = new(CurCheckData.Name.Value, CurCheckData.Content, CurCheckData.CodeFile);

			KooCode.Datas.CodeTemplates.Add(newTemplateData);



			//代码添加后 
			//遍历所有的codeTemplateDatas，根据名称生成对应的MenuItem
			string updateContent = "";
			foreach (CodeTemplateData data in Datas.CodeTemplates)
			{
				updateContent += "\t[MenuItem(\"Assets/KooFrame-脚本/" + data.Name + "\", false, 0)]\r\n\tpublic static void Create" + data.Name + "Scripts()\r\n\t{\r\n\t\tScriptsTemplatesCreater.CreateScriptByContent(\"" + data.Name + "\",\r\n\t\t\tDatas.GetTemplateContentByDataName(\"" + data.Name + "\"));\r\n\t}\n";
			}

			//生成右键菜单选项
			KooTool.CodeGenerator_ByTag(updateContent, KooCode.CodeTemplateMenuItemPath);

			EditorUtility.SetDirty(KooCode.Datas);
			AssetDatabase.SaveAssets();
		}


		private void BindUpdateContentBtn()
		{
			updateContentInFileBtn = this.Q<Button>("UpdateContentInFileBtn");
			updateContentInFileBtn.clicked += UpdateContentByFile;
		}


		private void UpdateContentByFile()
		{
			CurCheckData.UpdateData();
			UpdateInspector(CurCheckData);
			//("更新内容" + CurCheckData.Content).Log();
		}

		/// <summary>
		/// 更新代码检视
		/// </summary>
		/// <param name="codeContent"></param>
		private void UpdateCodeView(string codeContent)
		{

			string coloredCode = codeContent;

			if (coloredCode.IsNullOrEmpty())
			{
				codeView.text = coloredCode;
				return;
			}

			//遍历字典，为每个关键词着色
			foreach (var kvp in keywordColors)
			{
				string keyword = kvp.Key;
				string color = kvp.Value;

				//使用正则表达式替换关键词并添加颜色标识
				coloredCode = Regex.Replace(coloredCode, "(^|\\s)(" + keyword + ")(?=$|\\s)", "$1<color=" + color + ">$2</color>");
			}

			codeView.text = coloredCode;
		}


		private void BindCodeContent()
		{

			codeContent = this.Q<TextField>("CodeContent");

			codeContent.RegisterValueChangedCallback((value) =>
			{
				CurCheckData.Content = value.newValue;
				UpdateCodeView(value.newValue);
			});

		}


		private void BindTextAssetField()
		{
			textAssetField = this.Q<ObjectField>("TextAssetField");
			textAssetField.RegisterValueChangedCallback((value) =>
			{
				CurCheckData.CodeFile = value.newValue as TextAsset;
				CurCheckData.UpdateData();

				UpdateInspector(CurCheckData);
			});
		}

		public override void UpdateInspector()
		{
			UpdateInspector(CurCheckData);
		}

		public void UpdateInspector(CodeData data)
		{
			CurCheckData = data;
			nameInputField.SetValueWithoutNotify(CurCheckData.Name.Value);
			textAssetField.SetValueWithoutNotify(CurCheckData.CodeFile);

			//更新代码修改
			codeContent.SetValueWithoutNotify(CurCheckData.Content);
			//更新代码检视
			UpdateCodeView(CurCheckData.Content);
		}



		Dictionary<string, string> keywordColors = new Dictionary<string, string>()
	{
		{"class", "yellow"}, // yellow
		{"public", "yellow"}, // yellow
		{"private", "yellow"}, // yellow
		{"protected", "yellow"}, // yellow
		{"internal", "yellow"}, // yellow
		{"void", "yellow"}, // yellow
		{"string", "#008cba"}, // light blue
		{"int", "#00a0e8"}, // sky blue
		{"float", "#00a0e8"}, // sky blue
		{"double", "#00a0e8"}, // sky blue
		{"bool", "#00a0e8"}, // sky blue
		{"char", "#00a0e8"}, // sky blue
		{"byte", "#00a0e8"}, // sky blue
		{"short", "#00a0e8"}, // sky blue
		{"long", "#00a0e8"}, // sky blue
		{"decimal", "#00a0e8"}, // sky blue
		{"object", "#00a8a8"}, // teal
		{"dynamic", "#00a8a8"}, // teal
		{"readonly", "#00a8a8"}, // teal
		{"const", "#00a8a8"}, // teal
		{"static", "#00a8a8"}, // teal
		{"if", "#00a0e8"}, // sky blue
		{"else", "#00a0e8"}, // sky blue
		{"else if", "#00a0e8"}, // sky blue
		{"switch", "#00a0e8"}, // sky blue
		{"case", "#00a0e8"}, // sky blue
		{"default", "#00a0e8"}, // sky blue
		{"for", "#00a0e8"}, // sky blue
		{"foreach", "#00a0e8"}, // sky blue
		{"while", "#00a0e8"}, // sky blue
		{"do", "#00a0e8"}, // sky blue
		{"try", "#00a0e8"}, // sky blue
		{"catch", "#00a0e8"}, // sky blue
		{"finally", "#00a0e8"}, // sky blue
		{"throw", "#00a0e8"}, // sky blue
		{"return", "#00a0e8"}, // sky blue
		{"continue", "#00a0e8"}, // sky blue
		{"break", "#00a0e8"}, // sky blue
		{"new", "#00a0e8"}, // sky blue
		{"using", "#00a0e8"}, // sky blue
		{"namespace", "#00a0e8"}, // sky blue
		{"assembly", "#00a0e8"}, // sky blue
		{"params", "#00a0e8"}, // sky blue
		{"var", "#00a0e8"}, // sky blue
		{"true", "#00c3b1"}, // turquoise
		{"false", "#00c3b1"}, // turquoise
		{"null", "#00c3b1"}, // turquoise
		{"this", "#00c3b1"}, // turquoise
		{"base", "#00c3b1"}, // turquoise
		{"get", "#00c3b1"}, // turquoise
		{"set", "#00c3b1"}, // turquoise
		{"value", "#00c3b1"}, // turquoise
		{"delegate", "#00a0e8"}, // sky blue
		{"event", "#00a0e8"}, // sky blue
		{"=>", "#00c3b1"}, // turquoise
		{"List", "#8258FA"}, // medium purple
		{"ArrayList", "#8258FA"}, // medium purple
		{"Dictionary", "#8258FA"}, // medium purple
		{"HashSet", "#8258FA"}, // medium purple
		{"LinkedList", "#8258FA"}, // medium purple
		{"Queue", "#8258FA"}, // medium purple
		{"Stack", "#8258FA"}, // medium purple
		{"IEnumerable", "#8258FA"}, // medium purple
		{"IEnumerator", "#8258FA"}, // medium purple
		{"IEnumerable<T>", "#8258FA"}, // medium purple
		{"IEnumerator<T>", "#8258FA"}, // medium purple
		{"#region", "#00a0e8"}, // sky blue
		{"#endregion", "#00a0e8"}, // sky blue
		{"#if", "#00a0e8"}, // sky blue
		{"#else", "#00a0e8"}, // sky blue
		{"#elif", "#00a0e8"}, // sky blue
		{"#endif", "#00a0e8"}, // sky blue
		{"#define", "#00a0e8"}, // sky blue
		{"#undef", "#00a0e8"}, // sky blue
		{"#warning", "#00a0e8"}, // sky blue
		{"#error", "#00a0e8"}, // sky blue
		{"#line", "#00a0e8"}, // sky blue
		{"#nullable", "#00a0e8"}, // sky blue
		{"#pragma", "#00a0e8"}, // sky blue
		{"#pragma warning", "#00a0e8"}, // sky blue
		{"#pragma checksum", "#00a0e8"}, // sky blue
		{"#pragma warning disable", "#00a0e8"}, // sky blue
		{"#pragma warning restore", "#00a0e8"}, // sky blue
		{"#SCRIPTNAME#", "#8258FA"}, // medium purple
		{"#AUTHORNAME#", "#8258FA"}, // medium purple
		{"#CREATETIME#", "#8258FA"}, // medium purple

	};
	}
}