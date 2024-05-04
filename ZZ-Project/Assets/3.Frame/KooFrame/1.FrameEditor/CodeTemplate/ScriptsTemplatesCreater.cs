#if UNITY_EDITOR


using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace KooFrame.BaseSystem
{
	/// <summary>
	/// 代码文件建立者
	/// </summary>
	public static class ScriptsTemplatesCreater
	{
		private static MethodInfo createScriptAssetWithContentInfo;

		public static void CreateMyScript(string scriptName, string resourceFile)
		{
			string locationPath = GetSelectedPathOrFallback();
			ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
				ScriptableObject.CreateInstance<MyDoCreateScriptAsset_Mono>(),
				locationPath + "/" + scriptName, null,
				resourceFile);
		}


		public static void CreateScriptByContent(string scriptName, string content)
		{
			string locationPath = GetSelectedPathOrFallback();

			////反射得到内容创建
			//createScriptAssetWithContentInfo = typeof(ProjectWindowUtil).GetMethod("CreateScriptAssetWithContent", BindingFlags.NonPublic | BindingFlags.Static);

			string targetFilePath = locationPath + "/" + scriptName + ".cs";

			//content.Log();

			//targetFilePath.Log();

			//createScriptAssetWithContentInfo.Invoke(null, new object[] { targetFilePath, content });


			ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
				ScriptableObject.CreateInstance<MyDoCreateScriptAssetInString_Mono>(),
				targetFilePath, null,
				content);

			//return 
		}


		public static string GetSelectedPathOrFallback()
		{
			string path = "Assets";
			foreach (Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
			{
				path = AssetDatabase.GetAssetPath(obj);
				if (!string.IsNullOrEmpty(path) && File.Exists(path))
				{
					path = Path.GetDirectoryName(path);
					break;
				}
			}

			return path;
		}

		internal class MyDoCreateScriptAsset_Mono : EndNameEditAction
		{
			public override void Action(int instanceId, string pathName, string resourceFile)
			{
				Object o = CreateScriptAssetFromTemplate(pathName, resourceFile);
				ProjectWindowUtil.ShowCreatedAsset(o);
			}

			internal static UnityEngine.Object CreateScriptAssetFromTemplate(string pathName, string resourceFile)
			{
				string fullPath = Path.GetFullPath(pathName);
				StreamReader streamReader = new StreamReader(resourceFile);
				string text = streamReader.ReadToEnd();
				streamReader.Close();

				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(pathName);
				Debug.Log(fileNameWithoutExtension);
				//替换文件名

				text = Regex.Replace(text, "#SCRIPTNAME#", fileNameWithoutExtension);
				bool encoderShouldEmitUTF8Identifier = true;
				bool throwOnInvalidBytes = false;
				UTF8Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier, throwOnInvalidBytes);
				bool append = false;

				StreamWriter streamWriter = new StreamWriter(fullPath, append, encoding);
				streamWriter.Write(text);
				streamWriter.Close();
				AssetDatabase.ImportAsset(pathName);
				return AssetDatabase.LoadAssetAtPath(pathName, typeof(UnityEngine.Object));
			}
		}

		internal class MyDoCreateScriptAssetInString_Mono : EndNameEditAction
		{
			public override void Action(int instanceId, string pathName, string resourceFile)
			{
				Object o = CreateScriptAssetInContent(pathName, resourceFile);
				ProjectWindowUtil.ShowCreatedAsset(o);
			}

			internal static UnityEngine.Object CreateScriptAssetInContent(string pathName, string content)
			{
				string fullPath = Path.GetFullPath(pathName);
				//StreamReader streamReader = new StreamReader(resourceFile);
				//streamReader.Close();
				string text = content;

				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(pathName);
				Debug.Log(fileNameWithoutExtension);
				//替换文件名

				text = Regex.Replace(text, "#SCRIPTNAME#", fileNameWithoutExtension);
				bool encoderShouldEmitUTF8Identifier = true;
				bool throwOnInvalidBytes = false;
				UTF8Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier, throwOnInvalidBytes);
				bool append = false;

				StreamWriter streamWriter = new StreamWriter(fullPath, append, encoding);
				streamWriter.Write(text);
				streamWriter.Close();
				AssetDatabase.ImportAsset(pathName);
				return AssetDatabase.LoadAssetAtPath(pathName, typeof(UnityEngine.Object));
			}
		}
	}
}
#endif