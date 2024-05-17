//****************** 代码文件申明 ************************
//* 文件：TankInputData                                       
//* 作者：Koo
//* 创建时间：2024/04/23 15:18:44 星期二
//* 功能：nothing
//*****************************************************

using UnityEngine;

namespace GameBuild
{
	public class KanojoInputData : InputData
	{
		/// <summary>
		/// 移动输入
		/// </summary>
		public Vector2 Move;

		/// <summary>
		/// 转向输入
		/// </summary>
		public Vector2 Look;

		/// <summary>
		/// 鼠标指针坐标
		/// </summary>
		public Vector2 MousePos;

		/// <summary>
		/// 主相机
		/// </summary>
		public Camera MainCamera;
	}
}