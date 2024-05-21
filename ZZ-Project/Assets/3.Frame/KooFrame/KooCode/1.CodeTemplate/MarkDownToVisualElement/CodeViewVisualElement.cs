//****************** 代码文件申明 ************************
//* 文件：CodeViewVisualElement                                       
//* 作者：Koo
//* 创建时间：2024/05/20 22:34:54 星期一
//* 描述：Nothing
//*****************************************************

using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.UIElements;

namespace KooFrame
{
    public class CodeViewVisualElement : VisualElement
    {
        private Label codeView;

        private string reallyContent;

        private Button generatorCodeDataBtn;

        public new class UxmlFactory : UxmlFactory<CodeViewVisualElement, VisualElement.UxmlTraits> { }


        /// <summary>
        /// 外部调用初始化
        /// </summary>
        /// <param name="codeContent"></param>
        public void Init(string codeContent)
        {
            KooCode.AssetsData.KooCodeViewVisualTreeAsset.CloneTree(this);
            BindAllElement();
            reallyContent = codeContent;
            UpdateCodeView(codeContent);
        }

        private void BindAllElement()
        {
            BindCodeViewLabel();
            BindGeneratorCodeBtn();
        }


        private void BindCodeViewLabel()
        {
            codeView = this.Q<Label>("CodeView");
        }


        private void BindGeneratorCodeBtn()
        {
            generatorCodeDataBtn = this.Q<Button>("GeneratorCodeDataBtn");
            generatorCodeDataBtn.clicked += GeneratorCodeData;
        }

        private void GeneratorCodeData()
        {
            var window = CodeDataCreateWindow.ShowWindow();
            window.SetCreateDataContent(reallyContent);
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
                coloredCode = Regex.Replace(coloredCode, "(^|\\s)(" + keyword + ")(?=$|\\s)",
                    "$1<color=" + color + ">$2</color>");
            }

            codeView.text = coloredCode;
        }


        Dictionary<string, string> keywordColors = new Dictionary<string, string>()
        {
            { "class", "yellow" },                    // yellow
            { "public", "yellow" },                   // yellow
            { "private", "yellow" },                  // yellow
            { "protected", "yellow" },                // yellow
            { "internal", "yellow" },                 // yellow
            { "void", "yellow" },                     // yellow
            { "string", "#008cba" },                  // light blue
            { "int", "#00a0e8" },                     // sky blue
            { "float", "#00a0e8" },                   // sky blue
            { "double", "#00a0e8" },                  // sky blue
            { "bool", "#00a0e8" },                    // sky blue
            { "char", "#00a0e8" },                    // sky blue
            { "byte", "#00a0e8" },                    // sky blue
            { "short", "#00a0e8" },                   // sky blue
            { "long", "#00a0e8" },                    // sky blue
            { "decimal", "#00a0e8" },                 // sky blue
            { "object", "#00a8a8" },                  // teal
            { "dynamic", "#00a8a8" },                 // teal
            { "readonly", "#00a8a8" },                // teal
            { "const", "#00a8a8" },                   // teal
            { "static", "#00a8a8" },                  // teal
            { "if", "#00a0e8" },                      // sky blue
            { "else", "#00a0e8" },                    // sky blue
            { "else if", "#00a0e8" },                 // sky blue
            { "switch", "#00a0e8" },                  // sky blue
            { "case", "#00a0e8" },                    // sky blue
            { "default", "#00a0e8" },                 // sky blue
            { "for", "#00a0e8" },                     // sky blue
            { "foreach", "#00a0e8" },                 // sky blue
            { "while", "#00a0e8" },                   // sky blue
            { "do", "#00a0e8" },                      // sky blue
            { "try", "#00a0e8" },                     // sky blue
            { "catch", "#00a0e8" },                   // sky blue
            { "finally", "#00a0e8" },                 // sky blue
            { "throw", "#00a0e8" },                   // sky blue
            { "return", "#00a0e8" },                  // sky blue
            { "continue", "#00a0e8" },                // sky blue
            { "break", "#00a0e8" },                   // sky blue
            { "new", "#00a0e8" },                     // sky blue
            { "using", "#00a0e8" },                   // sky blue
            { "namespace", "#00a0e8" },               // sky blue
            { "assembly", "#00a0e8" },                // sky blue
            { "params", "#00a0e8" },                  // sky blue
            { "var", "#00a0e8" },                     // sky blue
            { "true", "#00c3b1" },                    // turquoise
            { "false", "#00c3b1" },                   // turquoise
            { "null", "#00c3b1" },                    // turquoise
            { "this", "#00c3b1" },                    // turquoise
            { "base", "#00c3b1" },                    // turquoise
            { "get", "#00c3b1" },                     // turquoise
            { "set", "#00c3b1" },                     // turquoise
            { "value", "#00c3b1" },                   // turquoise
            { "delegate", "#00a0e8" },                // sky blue
            { "event", "#00a0e8" },                   // sky blue
            { "=>", "#00c3b1" },                      // turquoise
            { "List", "#8258FA" },                    // medium purple
            { "ArrayList", "#8258FA" },               // medium purple
            { "Dictionary", "#8258FA" },              // medium purple
            { "HashSet", "#8258FA" },                 // medium purple
            { "LinkedList", "#8258FA" },              // medium purple
            { "Queue", "#8258FA" },                   // medium purple
            { "Stack", "#8258FA" },                   // medium purple
            { "IEnumerable", "#8258FA" },             // medium purple
            { "IEnumerator", "#8258FA" },             // medium purple
            { "IEnumerable<T>", "#8258FA" },          // medium purple
            { "IEnumerator<T>", "#8258FA" },          // medium purple
            { "#region", "#00a0e8" },                 // sky blue
            { "#endregion", "#00a0e8" },              // sky blue
            { "#if", "#00a0e8" },                     // sky blue
            { "#else", "#00a0e8" },                   // sky blue
            { "#elif", "#00a0e8" },                   // sky blue
            { "#endif", "#00a0e8" },                  // sky blue
            { "#define", "#00a0e8" },                 // sky blue
            { "#undef", "#00a0e8" },                  // sky blue
            { "#warning", "#00a0e8" },                // sky blue
            { "#error", "#00a0e8" },                  // sky blue
            { "#line", "#00a0e8" },                   // sky blue
            { "#nullable", "#00a0e8" },               // sky blue
            { "#pragma", "#00a0e8" },                 // sky blue
            { "#pragma warning", "#00a0e8" },         // sky blue
            { "#pragma checksum", "#00a0e8" },        // sky blue
            { "#pragma warning disable", "#00a0e8" }, // sky blue
            { "#pragma warning restore", "#00a0e8" }, // sky blue
            { "#SCRIPTNAME#", "#8258FA" },            // medium purple
            { "#AUTHORNAME#", "#8258FA" },            // medium purple
            { "#CREATETIME#", "#8258FA" },            // medium purple
        };
    }
}