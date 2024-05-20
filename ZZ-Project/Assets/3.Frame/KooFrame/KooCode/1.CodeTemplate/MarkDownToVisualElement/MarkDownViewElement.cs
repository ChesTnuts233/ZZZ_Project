//****************** 代码文件申明 ************************
//* 文件：MarkDownViewElement                                       
//* 作者：Koo
//* 创建时间：2024/05/18 20:17:17 星期六
//* 描述：Nothing
//*****************************************************

using Markdig.Syntax.Inlines;
using Markdig.Syntax;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;

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

		public new class UxmlFactory : UxmlFactory<MarkDownViewElement, VisualElement.UxmlTraits>
		{

		}


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
					Label headingLabel = new Label(GetInlineContent(heading.Inline));
					headingLabel.enableRichText = true;
					headingLabel.selection.isSelectable = true;
					headingLabel.AddToClassList("HeadingBlock");
					headingLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
					headingLabel.style.fontSize = 24 - (heading.Level * 2);
					parent.Add(headingLabel);
					break;
				case ParagraphBlock paragraph:
					Label paragraphLabel = new Label(GetInlineContent(paragraph.Inline));
					paragraphLabel.enableRichText = true;
					paragraphLabel.selection.isSelectable = true;
					paragraphLabel.AddToClassList("ParagraphBlock");
					parent.Add(paragraphLabel);
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
							listElement.Add(listItemBlockElement);
							foreach (var subItem in item)
							{
								if (subItem is ParagraphBlock subParagraph)
								{
									Label listItemLabel = new Label("- " + GetInlineContent(subParagraph.Inline));
									listItemLabel.enableRichText = true;
									listItemLabel.selection.isSelectable = true;
									listItemLabel.AddToClassList("ParagraphBlock");
									listItemBlockElement.Add(listItemLabel);
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
				case QuoteBlock quote:
					var quoteLabel = new Label(GetBlockContent(quote));
					quoteLabel.enableRichText = true;
					quoteLabel.selection.isSelectable = true;
					quoteLabel.AddToClassList("QuoteBlock");
					quoteLabel.style.unityFontStyleAndWeight = FontStyle.Italic;
					parent.Add(quoteLabel);
					break;
				case LinkInline link:
					var linkLabel = new Label($"{GetInlineContent(link)} ({link.Url})");
					parent.Add(linkLabel);
					break;
				case FencedCodeBlock codeBlock:
					var codeLabel = new Label(codeBlock.Lines.ToString());
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



		private void GeneratorTitle()
		{

		}


		private void GeneratorNormalText()
		{

		}
	}
}
