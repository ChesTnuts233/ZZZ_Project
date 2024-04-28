#if UNITY_EDITOR

using System.IO;
using KooFrame.BaseSystem;
using UnityEditor;
using UnityEngine;


namespace KooFrame
{
    public partial class FrameMenu
    {
        [MenuItem("KooFrame/生成框架UnityPackage", false, 100)]
        private static void GeneratorFramePackage()
        {
            KooFrameInstance.GeneratorUnityPackage();
        }

        [MenuItem("KooFrame/存档工具/打开存档路径", false, 0)]
        public static void OpenArchivedDirPath()
        {
            string path = Application.persistentDataPath.Replace("/", "\\");
            System.Diagnostics.Process.Start("explorer.exe", path);
        }

        [MenuItem("KooFrame/存档工具/清空存档", false, 0)]
        public static void CleanSave()
        {
            SaveSystem.DeleteAll();
        }

        [MenuItem("KooFrame/查询工具/查询脚本Static引用", false, 0)]
        public static void StaticReport()
        {
            FindStaticRef.StaticRef();
        }

        [MenuItem("KooFrame/查询工具/对象池状态查看器", false, 0)]
        public static void ShowPoolSystemViewer()
        {
            PoolSystemViewer.ShowExample();
        }
#if ENABLE_ADDRESSABLES
        [MenuItem("KooFrame/资源工具/生成资源引用代码", false, 0)]
        public static void GenerateResReferenceCode()
        {
            GenerateResReferenceCodeTool.GenerateResReferenceCode();
        }

        [MenuItem("KooFrame/资源工具/清理资源引用代码", false, 0)]
        public static void CleanResReferenceCode()
        {
            GenerateResReferenceCodeTool.CleanResReferenceCode();
        }
#endif

        [MenuItem("KooFrame/脚本工具/自定义脚本模板", false, 0)]
        public static void CreateScriptsTemplates()
        {
            EditorWindow.CreateWindow<ScriptsTemplateEditorWindow>();
        }


        [MenuItem("Assets/KooFrame/FrameSetting/CreateFrameSetting", false, 30)]
        private static void CreateFrameSetting()
        {
            FrameSetting.CreateFrameSetting();
        }

        [MenuItem("Assets/KooFrame/LocalizationConfig/CreateLocalizationConfig", false, 30)]
        private static void CreateLocalizationConfig() { }


        /// <summary>
        /// 生产项目推荐文件夹结构
        /// </summary>
        [MenuItem("KooFrame/脚本工具/生成推荐目录结构", false, 30)]
        private static void GeneratorProjectFolds()
        {
            string assetsPath = Application.dataPath;

            string[] foldsPath = { "/0.GameArts", "/1.GameBuild(Scripts)", "/2.SubSystem" };

            foreach (var path in foldsPath)
            {
                if (!Directory.Exists(assetsPath + path))
                {
                    //如果路径不存在 生成目录
                    Directory.CreateDirectory(assetsPath + path);
                }
            }

            KooTool.SaveAndRefresh();
        }
    }
}
#endif