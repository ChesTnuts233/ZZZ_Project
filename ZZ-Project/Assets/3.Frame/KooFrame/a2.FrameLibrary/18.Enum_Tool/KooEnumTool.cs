//****************** 代码文件申明 ************************
//* 文件：KooEnumTool                                       
//* 作者：Koo
//* 创建时间：2024/04/23 12:17:42 星期二
//* 功能：nothing
//*****************************************************

namespace KooFrame
{
    public partial class KooTool
    {
        public static int ToInt(this System.Enum e)
        {
            return e.GetHashCode();
        }
    }
}