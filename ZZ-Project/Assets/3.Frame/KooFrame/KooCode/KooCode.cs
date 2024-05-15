//****************** 代码文件申明 ************************
//* 文件：KooCode                                       
//* 作者：Koo
//* 创建时间：2024/05/14 17:09:38 星期二
//* 描述：Nothing
//*****************************************************

using UnityEditor;
using UnityEngine;

namespace KooFrame
{
	public class KooCode
	{
		#region 数据

		public static string CodeDatasPath = "Assets/3.Frame/KooFrame/KooCode/0.Data/CodeDatas.asset";

		public static string CodeSettingsPath = "Assets/3.Frame/KooFrame/KooCode/0.Data/AssetsData.asset";


		private static KooCodeDatas datasInstance;

		public static KooCodeDatas Datas
		{
			get
			{
				if (datasInstance == null)
				{
					datasInstance = AssetDatabase.LoadAssetAtPath<KooCodeDatas>(CodeDatasPath);
				}
				return datasInstance;
			}
		}



		private static CodeAssetsData assetsData;


		public static CodeAssetsData AssetsData
		{
			get
			{
				if (assetsData == null)
				{
					assetsData = AssetDatabase.LoadAssetAtPath<CodeAssetsData>(CodeSettingsPath);
				}
				return assetsData;
			}
		}

		#endregion
	}
}
