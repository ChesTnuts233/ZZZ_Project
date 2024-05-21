//****************** 代码文件申明 ************************
//* 文件：RendererBlockHeading                                       
//* 作者：Koo
//* 创建时间：2024/05/20 16:23:54 星期一
//* 功能：nothing
//*****************************************************

using Markdig.Renderers;
using Markdig.Syntax;

namespace KooFrame
{
    public class RendererBlockHeading : MarkdownObjectRenderer<RenderMarkDown, HeadingBlock>
    {
        protected override void Write(RenderMarkDown renderer, HeadingBlock headingBlock)
        {
            renderer.WriteLeafBlockInline(headingBlock);
        }
    }
}