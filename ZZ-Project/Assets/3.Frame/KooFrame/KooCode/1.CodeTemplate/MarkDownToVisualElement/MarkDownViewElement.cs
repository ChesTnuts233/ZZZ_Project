//****************** 代码文件申明 ************************
//* 文件：MarkDownViewElement                                       
//* 作者：Koo
//* 创建时间：2024/05/18 20:17:17 星期六
//* 描述：用来解析显示MarkDown的Element
//*****************************************************

using System.Collections;
using Markdig.Syntax.Inlines;
using Markdig.Syntax;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Networking;

namespace KooFrame
{
    public class MarkDownViewElement : VisualElement
    {
        public string MarkDownContent;

        public CodeMarkData MarkData;

        MarkdownDocument document;


        #region 动态生成的元素

        List<Label> headingBlockLabels = new List<Label>();

        List<Label> paragraphBlockLabels = new();

        List<Label> listBlockLabels = new();

        #endregion

        public new class UxmlFactory : UxmlFactory<MarkDownViewElement, VisualElement.UxmlTraits> { }


        public void Init(CodeMarkData markDown)
        {
            MarkData = markDown;
            MarkDownContent = MarkData.Content;
            document = MarkData.ParseMarkDown(false);
            this.styleSheets.Add(KooCode.AssetsData.MarkDownStyleSheet);
            this.Clear();
            foreach (var node in document)
            {
                RenderNode(node, this);
            }
        }

        void RenderNode(MarkdownObject node, VisualElement parent)
        {
            switch (node)
            {
                case HeadingBlock heading:
                    GetInlineContent(heading.Inline,parent);

                    // Label headingLabel = new Label(GetInlineContent(heading.Inline));
                    // headingLabel.enableRichText = true;
                    // headingLabel.selection.isSelectable = true;
                    // headingLabel.AddToClassList("HeadingBlock");
                    // headingLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
                    // headingLabel.style.fontSize = 24 - (heading.Level * 2);
                    // parent.Add(headingLabel);
                    break;
                case ParagraphBlock paragraph:
                    GetInlineContent(paragraph.Inline, parent);
                    // Label paragraphLabel = new Label(GetInlineContent(paragraph.Inline));
                    // paragraphLabel.enableRichText = true;
                    // paragraphLabel.selection.isSelectable = true;
                    // paragraphLabel.AddToClassList("ParagraphBlock");
                    // parent.Add(paragraphLabel);
                    break;
                case ListBlock list:
                    var listElement = new VisualElement();
                    listElement.AddToClassList("ListBlock");
                    Debug.Log("List:");
                    foreach (var listItem in list)
                    {
                        if (listItem is ListItemBlock item)
                        {
                            var listItemBlockElement = new VisualElement();
                            listItemBlockElement.AddToClassList("ListItemBlock");
                            listElement.Add(listItemBlockElement);
                            foreach (var subItem in item)
                            {
                                if (subItem is ParagraphBlock subParagraph)
                                {
                                    GetInlineContent(subParagraph.Inline, listItemBlockElement);

                                    // Label listItemLabel = new Label("- " + GetInlineContent(subParagraph.Inline));
                                    // listItemLabel.enableRichText = true;
                                    // listItemLabel.selection.isSelectable = true;
                                    // listItemLabel.AddToClassList("ParagraphBlock");
                                    // listItemBlockElement.Add(listItemLabel);
                                }
                                else
                                {
                                    //处理子节点
                                    RenderNode(subItem, listElement);
                                }
                            }
                        }
                    }

                    parent.Add(listElement);
                    break;
                case LinkInline link:
                    Debug.Log($"Link: {link.Url} - {link.FirstChild?.ToString()}+ {link.IsImage}");
                    if (link.IsImage)
                    {
                        EditorCoroutineUtility.StartCoroutine(LoadTextureFromUrl(link.Url, texture =>
                        {
                            if (texture != null)
                            {
                                var imageElement = new Image { image = texture };
                                parent.Add(imageElement);
                            }
                        }), this);
                    }
                    else
                    {
                        GetInlineContent(link, parent);
                    }

                    break;
                case QuoteBlock quote:
                    GetBlockContent(quote, parent);
                    // quoteLabel.enableRichText = true;
                    // quoteLabel.selection.isSelectable = true;
                    // quoteLabel.AddToClassList("QuoteBlock");
                    // parent.Add(quoteLabel);
                    break;
                case FencedCodeBlock codeBlock:
                    var codeLabel = new Label(codeBlock.Lines.ToString());
                    codeLabel.selection.isSelectable = true;
                    codeLabel.selection.tripleClickSelectsLine = true;
                    codeLabel.selection.doubleClickSelectsWord = true;
                    codeLabel.AddToClassList("FencedCodeBlock");
                    parent.Add(codeLabel);
                    break;
                case ThematicBreakBlock _:
                    var hr = new VisualElement();
                    hr.style.height = 2;
                    hr.style.backgroundColor = new StyleColor(Color.black);
                    parent.Add(hr);
                    break;
                default:
                    if (node is ContainerBlock containerBlock)
                    {
                        foreach (var subNode in containerBlock)
                        {
                            RenderNode(subNode, parent); //递归调用
                        }
                    }

                    break;
            }
        }


        private void GetInlineContent(ContainerInline container, VisualElement parent)
        {
            string content = "";
            foreach (var inline in container)
            {
                if (inline is LiteralInline literal)
                {
                    Label literalLabel = new();
                    literalLabel.text = literal.Content.ToString();
                    literalLabel.selection.isSelectable = true;
                    literalLabel.selection.tripleClickSelectsLine = true;
                    literalLabel.selection.doubleClickSelectsWord = true;
                    literalLabel.AddToClassList("ParagraphBlock");
                    parent.Add(literalLabel);
                }
                else if (inline is EmphasisInline emphasis)
                {
                    VisualElement emphasisVE = new();
                    GetInlineContent(emphasis, emphasisVE);
                    parent.Add(emphasisVE);
                }
                else if (inline is LinkInline link)
                {
                    if (link.IsImage)
                    {
                        EditorCoroutineUtility.StartCoroutine(LoadTextureFromUrl(link.Url, texture =>
                        {
                            if (texture != null)
                            {
                                var imageElement = new Image { image = texture };
                                parent.Add(imageElement);
                            }
                        }), this);
                    }
                    else
                    {
                        var linkLabel = new Label(link.Url);
                        parent.Add(linkLabel);
                    }
                }
                // 你可以继续添加更多的inline类型处理
            }
        }

        private void GetBlockContent(ContainerBlock container, VisualElement parent)
        {
            string content = "";
            foreach (var block in container)
            {
                if (block is ParagraphBlock paragraph)
                {
                    GetInlineContent(paragraph.Inline, parent);
                }
            }
        }


        IEnumerator LoadTextureFromUrl(string url, System.Action<Texture2D> callback)
        {
            using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("获取失败 "+"["+url+"]" + www.error);
                    callback(null);
                }
                else
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(www);
                    callback(texture);
                }
            }
        }

        private void GeneratorTitle() { }


        private void GeneratorNormalText() { }
    }
}