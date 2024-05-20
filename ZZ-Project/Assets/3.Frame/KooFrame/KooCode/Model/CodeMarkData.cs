using Markdig;
using Markdig.Syntax.Inlines;
using Markdig.Syntax;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace KooFrame
{
    [Serializable]
    public class CodeMarkData : CodeData
    {
        /// <summary>
        /// MarkDown
        /// </summary>
        [SerializeField] public TextAsset CodeMarkDown;

        [FilePath] public string MarkDownPath;


        public string MarkDownContent
        {
            get => Content;
            set => Content = value;
        }

        [Button("解析MarkDown内容")]
        public MarkdownDocument ParseMarkDown(bool isLog)
        {
            var pipeline = new MarkdownPipelineBuilder().Build();
            MarkdownDocument document = Markdown.Parse(CodeMarkDown.text, pipeline);
            if (isLog)
            {
                ExtractElements(document);
            }

            return document;
        }

        public override void UpdateData()
        {
            base.UpdateData();
            Content = CodeMarkDown.text;
        }


        public void ExtractElements(MarkdownDocument document)
        {
            foreach (var node in document.Descendants())
            {
                switch (node)
                {
                    case HeadingBlock heading:
                        Debug.Log($"Heading {heading.Level}: {GetInlineContent(heading.Inline)}");
                        break;
                    case ParagraphBlock paragraph:
                        Debug.Log($"Paragraph: {GetInlineContent(paragraph.Inline)}");
                        break;
                    case ListBlock list:
                        Debug.Log("List:");
                        foreach (var listItem in list)
                        {
                            if (listItem is ListItemBlock item)
                            {
                                foreach (var subItem in item)
                                {
                                    if (subItem is ParagraphBlock subParagraph)
                                    {
                                        Debug.Log($"  - {GetInlineContent(subParagraph.Inline)}");
                                    }
                                }
                            }
                        }

                        break;
                    case LinkInline link:
                        Debug.Log($"Link: {link.Url} - {link.FirstChild?.ToString()}+ {link.IsImage}");
                        break;
                    case QuoteBlock quote:
                        Debug.Log($"Quote: {GetBlockContent(quote)}");
                        break;
                    // 你可以继续添加更多的case来处理其他类型的元素
                    default:
                        break;
                }
            }
        }

        static string GetInlineContent(ContainerInline container)
        {
            string content = "";
            foreach (var inline in container)
            {
                if (inline is LiteralInline literal)
                {
                    content += literal.Content.ToString();
                }
                else if (inline is EmphasisInline emphasis)
                {
                    content += GetInlineContent(emphasis);
                }
                else if (inline is LinkInline link)
                {
                    content += $"[{GetInlineContent(link)}]({link.Url})";
                }
                // 你可以继续添加更多的inline类型处理
            }

            return content;
        }

        static string GetBlockContent(ContainerBlock container)
        {
            string content = "";
            foreach (var block in container)
            {
                if (block is ParagraphBlock paragraph)
                {
                    content += GetInlineContent(paragraph.Inline);
                }
            }

            return content;
        }
    }
}