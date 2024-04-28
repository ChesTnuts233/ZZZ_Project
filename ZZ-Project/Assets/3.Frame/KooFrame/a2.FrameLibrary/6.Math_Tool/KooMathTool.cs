using System;
using UnityEngine;

namespace KooFrame
{
	public static partial class KooTool
	{
		#region 数值常量

		/// <summary>
		/// 十万
		/// </summary>
		private const int Hundred_Thousand = 100000;

		/// <summary>
		/// 亿
		/// </summary>
		private const int Hundred_Million = 100000000;

		/// <summary>
		/// π (圆周率,一般用系统的 这个精度高一点点)
		/// </summary>
		public const float Pi = 3.1415926535f;

		/// <summary>
		/// 自然对数的底数 一般用系统的 这个精度高一点点
		/// </summary>
		public const float Euler = 2.7182818284f;

		/// <summary>
		/// 角度转弧度的乘数
		/// </summary>
		public const float DegreesToRadians = Pi / 180.0f;

		/// <summary>
		/// 弧度转角度的乘数
		/// </summary>
		public const float RadiansToDegrees = 180.0f / Pi;

		/// <summary>
		/// 黄金比例
		/// </summary>
		public const float GoldenRatio = 1.61803f;

		/// <summary>
		/// 一小时的秒数
		/// </summary>
		public const int SecondsPerHour = 3600;

		/// <summary>
		/// 一天的秒数
		/// </summary>
		public const int SecondsPerDay = 86400;

		/// <summary>
		/// 一周的秒数
		/// </summary>
		public const int SecondsPerWeek = 604800;

		/// <summary>
		/// 一年的秒数（平均）
		/// </summary>
		public const int SecondsPerYear = 31536000;

		/// <summary>
		/// 常见的重力加速度（地球）
		/// </summary>
		public const float StandardGravity = 9.80665f; // m/s²

		/// <summary>
		/// 速度光在真空中的常数
		/// </summary>
		public const float SpeedOfLight = 299792458.0f; // m/s

		#endregion

		#region 获得数值的个,十,百,千位数值

		/// <summary>
		/// 返回一个整数的个位数。
		/// </summary>
		/// <param name="number">要获取个位数的整数。</param>
		/// <returns>输入整数的个位数。</returns>
		public static int ReturnSplitUnit(int number)
		{
			return Mathf.Abs(number) % 10;
		}

		/// <summary>
		/// 返回一个整数的十位数。
		/// </summary>
		/// <param name="number">要获取十位数的整数。</param>
		/// <returns>输入整数的十位数。</returns>
		public static int ReturnSplitTen(int number)
		{
			return Mathf.FloorToInt(Mathf.Abs(number) * 0.1f) % 10;
		}

		/// <summary>
		/// 返回一个整数的百位数。
		/// </summary>
		/// <param name="number">要获取百位数的整数。</param>
		/// <returns>输入整数的百位数。</returns>
		public static int ReturnSplitHundred(int number)
		{
			return Mathf.FloorToInt(Mathf.Abs(number) * 0.01f) % 10;
		}

		/// <summary>
		/// 返回一个整数的千位数。
		/// </summary>
		/// <param name="number">要获取千位数的整数。</param>
		/// <returns>输入整数的千位数。</returns>
		public static int ReturnSplitThousand(int number)
		{
			return Mathf.FloorToInt(Mathf.Abs(number) * 0.001f) % 10;
		}

		/// <summary>
		/// 返回一个整数的个位数和十位数。
		/// </summary>
		/// <param name="number">要获取个位数和十位数的整数。</param>
		/// <param name="unit">输出的个位数。</param>
		/// <param name="ten">输出的十位数。</param>
		public static void ReturnSplitToTen(int number, out int unit, out int ten)
		{
			number = Mathf.Abs(number);
			unit = number % 10;
			//Mathf.FloorToInt 向下取整
			ten = Mathf.FloorToInt(number * 0.1f) % 10;
		}

		/// <summary>
		/// 返回一个整数的个位数、十位数和百位数。
		/// </summary>
		/// <param name="number">要获取个位数、十位数和百位数的整数。</param>
		/// <param name="unit">输出的个位数。</param>
		/// <param name="ten">输出的十位数。</param>
		/// <param name="hundred">输出的百位数。</param>
		public static void ReturnSplitToHundred(int number, out int unit, out int ten, out int hundred)
		{
			number = Mathf.Abs(number);
			unit = number % 10;
			ten = Mathf.FloorToInt(number * 0.1f) % 10;
			hundred = Mathf.FloorToInt(number * 0.01f) % 10;
		}

		/// <summary>
		/// 返回一个整数的个位数、十位数、百位数和千位数。
		/// </summary>
		/// <param name="number">要获取个位数、十位数、百位数和千位数的整数。</param>
		/// <param name="unit">输出的个位数。</param>
		/// <param name="ten">输出的十位数。</param>
		/// <param name="hundred">输出的百位数。</param>
		/// <param name="thousand">输出的千位数。</param>
		public static void ReturnSplitToThousand(int number, out int unit, out int ten, out int hundred,
			out int thousand)
		{
			number = Mathf.Abs(number);
			unit = number % 10;
			ten = Mathf.FloorToInt(number * 0.1f) % 10;
			hundred = Mathf.FloorToInt(number * 0.01f) % 10;
			thousand = Mathf.FloorToInt(number * 0.001f) % 10;
		}

		/// <summary>
		/// 获得一个数的位数
		/// </summary>
		public static int GetDigitCount(int number)
		{
			int count = 0;

			if (number == 0)
			{
				return 1;
			}
			number = Math.Abs(number);

			while (number > 0)
			{
				number /= 10;
				count++;
			}
			return count;
		}

		#endregion

		#region 数值转换成字符串

		/// <summary>
		/// 将大数值转换为带有对应单位的字符串
		/// </summary>
		/// <param name="number">数值，不一定为整型，且long / int = long / long</param>
		/// <returns></returns>
		public static string BigNumberToUnitString(double number)
		{
			if (number >= Hundred_Million)
			{
				return $"{GetPreciseDecimal((float)(number / Hundred_Million), 2)}亿";
			}
			else if (number >= Hundred_Thousand)
			{
				return $"{GetPreciseDecimal((float)(number / Hundred_Thousand), 2) * 10}万";
			}
			else
			{
				return number.ToString();
			}
		}

		/// <summary>
		/// 对应数值保留几位小数,float精度大约为6-9位数字，double精度大约15-17位数字
		/// </summary>
		/// <param name="number">原始数值</param>
		/// <param name="decimalPlaces">保留小数位数</param>
		/// <returns></returns>
		public static float GetPreciseDecimal(float number, int decimalPlaces = 0)
		{
			if (decimalPlaces < 0)
				decimalPlaces = 0;

			int powerNumber = (int)Mathf.Pow(10, decimalPlaces);
			float tmeporary = number * powerNumber;
			return (float)Math.Round(tmeporary / powerNumber, decimalPlaces);
		}

		/// <summary>
		/// 对应数值保留几位小数,float精度大约为6-9位数字，double精度大约15-17位数字
		/// </summary>
		/// <param name="number">原始数值</param>
		/// <param name="decimalPlaces">保留小数位数</param>
		/// <returns></returns>
		public static double GetPreciseDecimal(double number, int decimalPlaces = 0)
		{
			if (decimalPlaces < 0)
				decimalPlaces = 0;

			int powerNumber = (int)Mathf.Pow(10, decimalPlaces);
			double tmeporary = number * powerNumber;
			//Math.Round，参数二将双精度浮点值舍入到指定数量的小数位数。
			return Math.Round(tmeporary / powerNumber, decimalPlaces);
		}

		/// <summary>
		/// 数字0-9转换为中文数字
		/// </summary>
		/// <param name="num"></param>
		/// <returns>中文大写数值</returns>
		public static string SingleDigitsNumberToChinese(int num = 0)
		{
			num = Mathf.Clamp(num, 0, 9);
			//切分出字符数组
			string[] unitAllString = "零,一,二,三,四,五,六,七,八,九".Split(',');
			return unitAllString[num];
		}

		/// <summary>
		/// 数字转换为中文，可以适用任何3位及其以下数值
		/// </summary>
		/// <param name="number"></param>
		/// <returns></returns>
		public static string NumberToCNString(int number)
		{
			string numStr = number.ToString();
			string resultStr = "";
			int strLength = numStr.Length;
			//切分出字符数组
			string[] unitAllString = KooTool.GetSpliteStringFormat("零,一,二,三,四,五,六,七,八,九");
			string units = "", tens = "", hundreds = "";
			string tenStr = "十";
			string hundredStr = "百";

			for (int i = 1; i <= strLength; i++)
			{
				//Substring第一个参数为从第几个字符索引位置开始截取， 参数二为截取的长度
				int sNum = Convert.ToInt32(numStr.Substring(i - 1, 1));
				string cnStr = unitAllString[sNum];
				if (i == 1)
				{
					units = cnStr;
					resultStr = cnStr;
				}
				else if (i == 2)
				{
					tens = cnStr;
					//判断十位是否是0
					if (tens == unitAllString[0])
					{
						if (units == unitAllString[1])
							resultStr = tenStr;
						else
							//例如二十，三十
							resultStr = units + tenStr;
					}
					else
					{
						if (units == unitAllString[1])
							resultStr = tenStr + tens;
						else
							resultStr = units + tenStr + tens;
					}
				}
				else if (i == 3)
				{
					hundreds = cnStr;
					//判断百位是否是0
					if (hundreds == unitAllString[0])
					{
						if (tens.Equals(unitAllString[0]))
							resultStr = units + hundredStr;
						else
							//例如一百一，二百一
							resultStr = units + hundredStr + tens;
					}
					else if (tens == unitAllString[0])
						resultStr = units + hundredStr + tens + hundreds;
					else
						resultStr = units + hundredStr + tens + tenStr + hundreds;
				}
			}

			return resultStr;
		}

		/// <summary>
		/// 数值转成英文序号格式（小于100的数值）
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public static string NumberToSequence(long num = 1)
		{
			string temporaryStr = "";
			if (num > 100)
				return temporaryStr;
			if (num % 10 == 1)
				temporaryStr = "ST";
			else if (num % 10 == 2)
				temporaryStr = "ND";
			else if (num % 10 == 3)
				temporaryStr = "RD";
			else
				temporaryStr = "TH";
			return num.ToString() + temporaryStr;
		}

		#endregion

		/// <summary>
		/// 判断数字是否为偶数
		/// </summary>
		/// <param name="number">判断的数字</param>
		/// <returns></returns>
		public static bool IsEven(int number)
		{
			return number % 2 == 0;
		}

		#region 数值映射

		/// <summary>
		/// 将当前值的取值范围映射到另外一个取值范围的新值
		/// </summary>
		/// <param name="value">当前值</param>
		/// <param name="fromMin">从范围A的最小值</param>
		/// <param name="fromMax">到范围A的最大值</param>
		/// <param name="toMin">从范围B的最小值</param>
		/// <param name="toMax">到范围B的最大值</param>
		/// <returns>目标映射范围中的值</returns>
		public static float MapValue(float value, float fromMin, float fromMax, float toMin, float toMax)
		{
			// 将值从一个范围映射到另一个范围
			return (value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;
		}

		#endregion
	}
}