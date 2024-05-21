using MG.MDV;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


namespace KooFrame
{
    /// <summary>
    /// 代码笔记监视器
    /// </summary>
    public class CodeMarkInspector : CodeInspector
    {
        public CodeMarkData curCheckMarkData;
        
        #region 元素

        private MarkDownViewElement markDownViewElement;

        private VisualTreeAsset container_assets;

        private IMGUIContainer imGUIcontainer;

        private MarkdownViewer viewer;

        private Editor editor;

        private Button checkBtn;

        #endregion

        private bool isCheckOrViewer;

        public new class UxmlFactory : UxmlFactory<CodeMarkInspector, VisualElement.UxmlTraits> { }

        /// <summary>
        /// 绑定到管理窗口
        /// </summary>
        /// <param name="managerWindow"></param>
        public override void BindToManagerWindow(KooCodeWindow managerWindow)
        {
            base.BindToManagerWindow(managerWindow);
            container_assets = KooCode.AssetsData.MarkInspectorVisualTreeAsset;
            container_assets.CloneTree(this);
            
            imGUIcontainer = this.Q<IMGUIContainer>("MarkDownView");

            UnityEngine.Object.DestroyImmediate(editor);
            editor = UnityEditor.Editor.CreateEditor(container_assets);

            markDownViewElement = this.Q<MarkDownViewElement>("MarkDownViewElement");
            
            checkBtn = this.Q<Button>("CheckBtn");

            markDownViewElement.style.display = DisplayStyle.Flex;
            imGUIcontainer.style.display = DisplayStyle.None;
            isCheckOrViewer = true;
            checkBtn.clicked += () =>
            {
                isCheckOrViewer = !isCheckOrViewer;
                if (isCheckOrViewer == true)
                {
                    markDownViewElement.style.display = DisplayStyle.Flex;
                    imGUIcontainer.style.display = DisplayStyle.None;
                }
                else
                {
                    markDownViewElement.style.display = DisplayStyle.None;
                    imGUIcontainer.style.display = DisplayStyle.Flex;
                }
            };
        }

        public override void Show()
        {
            this.style.display = DisplayStyle.Flex;
            imGUIcontainer.onGUIHandler += OnMarkDownViewGUI;
            //添加绘制监听
            EditorApplication.update += UpdateRequests;
        }

        public override void Close()
        {
            base.Close();
            this.style.display = DisplayStyle.None;
            imGUIcontainer.onGUIHandler -= OnMarkDownViewGUI;
            EditorApplication.update -= UpdateRequests;
        }

        private void OnMarkDownViewGUI()
        {
            //viewer.Draw();
            viewer.UpdateText(curCheckMarkData.MarkDownContent);
            viewer.Draw();
        }

        void UpdateRequests()
        {
            if (viewer.Update())
            {
                editor.Repaint();
            }
        }

        public override void UpdateInspector()
        {
            UpdateInspector(curCheckMarkData);
        }

        public void UpdateInspector(CodeMarkData data)
        {
            curCheckMarkData = data;
            string content = (data.CodeMarkDown).text;
            viewer = new MG.MDV.MarkdownViewer(KooCode.AssetsData.DarkSkin, curCheckMarkData.MarkDownPath, content);
            markDownViewElement.Init(data);
        }
    }
}