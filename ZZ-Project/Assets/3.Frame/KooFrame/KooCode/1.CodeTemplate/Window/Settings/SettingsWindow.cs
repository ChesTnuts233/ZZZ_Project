using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Diagnostics;
using System.IO;

namespace KooFrame
{
	public class SettingsWindow : EditorWindow
	{
		private VisualElement root => rootVisualElement;

		#region 元素

		private TextField hexoPath;

		private Button generatorHexoBtn;

		#endregion

		public static void ShowWindow()
		{
			SettingsWindow wnd = GetWindow<SettingsWindow>();
			wnd.titleContent = new GUIContent("SettingsWindow");
		}

		public void CreateGUI()
		{
			KooCode.AssetsData.SettingsWindowVisualAsset.CloneTree(root);
			hexoPath = root.Q<TextField>("HexoPath");

			generatorHexoBtn = root.Q<Button>("GeneratorHexoBtn");
			generatorHexoBtn.clicked += GeneratorHexo;
		}


		private void GeneratorHexo()
		{
			string bashPath = "D:\\Program Files\\Git\\bin\\bash.exe\"";
			string scriptPath = "C:\\Users\\Koo\\Desktop\\KooBlog_Clean\\source\\_posts\\一键生成.sh";
			string workingDirectory = "C:\\Users\\Koo\\Desktop\\KooBlog_Clean\\source\\_posts";
			string urlToOpen = "http://localhost:4000";

			if (File.Exists(scriptPath))
			{
				ExecuteCommand(bashPath, scriptPath, workingDirectory);
			}
			else
			{
				UnityEngine.Debug.LogError("Script file not found.");
			}

			Application.OpenURL(urlToOpen);

		}


		public void ExecuteCommand(string bashPath, string scriptPath, string workingDirectory)
		{
			Process process = new Process();
			process.StartInfo.FileName = bashPath;
			process.StartInfo.Arguments = $"\"{scriptPath}\"";
			process.StartInfo.WorkingDirectory = workingDirectory;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardError = true;
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.CreateNoWindow = true;

			process.OutputDataReceived += (sender, args) =>
			{
				if (!string.IsNullOrEmpty(args.Data))
				{
					UnityEngine.Debug.Log(args.Data);
				}
			};
			process.ErrorDataReceived += (sender, args) =>
			{
				if (!string.IsNullOrEmpty(args.Data))
				{
					UnityEngine.Debug.LogError(args.Data);
				}
			};

			process.Start();
			process.BeginOutputReadLine();
			process.BeginErrorReadLine();
			process.WaitForExit();
		}


		
	}
}
