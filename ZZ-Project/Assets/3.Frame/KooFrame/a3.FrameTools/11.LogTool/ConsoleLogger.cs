//****************** 代码文件申明 ************************
//* 文件：ConsoleLogger                                       
//* 作者：Koo
//* 创建时间：2024/04/18 17:56:44 星期四
//* 功能：对控制台的封装
//*****************************************************

using System;

namespace KooFrame
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message) => Console.WriteLine(message);

        public void Warning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void Error(string message)
        {
            //定义输出颜色
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void Succeed(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}