using System.Linq;
using System.Text.RegularExpressions;

namespace KooFrame
{
    public static partial class KooTool
    {
        #region 字符串拆分与替换

        public static char[] Punctuations = {
            '。',
            '.',
            ';',
            '；',
            '?',
            '？',
            '!',
            '！',
            ',',
            '，'
        };

        /// <summary>
        /// 按照,#+|，将字符串进行拆分
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static string[] GetSpliteStringFormat(string inputStr)
        {
            Regex reg = new Regex("[,#+|]");
            return reg.Split(inputStr);
        }

        /// <summary>
        /// 将输入字符串中的空格、换行符、新行、tab字符替换为指定的字符串。
        /// </summary>
        /// <param name="inputStr">要替换的输入字符串。</param>
        /// <param name="replaceStr">要用于替换空格的字符串（默认为空字符串）。</param>
        /// <returns>替换后的字符串。</returns>
        public static string ReplaceSpaceByFormat(string inputStr, string replaceStr = "")
        {
            Regex reg = new Regex(@"\s");
            return reg.Replace(inputStr, replaceStr);
        }

        /// <summary>
        /// 仅以指定格式替换左右两端的空格，string里的 Trim也可删除左右两端空格
        /// </summary>
        /// <param name="inputStr"></param>
        /// <param name="replaceStr">要替换成的字符</param>
        /// <returns></returns>
        public static string ReplaceTrimSpaceByFormat(string inputStr, string replaceStr = "")
        {
            Regex reg = new Regex(@"(^\s*)|(\s*$)");
            return reg.Replace(inputStr, replaceStr);
        }

        /// <summary>
        /// 仅以指定格式替换左端的空格，string里的 TrimStart也可删除左端空格
        /// </summary>
        /// <param name="inputStr"></param>
        /// <param name="replaceStr">要替换成的字符</param>
        /// <returns></returns>
        public static string ReplaceTrimStartByFormat(string inputStr, string replaceStr = "")
        {
            Regex reg = new Regex(@"(^\s*)");
            return reg.Replace(inputStr, replaceStr);
        }

        /// <summary>
        /// 仅以指定格式替换右端的空格，string里的 TrimEnd也可删除右端空格
        /// </summary>
        /// <param name="inputStr"></param>
        /// <param name="replaceStr">要替换成的字符</param>
        /// <returns></returns>
        public static string ReplaceTrimEndByFormat(string inputStr, string replaceStr = "")
        {
            Regex reg = new Regex(@"(\s*$)");
            return reg.Replace(inputStr, replaceStr);
        }

        #endregion

        /// <summary>
        /// 是否是数字和字母组成
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static bool IsNumberAndLetterStruct(string inputStr)
        {
            Regex reg = new Regex("^[A-Za-z0-9]+$");
            return reg.IsMatch(inputStr);
        }

        /// <summary>
        /// 判断字符是否由数字或者字母组成 调用的是char的系统api
        /// </summary>
        /// <param name="inputChar">输入的字符</param>
        /// <returns>是数字或者字母返回true 反之false</returns>
        public static bool IsNumberAndLetterStruct(char inputChar)
        {
            Regex reg = new Regex("^[A-Za-z0-9]+$");
            return reg.IsMatch(inputChar.ToString());
        }

        /// <summary>
        /// 判断一个字符是否是用来进行文本分割的标点符号 比如中英文中的逗号 句号 分号等
        /// </summary>
        /// <param name="inputChar"></param>
        /// <returns></returns>
        public static bool IsSegmentationPunctuations(char inputChar)
        {
            //return Punctuations.Contains(inputChar);
            return inputChar switch
            {
                '。' => true,
                '.' => true,
                ';' => true,
                '；' => true,
                '?' => true,
                '？' => true,
                '!' => true,
                '！' => true,
                ',' => true,
                '，' => true,
                '=' => true,
                _ => false
            };
        }

        /// <summary>
        /// 判断字符是否是中文汉字
        /// </summary>
        /// <param name="word">字符</param>
        /// <returns></returns>
        public static bool IsChineseChar(char word)
        {
            //在ASCII码中 中文汉字的范围以下区间
            return 0x4e00 <= word && word <= 0x9fbb;
        }

        /// <summary>
        /// 是否是小写英文字母组成
        /// </summary>
        /// <param name="inputStr">例如russel</param>
        /// <returns></returns>
        public static bool IsLowercaseLetterStruct(string inputStr)
        {
            Regex reg = new Regex("^[a-z]+$");
            return reg.IsMatch(inputStr);
        }

        /// <summary>
        /// 是否由大写英文字母组成
        /// </summary>
        /// <param name="inputStr">例如ABC</param>
        /// <returns></returns>
        public static bool IsUppercaseLetterStruct(string inputStr)
        {
            Regex reg = new Regex("^[A-Z]+$");
            return reg.IsMatch(inputStr);
        }

        /// <summary>
        /// 对字符串操作的拓展
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string content)
        {
            return string.IsNullOrEmpty(content);
        }
        
        
        
        
    }
}