//****************** 代码文件申明 ************************
//* 文件：UnityLoggger                                       
//* 作者：Koo
//* 创建时间：2024/04/18 18:00:48 星期四
//* 功能：对Unity的Log进行一层封装
//*****************************************************

using System;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace KooFrame
{
    public class UnityLoggger : ILogger
    {
        private MethodInfo logFunction;
        private MethodInfo errorFunction;
        private MethodInfo warningFunction;


        public UnityLoggger()
        {
            //反射得到Unity的Debug
            Type type = Type.GetType("UnityEngine.Debug,UnityEngine");
            //在反射得到的类中 得到各个Log方法 以便于间接调用
            this.logFunction = type.GetMethod("Log", new Type[1]
            {
                typeof(object)
            });
            this.errorFunction = type.GetMethod("LogError", new Type[1]
            {
                typeof(object)
            });
            this.warningFunction = type.GetMethod("LogWarning", new Type[1]
            {
                typeof(object)
            });
        }


        public void Log(string message)
        {
            //直接调用反射得来的委托
            this.logFunction.Invoke((object)null, new object[1]
            {
                (object)message
            });
        }

        public void Warning(string message)
        {
            message = this.Decorate(FrameLogType.Warning, message);
            this.warningFunction.Invoke((object)null, new object[1]
            {
                (object)message
            });
        }

        public void Error(string message)
        {
            message = this.Decorate(FrameLogType.Error, message);
            this.errorFunction.Invoke((object)null, new object[1]
            {
                (object)message
            });
        }

        public void Succeed(string message)
        {
            message = this.Decorate(FrameLogType.Succeed, message);
            this.logFunction.Invoke((object)null, new object[1]
            {
                (object)message
            });
        }


        /// <summary>
        /// 装饰
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private string Decorate(FrameLogType logType, string msg)
        {
            string[] strArray = msg.Split(new string[1] { "\n" }, StringSplitOptions.None);
            StringBuilder stringBuilder = new StringBuilder();
            switch (logType)
            {
                case FrameLogType.Warning:
                    foreach (string str in strArray)
                        stringBuilder.AppendFormat("<color=#FFFF00>{0}</color>\n", (object)str);
                    break;
                case FrameLogType.Error:
                    foreach (string str in strArray)
                        stringBuilder.AppendFormat("<color=#FF0000>{0}</color>\n", (object)str);
                    break;
                case FrameLogType.Succeed:
                    foreach (string str in strArray)
                        stringBuilder.AppendFormat("<color=#00FF00>{0}</color>\n", (object)str);
                    break;
            }

            return stringBuilder.ToString();
        }
    }
}