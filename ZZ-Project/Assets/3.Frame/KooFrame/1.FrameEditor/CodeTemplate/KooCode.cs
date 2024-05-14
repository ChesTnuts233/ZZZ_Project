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
	public class KooCode : Editor
	{
		#region 数据

		public static string CodeDatasPath = "Assets/3.Frame/KooFrame/1.FrameEditor/CodeTemplate/Data/CodeDatas.asset";

		public static string CodeSettingsPath = "Assets/3.Frame/KooFrame/1.FrameEditor/CodeTemplate/Data/SettingsData.asset";


		[SerializeField]
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



		[SerializeField]
		private static CodeSettingsData settingsData;


		public static CodeSettingsData SettingsData
		{
			get
			{
				if (settingsData == null)
				{
					settingsData = AssetDatabase.LoadAssetAtPath<CodeSettingsData>(CodeSettingsPath);
				}
				return settingsData;
			}
		}

		#endregion
	}
}
