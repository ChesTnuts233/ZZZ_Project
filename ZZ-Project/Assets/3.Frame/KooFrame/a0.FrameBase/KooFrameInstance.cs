using System.Text;
using KooFrame.FrameTools;

namespace KooFrame.BaseSystem
{
	public static class KooFrameInstance
	{
		public class FrameInfo
		{
			private static string _name = "KooFrame";
			private static string _version = "v0.0.3";

			private static string _description =
				"\n这里是Koo工具集框架的根目录文件夹\n" +
				"\nKooFrame目前是为了狂怒传说项目专门编写的游戏框架\n" +
				"\n是Knowledge Of Organization的缩写" +
				"\n此框架学习了很多成熟框架 只是一个为了统一合作 知识整理 编程学习用的框架" +
				"\n\n1. 框架的大部分System静态API 学习自JKFrame、GameFramework、QFramework等各种知名框架 代码著作权依然归属于这些开源作者 感谢\n\n" +
				"\n\n2. 本框架只做整理归纳 只用作个人学习，团队Demo内部学习使用\n\n" +
				"部分API名称和内容有部分修改调整" +
				"\n里面 包含了一些很多开发过程中可以用到的通用工具集\n ";

			private static string _framePath = "/3.Frame/";


			public static string Name { get => _name; set => _name = value; }
			public static string Version { get => _version; private set => _version = value; }
			public static string Description { get => _description; set => _description = value; }
			public static string FramePath { get => _framePath; set => _framePath = value; }

			public static string AssetPath
			{
				get =>"Assets"+_framePath + _name;
			}

			public static string NoAssetPath
			{
				get =>_framePath + _name;
			}
		}


		#region 框架描述

		public static string GetFrameDescription()
		{
			StringBuilder frameDescription = null;
			frameDescription.AppendLine("这里是Koo工具集框架的根目录文件夹");
			frameDescription.AppendLine("Koo 是 Knowledge Of Organization的缩写\n所以此框架是一个知识整理用 编程学习用的框架");
			frameDescription.AppendLine("\n这里面包含了很多开发过程中可以用到的简单工具集\n ");

			return frameDescription.ToString();
		}

		#endregion

		#region 框架名称操作

		/// <summary>
		/// 返回框架工程文件名(有5个默认参数,默认都为false)
		/// </summary>
		/// <param name="isAddDataPath">是否在名称前加到工程文件的绝对路径</param>
		/// <param name="isAddAssetsBefore">是否在本名前加Assets/路径</param>
		/// <param name="isAddVersion">是否添加版本号</param>
		/// <param name="isAddTime">是否加时间</param>
		/// <param name="isAddFileType">是否加类型后缀</param>
		/// <returns>返回被修饰过的框架包名称</returns>
		public static string GetFrameworkFileName
		(
			bool isAddDataPath = false, //是否在名称前加到工程文件的绝对路径
			bool isAddAssetsBefore = false, //是否在本名前加Assets/路径
			bool isAddVersion = false, //是否添加版本号
			bool isAddTime = false, //是否加时间
			bool isAddFileType = false //是否加类型后缀
		)
		{
			StringBuilder sb = new StringBuilder();
			if (isAddDataPath)
			{
#if UNITY_EDITOR
				sb.Append(KooTool.GetLocalPathTo());
#endif
			}

			if (isAddAssetsBefore)
			{
				sb.Append(FrameInfo.FramePath);
			}

			sb.Append(FrameInfo.Name);
			//frameworkName = "KooFramework";
			if (isAddVersion)
			{
				sb.Append("_" + FrameInfo.Version);
			}

			if (isAddTime)
			{
				sb.Append("_" + KooDataTimeTool.GetFormatNowTime(is12Time: false));
			}

			if (isAddFileType)
			{
				sb.Append(".unitypackage");
			}

			string name = sb.ToString();
			return name;
		}

		/// <summary>
		/// 复制框架名称到剪贴板(有5个默认参数，默认都为false)
		/// </summary>
		public static void GetCopyFrameworkFileName
		(
			bool isAddDataPath = false, //是否在名称前加到工程文件的绝对路径
			bool isAddAssetsBefore = false, //是否在本名前加Assets/路径
			bool isAddVersion = false, //是否添加版本号
			bool isAddTime = false, //是否加时间
			bool isAddFileType = false //是否加类型后缀
		)
		{
			KooUtils.SetCopyBuffer(GetFrameworkFileName(isAddDataPath, isAddAssetsBefore,
				isAddVersion, isAddTime, isAddFileType)); //复制字符串到剪贴板
		}

		#endregion

#if UNITY_EDITOR

		#region 框架打包

		/// <summary>
		/// 生成KooFramework框架的UnityPackage包在项目目录下(同时会把框架名称复制到剪贴板上)
		/// </summary>
		//[MenuItem("Assets/My Tools/生成框架UnityPackage", false, 1)]
		public static void GeneratorUnityPackage()
		{
			//使用FileTools内封装的ExportPackage导出包体
			KooTool.ExportPackage(ExtralEditorUtility.FindFoldersWithEndingPath("/KooFrame")[0], GetFrameworkFileName(isAddDataPath: true, isAddAssetsBefore: false, isAddVersion: true, isAddTime: true,
				isAddFileType: true));
			KooTool.OpenLocalPath();
			GetCopyFrameworkFileName(isAddTime: true, isAddVersion: true); //复制文件名到剪贴板
		}

		#endregion

#endif
	}
}