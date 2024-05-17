//****************** 代码文件申明 ************************
//* 文件：AnimatorHashData                                       
//* 作者：Koo
//* 创建时间：2024/03/26 01:20:33 星期二
//* 描述：Nothing
//*****************************************************
using KooFrame;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace GameBuild
{
	[Serializable]
	public class AnimatorParameterData
	{
		[SerializeField, LabelText("参数名")]
		public string Name;

		//这个名字所属动画机
		[SerializeField, LabelText("所属动画机")]
		public List<string> AnimatorControllerNames = new();

		//这个名字可能的参数类型
		[SerializeField, LabelText("参数类型")]
		public List<AnimatorControllerParameterType> Types = new();


	}

	[CreateAssetMenu(fileName = "AnimatorHashData", menuName = "temp/动画Hash生成")]
	public class AnimatorHashData : OdinConfigBase_SO
	{

		[SerializeField, LabelText("生成代码类文件"), EnumToggleButtons, InfoBox("生成的静态Hash字段会统一生成在此静态类中")]
		private Type codeGeneratorType;

		private Type lastType;

		[LabelText("生成代码类文件的路径"), Sirenix.OdinInspector.FilePath, SerializeField]
		public string CodeGeneratorClassFilePath;

		[LabelText("动画机文件路径"), SerializeField]
		private List<string> controllerPathList = new();

		/// <summary>
		/// 动画参数名字典 string为名称 名称唯一
		/// </summary>
		[SerializeField, DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.CollapsedFoldout)]
		private Dictionary<string, AnimatorParameterData> AnimatorHashDic = new();



		private void OnValidate()
		{
			if (lastType == null || !codeGeneratorType.Equals(lastType))
			{
				CodeGeneratorClassFilePath = KooTool.FindClassFilePath(codeGeneratorType);
				CodeGeneratorClassFilePath.Log();
				lastType = codeGeneratorType;

			}

		}


#if UNITY_EDITOR // Editor-related code must be excluded from builds
		private static Color GetButtonColor()
		{
			Sirenix.Utilities.Editor.GUIHelper.RequestRepaint();
			return Color.HSVToRGB(Mathf.Cos((float)UnityEditor.EditorApplication.timeSinceStartup + 1f) * 0.225f + 0.125f, 1, 1);
		}
#endif


		/// <summary>
		/// 在Assets路径下寻找所有的AnimatorController
		/// </summary>
		[Button("生成动画Hash代码", ButtonSizes.Large), PropertyOrder(-10)]
		[GUIColor("GetButtonColor")]
		private void FindAllControler()
		{
			string[] guids = AssetDatabase.FindAssets("t:AnimatorController"); // 查找所有AnimatorController类型的资源
																			   //清空数据
			controllerPathList.Clear();
			AnimatorHashDic.Clear();
			//统计信息
			//遍历所有AnimatorController
			foreach (string guid in guids)
			{
				string path = AssetDatabase.GUIDToAssetPath(guid);
				//不包含插件内和包里的
				if (path.Contains("Plugins") || path.Contains("Packages/"))
				{
					continue;
				}
				controllerPathList.Add(path);
				if (File.Exists(KooTool.ConvertAssetPathToSystemPath(path)))
				{
					string content = File.ReadAllText(KooTool.ConvertAssetPathToSystemPath(path));

					int startIndex = content.IndexOf("m_AnimatorParameters:");
					int endIndex = content.IndexOf("m_AnimatorLayers:");

					string extractedText = content.Substring(startIndex, endIndex - startIndex);

					if (extractedText.Contains("m_AnimatorParameters: []"))
					{
						continue;
					}


					//controllerList.Add(extractedText);

					(path + "\n" + extractedText).Log();


					MatchCollection matches = Regex.Matches(extractedText, @"- m_Name: (\w+).*\s+m_Type: (\d+)");
					foreach (Match match in matches)
					{
						string name = match.Groups[1].Value;
						int type = int.Parse(match.Groups[2].Value);
						if (AnimatorHashDic.TryAdd(name, new AnimatorParameterData { Name = name }))
						{
							//字典添加成功后
							AnimatorHashDic[name].AnimatorControllerNames.Add(Path.GetFileNameWithoutExtension(path));
							AnimatorHashDic[name].Types.Add((AnimatorControllerParameterType)type);
						}
						else
						{
							//字典中已经有了
							continue;
						}

					}
				}


			}

			string generatorContent = "";


			//生成代码和XML注释 如果data的Name重复的话 在注释上申明所属的动画机名称 和 可能的参数类型
			foreach (var item in AnimatorHashDic)
			{
				generatorContent += $"\t\t/// <summary>\n";
				generatorContent += $"\t\t/// {item.Value.Name} 所属动画机：{string.Join(",", item.Value.AnimatorControllerNames)}\n";
				generatorContent += $"\t\t/// 可能的参数类型：{string.Join(",", item.Value.Types)}\n";
				generatorContent += $"\t\t/// </summary>\n";
				generatorContent += $"\t\tpublic static readonly int {item.Key}Hash = Animator.StringToHash(\"{item.Key}\");\n";
			}

			//生成类文件
			KooTool.CodeGenerator_ByTag(generatorContent, CodeGeneratorClassFilePath);


			"动画参数Hash生成完毕".Log();
		}



	}
}

