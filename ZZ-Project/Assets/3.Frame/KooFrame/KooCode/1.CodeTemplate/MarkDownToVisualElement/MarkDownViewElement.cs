//****************** 代码文件申明 ************************
//* 文件：MarkDownViewElement                                       
//* 作者：Koo
//* 创建时间：2024/05/18 20:17:17 星期六
//* 描述：Nothing
//*****************************************************

using System;
using System.Collections;
using Markdig.Syntax.Inlines;
using Markdig.Syntax;
using System.Collections.Generic;
using System.IO;
using Markdig;
using Unity.EditorCoroutines.Editor;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEngine.Networking;

namespace KooFrame
{
    public class MarkDownViewElement : VisualElement
    {
        public string MarkDownContent;

        public CodeMarkData MarkData;

        MarkdownDocument document;


        // VisualElement ParseDocument()
        // {
        //     var renderer = new RenderMarkDown();
        //     var pipelineBuilder = new MarkdownPipelineBuilder()
        //         .UseAutoLinks();
        // }


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
                    VisualElement headingBlockVE = new();
                    headingBlockVE.AddToClassList("HeadingBlock");
                    headingBlockVE.style.unityFontStyleAndWeight = FontStyle.Bold;
                    headingBlockVE.style.fontSize = 24 - (heading.Level * 2);
                    GetInlineContent(heading.Inline, headingBlockVE);
                    parent.Add(headingBlockVE);
                    break;
                case ParagraphBlock paragraph:
                    CreateParagraphBlock(parent, paragraph);
                    break;
                case ListBlock list:
                    var listElement = new VisualElement();
                    listElement.AddToClassList("ListBlock");
                    foreach (var listItem in list)
                    {
                        if (listItem is ListItemBlock item)
                        {
                            var listItemBlockElement = new VisualElement();
                            listItemBlockElement.AddToClassList("ListItemBlock");
                            foreach (var subItem in item)
                            {
                                if (subItem is ParagraphBlock subParagraph)
                                {
                                    VisualElement listItemVe = new();
                                    listItemVe.style.flexDirection = FlexDirection.Row;
                                    Label firstLabel = new("-");
                                    listItemVe.Add(firstLabel);
                                    listItemVe.AddToClassList("ParagraphBlock");
                                    GetInlineContent(subParagraph.Inline, listItemVe);
                                    listItemBlockElement.Add(listItemVe);
                                }
                                else
                                {
                                    //处理子节点
                                    RenderNode(subItem, listElement);
                                }
                            }

                            listElement.Add(listItemBlockElement);
                        }
                    }

                    parent.Add(listElement);
                    break;
                case QuoteBlock quote:
                    VisualElement quoteBlockVE = new();
                    GetBlockContent(quote, quoteBlockVE);
                    quoteBlockVE.AddToClassList("QuoteBlock");
                    parent.Add(quoteBlockVE);
                    break;
                case FencedCodeBlock codeBlock:
                    // var codeLabel = new Label(codeBlock.Lines.ToString());
                    // codeLabel.AddToClassList("FencedCodeBlock");
                    CodeViewVisualElement codeView = new();
                    codeView.Init(codeBlock.Lines.ToString());
                    parent.Add(codeView);
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

        private void CreateParagraphBlock(VisualElement parent, ParagraphBlock paragraph)
        {
            VisualElement paragraphVE = new();
            paragraphVE.AddToClassList("ParagraphBlock");
            GetInlineContent(paragraph.Inline, paragraphVE);
            parent.Add(paragraphVE);
        }


        private void GetInlineContent(ContainerInline container, VisualElement subParent)
        {
            foreach (var inline in container)
            {
                switch (inline)
                {
                    case LiteralInline literal:
                        Label literalInlineLabel = new(literal.Content.ToString())
                        {
                            enableRichText = true,
                            selection =
                            {
                                isSelectable = true,
                                doubleClickSelectsWord = true,
                                tripleClickSelectsLine = true
                            }
                        };
                        literalInlineLabel.AddToClassList("LiteralInline");
                        subParent.Add(literalInlineLabel);
                        break;
                    case EmphasisInline emphasis:
                        VisualElement emphasisInlineVE = new VisualElement();
                        emphasisInlineVE.AddToClassList("EmphasisInline");

                        GetInlineContent(emphasis, emphasisInlineVE);
                        break;
                    case CodeInline codeInline:
                        Label codeInlineLabel = new(codeInline.Content.ToString())
                        {
                            enableRichText = true,
                            selection =
                            {
                                isSelectable = true,
                                doubleClickSelectsWord = true,
                                tripleClickSelectsLine = true
                            }
                        };
                        codeInlineLabel.AddToClassList("CodeInLine");
                        subParent.Add(codeInlineLabel);
                        break;
                    case LinkInline link:
                        if (link.IsImage)
                        {
                            if (link.Url.EndsWith(".gif"))
                            {
                                // UniGIFImageInEditor imageInEditor = new();
                                // imageInEditor.Init(KooCodeWindow.Instance);
                                // EditorCoroutineUtility.StartCoroutine(imageInEditor.SetGifFromUrlCoroutine(link.Url),
                                //     this);
                                // parent.Add(this);
                            }
                            else
                            {
                                Image imageElement = new();
                                subParent.Add(imageElement);
                                EditorCoroutineUtility.StartCoroutine(
                                    LoadTextureFromUrl(GetFullPath(link.Url),
                                        (texture) => { imageElement.image = texture; }), this);
                            }
                        }
                        else
                        {
                            Label linkInlineLabel = new(link.Url.ToString())
                            {
                                enableRichText = true,
                                selection =
                                {
                                    isSelectable = true,
                                    doubleClickSelectsWord = true,
                                    tripleClickSelectsLine = true
                                }
                            };
                            linkInlineLabel.AddToClassList("LinkInline");
                            subParent.Add(linkInlineLabel);
                        }

                        break;
                }
            }
        }

        private void GetBlockContent(ContainerBlock container, VisualElement parent)
        {
            foreach (Block block in container)
            {
                switch (block)
                {
                    case HtmlBlock htmlBlock:
                        break;
                    case ParagraphBlock paragraphBlock:
                        CreateParagraphBlock(parent, paragraphBlock);
                        break;
                    case LeafBlock leafBlock:
                        break;
                    case QuoteBlock quoteBlock:
                        GetBlockContent(quoteBlock, parent);
                        break;
                }
            }
        }

        private string GetFullPath(string url)
        {
            // 判断是否是绝对路径
            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                return url;
            }
            else
            {
                // 处理相对路径
                string path = Path.Combine(KooCode.SettingsData.HexoPath, url);

                // 将本地路径转换为 URI
                if (path.Contains("://") || path.Contains(":///"))
                {
                    return path;
                }
                else
                {
                    return "file:///" + path;
                }
            }
        }


        private IEnumerator LoadTextureFromUrl(string url, System.Action<Texture2D> callback)
        {
            using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("获取失败 " + "[" + url + "]" + www.error);
                    callback(null);
                }
                else
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(www);
                    callback(texture);
                }
            }
        }
    }
}