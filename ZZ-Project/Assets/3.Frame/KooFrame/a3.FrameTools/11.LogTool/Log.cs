//****************** 代码文件申明 ************************
//* 文件：Log                                       
//* 作者：Koo
//* 创建时间：2024/04/18 18:20:31 星期四
//* 功能：nothing
//*****************************************************

namespace KooFrame
{
    public static class Log
    {
        private static ILogger logger;
        
        /// <summary>
        /// 是否写入时间
        /// </summary>
        private static bool _isWriteTime = true;
        
        /// <summary>
        /// 是否写入线程ID
        /// </summary>
        private static bool _isWriteThreadID = true;
        
        /// <summary>
        /// 是否写入堆栈路径
        /// </summary>
        private static bool _isWriteTrace = true;
        
        //private static bool 
        
        
        private static bool m_EnableSave = false;
        
        private static string m_SavePath = string.Empty;
        
        private static bool m_UseCustomSaveFileName = false;
        
        private static string m_CustomSaveFileName = string.Empty;
        
        
    }
}