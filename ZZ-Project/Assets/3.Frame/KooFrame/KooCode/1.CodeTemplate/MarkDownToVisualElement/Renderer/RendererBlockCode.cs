//****************** 代码文件申明 ************************
//* 文件：RendererBlockCode                                       
//* 作者：Koo
//* 创建时间：2024/05/20 16:12:04 星期一
//* 功能：块代码渲染
//*****************************************************

using Markdig.Renderers;
using Markdig.Syntax;
using UnityEngine.UIElements;

namespace KooFrame
{
    public class RendererBlockCode : MarkdownObjectRenderer<RenderMarkDown, CodeBlock>
    {
        protected override void Write(RenderMarkDown renderer, CodeBlock block)
        {
            FencedCodeBlock fencedCodeBlock = block as FencedCodeBlock;

            if (fencedCodeBlock != null && !string.IsNullOrEmpty(fencedCodeBlock.Info))
            {
                //这里调用语法高亮的字典
            }

            VisualElement blockCodeVE = new();

            blockCodeVE.AddToClassList("BlockCode");
            VisualElement blockLeafRawLinesVE = renderer.WriteLeafRawLines(block);

            blockCodeVE.Add(blockLeafRawLinesVE);
            renderer.BindBlock(blockCodeVE);
        }
    }
}