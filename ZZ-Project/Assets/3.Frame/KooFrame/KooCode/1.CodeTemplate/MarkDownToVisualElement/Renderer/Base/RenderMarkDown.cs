//****************** 代码文件申明 ************************
//* 文件：RenderMarkDown                                       
//* 作者：Koo
//* 创建时间：2024/05/20 16:13:48 星期一
//* 功能：nothing
//*****************************************************

using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using UnityEngine.UIElements;

namespace KooFrame
{
    public class RenderMarkDown : RendererBase
    {
        public VisualElement Root;

        public bool ConsumeSpace = false;
        public bool ConsumeNewLine = false;
        private string link = null;


        public override object Render(MarkdownObject document)
        {
            Write(document);
            return this;
        }


        public RenderMarkDown()
        {
            //ObjectRenderers.Add();
            
        }
        
        
        
        
        public void BindBlock(VisualElement bindBlock)
        {
            Root.Add(bindBlock);
        }

        /// <summary>
        /// 渲染LeafRawLines并返回一个元素集
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        public VisualElement WriteLeafRawLines(CodeBlock block)
        {
            if (block.Lines.Lines == null)
            {
                return null;
            }

            VisualElement leafRawLines = new();

            var lines = block.Lines;
            var slices = lines.Lines;
            for (int i = 0; i < lines.Count; i++)
            {
                Label line = new()
                {
                    selection =
                    {
                        isSelectable = true,
                    },
                    text = slices[i].ToString()
                };

                leafRawLines.Add(line);
            }

            return leafRawLines;
        }

        public void WriteLeafBlockInline(LeafBlock block)
        {
            Inline inline = block.Inline;
            while (inline != null)
            {
                Write(inline);
                inline = inline.NextSibling;
            }
        }
    }
}