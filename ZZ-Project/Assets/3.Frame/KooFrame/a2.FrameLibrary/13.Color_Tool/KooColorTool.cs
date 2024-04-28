﻿using System.Text.RegularExpressions;
using UnityEngine;

namespace KooFrame
{
    public static partial class KooTool
    {
        /// <summary>
        /// 灰度值
        /// </summary>
        public static Color GrayscaleValue = new Color(0.299f, 0.587f, 0.114f, 1f);

        /// <summary>
        /// 检查输入的字符串是否符合基本的十六进制颜色格式。
        /// </summary>
        /// <example>
        /// <code>
        /// string colorStr1 = "#FF0000"; // 符合格式的颜色字符串
        /// string colorStr2 = "#ABC"; // 符合格式的颜色字符串
        /// string colorStr3 = "#ZZZ"; // 不符合格式的颜色字符串
        /// </code>
        /// </example>>
        /// <param name="inputStr">要检查的字符串。</param>
        /// <returns>如果字符串符合基本的十六进制颜色格式，则返回 true；否则返回 false。</returns>
        public static bool IsBas16ColorFormat(string inputStr)
        {
            Regex reg = new Regex(@"^#?([a-fA-F0-9]{6}|[a-fA-F0-9]{3})$");
            return reg.IsMatch(inputStr);
        }

        /// <summary>
        /// 转换HSV颜色数值 从0 360到0 1 
        /// </summary>
        /// <returns>mappedValue 映射值</returns>
        public static float Convert360To10(this float originValue)
        {
            return originValue / 360f;
        }

        // public static Color ChangeAlpha(this Color color, float alphaValue)
        // {
        //     return new Color(color.r, color.g, color.b, alphaValue);
        // }
    }
}