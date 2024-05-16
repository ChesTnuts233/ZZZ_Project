#if UNITY_EDITOR


using System;
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

			string targetFilePath = locationPath + "/" + scriptName + ".cs";

			ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
				ScriptableObject.CreateInstance<MyDoCreateScriptAssetInString_Mono>(),
				targetFilePath, null,
				content);

		}





		public static void CreateScriptByContentAndPath(string scriptName, string path, string content)
		{
			string targetFilePath = path + "/" + scriptName + ".cs";

			MyDoCreateScriptAssetInString_Mono.CreateScriptAssetInContent(targetFilePath, content);
		}

		/// <summary>
		/// 创建节点到目录树中
		/// </summary>
		private static void CreateNodeInTree(string path, string scriptName)
		{
			string targetPath = path + "/" + Path.GetFileName(path) + ".asset";

			if (!File.Exists(targetPath)) //如果存在
			{
				return;
			}
			DirectoryData treeData = AssetDatabase.LoadAssetAtPath<DirectoryData>(targetPath);

			var createNode = treeData.CreateNode(typeof(UMLNode));

			createNode.NodeNickName = scriptName;
			createNode.NodeNickName.Log();
			EditorUtility.SetDirty(createNode);
		}


		public static string GetSelectedPathOrFallback()
		{
			string path = "Assets";
			foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
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
				UnityEngine.Object o = CreateScriptAssetFromTemplate(pathName, resourceFile);
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
				UnityEngine.Object o = CreateScriptAssetInContent(pathName, resourceFile);
				ProjectWindowUtil.ShowCreatedAsset(o);
			}

			public static UnityEngine.Object CreateScriptAssetInContent(string pathName, string content)
			{
				string fullPath = Path.GetFullPath(pathName);
				//StreamReader streamReader = new StreamReader(resourceFile);
				//streamReader.Close();
				string text = content;

				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(pathName);
				//替换文件名
				text = Regex.Replace(text, "#SCRIPTNAME#", fileNameWithoutExtension);

				//替换文件作者
				text = Regex.Replace(text, "#AUTHORNAME#", Environment.UserName);

				//替换文件创建时间
				text = Regex.Replace(text, "#CREATETIME#", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss dddd"));

				bool encoderShouldEmitUTF8Identifier = true;
				bool throwOnInvalidBytes = false;
				UTF8Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier, throwOnInvalidBytes);
				bool append = false;

				StreamWriter streamWriter = new StreamWriter(fullPath, append, encoding);
				streamWriter.Write(text);
				streamWriter.Close();

				CreateNodeInTree(Path.GetDirectoryName(pathName), fileNameWithoutExtension);
				AssetDatabase.ImportAsset(pathName);

				return AssetDatabase.LoadAssetAtPath(pathName, typeof(UnityEngine.Object));
			}
		}
	}
}
#endif